using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Interfaces;
using ConfigGenerator.Models;
using ConfigGenerator.Utils;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// アプリケーションのメイン画面のViewModelクラスです。
/// ナビゲーションメニューの項目管理、
/// ペインの開閉状態や現在表示中のページの管理などを行います。
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly IFolderProcessor _folderProcessor;
    private readonly IFileProcessor _fileProcessor;

    // ナビゲーションメニューの項目リスト
    public ObservableCollection<NavigationMenuItem> MenuItems { get; }

    // ペインが開いているかどうかを示すフラグ
    [ObservableProperty]
    private bool _isPaneOpen;

    // 選択中のナビゲーションメニュー項目
    [ObservableProperty]
    private NavigationMenuItem? _selectedMenuItem;

    // ナビゲーションメニューの項目リスト（内部用）
    private readonly List<NavigationMenuItem> _listView = AppData.Views;

    // 現在表示中のページのViewModel
    [ObservableProperty]
    private ViewModelBase _currentPage = new SensorDataCollectorSettingsViewModel();

    /// <summary>
    /// MainViewModelのコンストラクタ
    /// </summary>
    public MainViewModel(IFolderProcessor folderProcessor, IFileProcessor fileProcessor)
    {
        _folderProcessor = folderProcessor;
        _fileProcessor = fileProcessor;
        MenuItems = new ObservableCollection<NavigationMenuItem>(_listView);
        SelectedMenuItem = MenuItems.First(vm => vm.ModelType == typeof(SensorDataCollectorSettingsViewModel));
    }

    /// <summary>
    /// ペインの開閉を切り替えるコマンド
    /// </summary>
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    /// <summary>
    /// 選択中のナビゲーションメニュー項目が変更されたときの処理
    /// </summary>
    /// <param name="value">新しく選択された項目</param>
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

    [RelayCommand]
    private void GenerateAndSaveConfig()
    {
        _folderProcessor.CreateFolder(AppConstants.SENSOR_DATA_COLLECTOR_CONFIG_FOLDER)
            .Match(
                Right: _ =>
                {
                    _fileProcessor.CreateFile(AppConstants.SENSOR_DATA_COLLECTOR_CONFIG_FILE);
                    if (CurrentPage is SensorDataCollectorSettingsViewModel sensorVm)
                    {
                        sensorVm.SaveToModel();
                        _fileProcessor.WriteToJsonFile(sensorVm.DataModel, AppConstants.SENSOR_DATA_COLLECTOR_CONFIG_FILE);
                    }
                },
                Left: _ => { Console.WriteLine("Test"); }
            );
    }
}
