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

/// <summary>
/// アプリケーションのメイン画面のViewModelクラスです。
/// ナビゲーションメニューの項目管理、
/// ペインの開閉状態や現在表示中のページの管理などを行います。
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    /// <summary>
    /// ナビゲーションメニューの項目リスト
    /// </summary>
    public ObservableCollection<NavigationMenuItem> MenuItems { get; }

    /// <summary>
    /// ペインが開いているかどうかを示すフラグ
    /// </summary>
    [ObservableProperty]
    private bool _isPaneOpen;

    /// <summary>
    /// 選択中のナビゲーションメニュー項目
    /// </summary>
    [ObservableProperty]
    private NavigationMenuItem? _selectedMenuItem;

    /// <summary>
    /// ナビゲーションメニューの項目リスト（内部用）
    /// </summary>
    private readonly List<NavigationMenuItem> _listView = AppData.Views;

    /// <summary>
    /// 現在表示中のページのViewModel
    /// </summary>
    [ObservableProperty]
    private ViewModelBase _currentPage = new SensorDataCollectorSettingsViewModel();

    /// <summary>
    /// MainViewModelのコンストラクタ
    /// </summary>
    public MainViewModel()
    {
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
}
