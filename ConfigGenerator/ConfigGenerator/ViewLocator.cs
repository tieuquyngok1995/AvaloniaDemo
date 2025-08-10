using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ConfigGenerator;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Func<Control?>> _locator = new();
    private readonly ConcurrentDictionary<Type, Control> _viewCache = new();
    private readonly IServiceProvider? _serviceProvider;
    private readonly bool _useCache;

    // Constructor mặc định cho XAML (Design Mode)
    public ViewLocator() : this(null, false) { }

    /// <summary>
    /// Tạo ViewLocator.
    /// - serviceProvider: nếu null sẽ fallback về Activator (không dùng DI)
    /// - useCache: nếu true sẽ cache View instances để reuse (giữ trạng thái View giữa các lần hiển thị)
    /// </summary>
    public ViewLocator(IServiceProvider? serviceProvider = null, bool useCache = false)
    {
        _serviceProvider = serviceProvider;
        _useCache = useCache;

        // Đăng ký tự động theo convention: 'SomethingViewModel' -> 'SomethingView'
        AutoRegisterViews();
    }

    /// <summary>
    /// Hỗ trợ đăng ký thủ công (nếu cần)
    /// </summary>
    public void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
    {
        RegisterViewFactory(typeof(TViewModel), typeof(TView));
    }

    /// <summary>
    /// Xây dựng Control từ viewmodel instance
    /// </summary>
    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = "No VM provided" };

        var vmType = data.GetType();

        if (!_locator.TryGetValue(vmType, out var factory))
            return new TextBlock { Text = $"VM Not Registered: {vmType.FullName}" };

        var view = factory?.Invoke();
        return view ?? new TextBlock { Text = $"View factory returned null for {vmType.FullName}" };
    }

    /// <summary>
    /// Chỉ match khi đã có factory tương ứng (chính xác hơn)
    /// </summary>
    public bool Match(object? data)
    {
        return data != null && _locator.ContainsKey(data.GetType());
    }

    // ---------------------- Helpers ----------------------

    private void RegisterViewFactory(Type viewModelType, Type viewType)
    {
        if (!typeof(Control).IsAssignableFrom(viewType))
            throw new ArgumentException($"{viewType.FullName} is not a Control.");

        // Nếu đã đăng ký -> ignore
        if (_locator.ContainsKey(viewModelType))
            return;

        Func<Control?> factory = () =>
        {
            // Nếu đang ở design mode, tạo trực tiếp để designer hoạt động ổn định
            if (Design.IsDesignMode)
                return Activator.CreateInstance(viewType) as Control;

            // Nếu cache được bật, thử lấy từ cache
            if (_useCache)
            {
                return _viewCache.GetOrAdd(viewModelType, _ => CreateInstance(viewType));
            }

            // Không cache -> mỗi lần tạo mới một instance
            return CreateInstance(viewType);
        };

        _locator.Add(viewModelType, factory);
    }

    private Control? CreateInstance(Type viewType)
    {
        // Thử DI container trước nếu có
        if (_serviceProvider != null)
        {
            var svc = _serviceProvider.GetService(viewType) as Control;
            if (svc != null)
                return svc;
        }

        // Fallback: tạo instance trực tiếp (giúp đơn giản và tương thích)
        return Activator.CreateInstance(viewType) as Control;
    }

    private void AutoRegisterViews()
    {
        // Lấy tất cả assembly hiện có (bỏ assembly động để tránh lỗi)
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .ToArray();

        foreach (var asm in assemblies)
        {
            Type[] types;
            try
            { types = asm.GetTypes(); }
            catch { continue; }

            var viewModels = types
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel", StringComparison.Ordinal))
                .ToArray();

            foreach (var vm in viewModels)
            {
                // cố gắng tìm view theo nhiều cách:
                // 1) thay "ViewModel" -> "View" giữ namespace
                // 2) tìm type cùng tên trong bất kỳ assembly nào

                var viewCandidateFullName = vm.FullName!.Replace("ViewModel", "View");
                var viewType = Type.GetType(viewCandidateFullName);

                if (viewType == null)
                {
                    // tìm trong cùng assembly: namesapce + tên
                    var viewName = vm.Namespace is null
                        ? vm.Name.Replace("ViewModel", "View")
                        : vm.Namespace + "." + vm.Name.Replace("ViewModel", "View");

                    viewType = asm.GetType(viewName);
                }

                // nếu vẫn null, cố gắng tìm bất kỳ type nào có tên tương ứng ở các assembly khác
                if (viewType == null)
                {
                    var simpleViewName = vm.Name.Replace("ViewModel", "View");
                    viewType = assemblies
                        .SelectMany(a =>
                        {
                            try
                            { return a.GetTypes(); }
                            catch { return Array.Empty<Type>(); }
                        })
                        .FirstOrDefault(t => t.Name.Equals(simpleViewName, StringComparison.Ordinal));
                }

                if (viewType != null && typeof(Control).IsAssignableFrom(viewType))
                {
                    RegisterViewFactory(vm, viewType);
                }
            }
        }
    }
}
