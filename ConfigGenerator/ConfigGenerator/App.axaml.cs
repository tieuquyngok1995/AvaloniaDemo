using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using ConfigGenerator.ViewModels;
using ConfigGenerator.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigGenerator;

/// <summary>
/// アプリケーションのエントリーポイントとなるクラスです。
/// DIコンテナの構成やViewModel/Viewの登録、メインウィンドウの初期化などを行います。
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// XAMLリソースの初期化を行います。
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// フレームワーク初期化完了時の処理を行います。
    /// DIサービスの構成、ViewLocatorの登録、メインウィンドウの生成などを実施します。
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        var locator = new ViewLocator();
        DataTemplates.Add(locator);

        var services = new ServiceCollection();
        ConfigureViewModels(services);
        ConfigureViews(services);

        ServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);

        MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // デスクトップアプリの場合、MainWindowを生成
            desktop.MainWindow = new MainWindow(vm);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            // シングルビューアプリの場合、MainViewを生成
            singleViewPlatform.MainView = new MainView { DataContext = vm };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// ViewModelのDI登録を行います。
    /// </summary>
    internal static void ConfigureViewModels(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddTransient<SensorDataCollectorSettingsViewModel>();
        services.AddTransient<ExchangeSyncSettingsViewModel>();
        services.AddTransient<ServiceManagerSettingsViewModel>();
    }

    /// <summary>
    /// ViewのDI登録を行います。
    /// </summary>
    internal static void ConfigureViews(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddTransient<SensorDataCollectorSettingsViewModel>();
        services.AddTransient<ExchangeSyncSettingsViewModel>();
        services.AddTransient<ServiceManagerSettingsViewModel>();
    }
}
