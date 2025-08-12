using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ConfigGenerator;

/// <summary>
/// ViewModelから対応するViewを解決し、インスタンスを生成するためのロケータークラスです。
/// DIコンテナやキャッシュ機能にも対応しています。
/// </summary>
public class ViewLocator : IDataTemplate
{
    // ViewModel型とView生成ファクトリのマッピング
    private readonly Dictionary<Type, Func<Control?>> _locator = [];
    // Viewのキャッシュ（useCache=trueの場合のみ利用）
    private readonly ConcurrentDictionary<Type, Control?> _viewCache = new();
    // DIサービスプロバイダー
    private readonly IServiceProvider? _serviceProvider;
    // キャッシュ利用フラグ
    private readonly bool _useCache;

    /// <summary>
    /// デザインモード用のデフォルトコンストラクタ
    /// </summary>
    public ViewLocator() : this(null, false) { }

    /// <summary>
    /// ViewLocatorを生成します。
    /// serviceProvider: DIコンテナ。nullの場合はActivatorでインスタンス生成。
    /// useCache: trueの場合、Viewインスタンスをキャッシュし再利用します。
    /// </summary>
    public ViewLocator(IServiceProvider? serviceProvider = null, bool useCache = false)
    {
        _serviceProvider = serviceProvider;
        _useCache = useCache;

        // 命名規則に従い自動でViewを登録
        AutoRegisterViews();
    }

    /// <summary>
    /// 手動でViewModelとViewのファクトリを登録します。
    /// </summary>
    public void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
    {
        RegisterViewFactory(typeof(TViewModel), typeof(TView));
    }

    /// <summary>
    /// ViewModelインスタンスから対応するView(Control)を生成します。
    /// </summary>
    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "No VM provided" };
        }

        Type vmType = data.GetType();

        if (!_locator.TryGetValue(vmType, out Func<Control?>? factory))
        {
            return new TextBlock { Text = $"VM Not Registered: {vmType.FullName}" };
        }

        Control? view = factory?.Invoke();
        return view ?? new TextBlock { Text = $"View factory returned null for {vmType.FullName}" };
    }

    /// <summary>
    /// 対応するViewファクトリが登録されている場合のみマッチします。
    /// </summary>
    public bool Match(object? data)
    {
        return data != null && _locator.ContainsKey(data.GetType());
    }

    /// <summary>
    /// ViewModel型とView型を指定してファクトリを登録します。
    /// </summary>
    private void RegisterViewFactory(Type viewModelType, Type viewType)
    {
        if (!typeof(Control).IsAssignableFrom(viewType))
        {
            throw new ArgumentException($"{viewType.FullName} is not a Control.");
        }

        // 既に登録済みの場合は何もしない
        if (_locator.ContainsKey(viewModelType))
        {
            return;
        }

        Control? factory()
        {
            // デザインモード時は直接インスタンス生成
            if (Design.IsDesignMode)
            {
                return Activator.CreateInstance(viewType) as Control;
            }

            // キャッシュ利用時はキャッシュから取得または生成
            if (_useCache)
            {
                return _viewCache.GetOrAdd(viewModelType, valueFactory: _ => CreateInstance(viewType));
            }

            // 毎回新規インスタンス生成
            return CreateInstance(viewType);
        }

        _locator.Add(viewModelType, factory);
    }

    /// <summary>
    /// Viewのインスタンスを生成します（DI優先、なければActivator）。
    /// </summary>
    private Control? CreateInstance(Type viewType)
    {
        // DIコンテナから取得
        if (_serviceProvider != null)
        {
            if (_serviceProvider.GetService(viewType) is Control svc)
            {
                return svc;
            }
        }

        // Fallback: 直接インスタンス生成
        return Activator.CreateInstance(viewType) as Control;
    }

    /// <summary>
    /// 命名規則（ViewModel→View）に従い、全てのViewModelに対応するViewを自動登録します。
    /// </summary>
    private void AutoRegisterViews()
    {
        // 全アセンブリを取得（動的アセンブリは除外）
        System.Reflection.Assembly[] assemblies = [.. AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic)];

        foreach (System.Reflection.Assembly asm in assemblies)
        {
            Type[] types;
            try
            { types = asm.GetTypes(); }
            catch { continue; }

            Type[] viewModels = [.. types.Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel", StringComparison.Ordinal))];

            foreach (Type? vm in viewModels)
            {
                // ViewModel名からView名を推測して検索
                var viewCandidateFullName = vm.FullName!.Replace("ViewModel", "View");
                var viewType = Type.GetType(viewCandidateFullName);

                if (viewType == null)
                {
                    // 同一アセンブリ内で検索
                    var viewName = vm.Namespace is null
                        ? vm.Name.Replace("ViewModel", "View")
                        : vm.Namespace + "." + vm.Name.Replace("ViewModel", "View");

                    viewType = asm.GetType(viewName);
                }

                // 他アセンブリも含めて型名で検索
                if (viewType == null)
                {
                    var simpleViewName = vm.Name.Replace("ViewModel", "View");
                    viewType = assemblies
                        .SelectMany(a =>
                        {
                            try
                            { return a.GetTypes(); }
                            catch { return []; }
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
