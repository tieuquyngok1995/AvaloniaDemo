using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Models;
using ConfigGenerator.Service;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// アプリケーションのメイン画面のViewModelクラスです。
/// ナビゲーションメニューの項目管理、
/// ペインの開閉状態や現在表示中のページの管理などを行います。
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<NavigationMenuItem> MenuItems { get; }

    private readonly List<NavigationMenuItem> _templates =
    [
        new NavigationMenuItem(typeof(SensorDataCollectorSettingsViewModel), "SensorIcon", "Sensor Data Collector"),
        new NavigationMenuItem(typeof(ExchangeSyncSettingsViewModel), "ExchangeIcon", "Exchange Sync Settings"),
        new NavigationMenuItem(typeof(ServiceManagerSettingsViewModel), "ServiceManagerIcon", "Service Manager")
    ];

    public MainViewModel()
    {
        MenuItems = new ObservableCollection<NavigationMenuItem>(_templates);
        SelectedMenuItem = MenuItems.First(vm => vm.ModelType == typeof(ExchangeSyncSettingsViewModel));
    }

    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    //private ViewModelBase _currentPage = new ExchangeSyncSettingsViewModel();
    private ViewModelBase _currentPage = new ExchangeSyncSettingsViewModel(new FilePickerService());

    [ObservableProperty]
    private NavigationMenuItem? _selectedMenuItem;

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    partial void OnSelectedMenuItemChanged(NavigationMenuItem? value)
    {
        if (value is null)
            return;

        var vm = Design.IsDesignMode
            ? Activator.CreateInstance(value.ModelType)
            : Ioc.Default.GetService(value.ModelType);

        if (vm is not ViewModelBase vmb)
            return;

        CurrentPage = vmb;
    }
}
