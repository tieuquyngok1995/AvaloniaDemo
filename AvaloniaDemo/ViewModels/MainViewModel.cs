using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvaloniaDemo.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Interfaces;
using ConfigGenerator.Models;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// アプリケーションのメイン画面のViewModelクラスです。
/// ナビゲーションメニューの項目管理、
/// ペインの開閉状態や現在表示中のページの管理などを行います。
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly IMessageBoxService _messageBox;
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
    public MainViewModel(IMessageBoxService messageBox, IFolderProcessor folderProcessor, IFileProcessor fileProcessor)
    {
        _messageBox = messageBox;
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
    private async Task GenerateAndSaveConfigAsync()
    {
        //bool confirm = await _messageBox.Show("Xác nhận", "Bạn có muốn xóa không?");
    }



    [RelayCommand]
    public async Task ShowInfo()
    {
        System.Diagnostics.Debug.WriteLine("ShowInfo called!");
        try
        {
            await _messageBox.ShowInfoAsync("Test message!", "Test");
            System.Diagnostics.Debug.WriteLine("MessageBox shown successfully!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task ShowWarning()
    {
        await _messageBox.ShowWarningAsync("Cảnh báo: Dữ liệu có thể bị mất!");
    }

    [RelayCommand]
    public async Task ShowError()
    {
        await _messageBox.ShowErrorAsync("Lỗi kết nối database!", "Lỗi");
    }

    [RelayCommand]
    public async Task ShowQuestion()
    {
        var result = await _messageBox.ShowYesNoAsync("Bạn có muốn lưu thay đổi?", "Xác nhận");

        if (result == MessageBoxResult.Yes)
        {
            await _messageBox.ShowInfoAsync("Đã lưu thành công!");
        }
    }
}
