using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConfigGenerator.Models;
using ConfigGenerator.Service;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// Exchange連携設定画面用のViewModelクラスです。
/// 接続元や出力モードの選択、リソースマッピングテーブルの管理、
/// ファイル選択ダイアログの呼び出しなどを行います。
/// </summary>
public partial class ExchangeSyncSettingsViewModel(IFilePickerService filePickerService) : ViewModelBase
{
    [ObservableProperty]
    private ExchangeSyncSettingsUiState _uiState = new();

    [ObservableProperty]
    private ExchangeSyncSettingsModel _dataModel = new();

    // ファイル選択サービス
    private readonly IFilePickerService _filePickerService = filePickerService;

    /// <summary>
    /// ファイル選択ダイアログを表示するコマンド
    /// </summary>
    [RelayCommand]
    private async Task SelectFilesAsync()
    {
        UiState.SelectedFiles = await _filePickerService.PickFilesAsync(null);
    }

    /// <summary>
    /// デザイン時用のコンストラクタ
    /// </summary>
    public ExchangeSyncSettingsViewModel()
    : this(new DesignTimeFilePickerService())
    {
    }

    /// <summary>
    /// デザイン時用のダミーサービス
    /// </summary>
    private class DesignTimeFilePickerService : IFilePickerService
    {
        public Task<string?> PickFilesAsync(string? title = null)
            => Task.FromResult<string?>("sample.txt");
    }
}
