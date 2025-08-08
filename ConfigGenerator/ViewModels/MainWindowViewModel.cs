using Avalonia.Controls;
using ConfigGenerator.Views;
using ReactiveUI;
using System.Reactive;

namespace ConfigGenerator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private UserControl _currentView;
    private string _currentViewTitle = "ホーム";

    private bool _isSensorDataCollectorSelected = true;
    private bool _isExchangeSyncSelected;
    private bool _isSystemSettingsSelected;

    // Views
    private readonly SensorDataCollectorView _sensorDataCollectorView;
    private readonly ExchangeSyncView _exchangeSyncView;
    //private readonly SystemSettingsView _systemSettingsView;
    public MainWindowViewModel()
    {
        // Initialize views
        _sensorDataCollectorView = new SensorDataCollectorView();
        _exchangeSyncView = new ExchangeSyncView();
        //_systemSettingsView = new SystemSettingsView();

        // Initialize commands
        ShowSensorDataCollectorViewCommand = ReactiveCommand.Create(() =>
        {
            System.Diagnostics.Debug.WriteLine("Command executed");
            ShowSensorDataCollectorView();
        });
        ShowExchangeSyncViewCommand = ReactiveCommand.Create(ShowExchangeSyncView);
        ShowSystemSettingsViewCommand = ReactiveCommand.Create(ShowSystemSettingsView);

        // Set default view
        CurrentView = _sensorDataCollectorView;
    }

    // Properties and commands
    public UserControl CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public string CurrentViewTitle
    {
        get => _currentViewTitle;
        set => this.RaiseAndSetIfChanged(ref _currentViewTitle, value);
    }

    public bool IsSensorDataCollectorSelected
    {
        get => _isSensorDataCollectorSelected;
        set => this.RaiseAndSetIfChanged(ref _isSensorDataCollectorSelected, value);
    }

    public bool IsExchangeSyncSelected
    {
        get => _isExchangeSyncSelected;
        set => this.RaiseAndSetIfChanged(ref _isExchangeSyncSelected, value);
    }

    public bool IsSystemSettingsSelected
    {
        get => _isSystemSettingsSelected;
        set => this.RaiseAndSetIfChanged(ref _isSystemSettingsSelected, value);
    }

    // Command definitions
    public ReactiveCommand<Unit, Unit> ShowSensorDataCollectorViewCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowExchangeSyncViewCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowSystemSettingsViewCommand { get; }

    // Navigation methods
    private void ShowSensorDataCollectorView()
    {
        CurrentView = _sensorDataCollectorView;
        CurrentViewTitle = "センサーデータ収集設定";

        IsSensorDataCollectorSelected = true;
        IsExchangeSyncSelected = false;
        IsSystemSettingsSelected = false;
    }

    private void ShowExchangeSyncView()
    {
        CurrentView = _exchangeSyncView;
        CurrentViewTitle = "Exchange連携設定";

        IsSensorDataCollectorSelected = false;
        IsExchangeSyncSelected = true;
        IsSystemSettingsSelected = false;
    }

    private void ShowSystemSettingsView()
    {
        //CurrentView = _systemSettingsView;
        CurrentViewTitle = "システム設定";

        IsSensorDataCollectorSelected = false;
        IsExchangeSyncSelected = false;
        IsSystemSettingsSelected = true;
    }
}
