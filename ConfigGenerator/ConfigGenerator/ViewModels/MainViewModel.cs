using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Models;

namespace ConfigGenerator.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {

        Items = new ObservableCollection<ListItemTemplate>(_templates);

        SelectedListItem = Items.First(vm => vm.ModelType == typeof(SensorDataCollectorSettingsViewModel));
    }

    private readonly List<ListItemTemplate> _templates =
    [
        new ListItemTemplate(typeof(SensorDataCollectorSettingsViewModel), "HomeRegular", "SensorDataCollector"),
        new ListItemTemplate(typeof(ExchangeSyncSettingsViewModel), "CursorHoverRegular", "ExchangeSyncSettings"),
        new ListItemTemplate(typeof(ServiceManagerSettingsViewModel), "TextNumberFormatRegular", "ServiceManager")
    ];


    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    private ViewModelBase _currentPage = new SensorDataCollectorSettingsViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
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

    public ObservableCollection<ListItemTemplate> Items { get; }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}
