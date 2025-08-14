using System.Collections.ObjectModel;
using System.Linq;
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
    /// <summary>
    /// 「SmartRoomsサイネージ」選択時にtrueを返します。
    /// </summary>
    public bool IsSignageVisible => SelectedExchangeConnectionSource?.Key == 0;

    /// <summary>
    /// 「SmartRooms管理サイト」選択時にtrueを返します。
    /// </summary>
    public bool IsManagementSiteVisible => SelectedExchangeConnectionSource?.Key == 1;

    /// <summary>
    /// 「独自設定」選択時にtrueを返します。
    /// </summary>
    public bool IsCustomVisible => SelectedExchangeConnectionSource?.Key == 2;

    /// <summary>
    /// 出力モードが「JSONファイル」の場合にtrueを返します。
    /// </summary>
    public bool IsJsonFileVisible => SelectedDataOutputMode?.Key == 0;

    /// <summary>
    /// 出力モードが「JSONサーバー」の場合にtrueを返します。
    /// </summary>
    public bool IsJsonServerVisible => SelectedDataOutputMode?.Key == 1;

    /// <summary>
    /// Exchangeリソースマッピングテーブルのデータコレクションです。
    /// </summary>
    public ObservableCollection<ExchangeResourceMappingModel> DataTable { get; } = [];

    /// <summary>
    /// Exchange接続元の選択肢リストです。
    /// </summary>
    public ObservableCollection<ComboBoxModel> ExchangeConnectionSources { get; } = AppData.ExchangeConnectionSources;

    /// <summary>
    /// データ出力モードの選択肢リストです。
    /// </summary>
    public ObservableCollection<ComboBoxModel> DataOutputModes { get; } = AppData.DataOutputModes;

    /// <summary>
    /// 選択中のテーブル行
    /// </summary>
    [ObservableProperty]
    private ExchangeResourceMappingModel? _selectedRow;

    /// <summary>
    /// 選択中のExchange接続元
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSignageVisible))]
    [NotifyPropertyChangedFor(nameof(IsManagementSiteVisible))]
    [NotifyPropertyChangedFor(nameof(IsCustomVisible))]
    private ComboBoxModel? _selectedExchangeConnectionSource;

    /// <summary>
    /// 選択中のデータ出力モード
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonFileVisible))]
    [NotifyPropertyChangedFor(nameof(IsJsonServerVisible))]
    private ComboBoxModel? _selectedDataOutputMode;

    /// <summary>
    /// 新しい行を追加するコマンド
    /// </summary>
    [RelayCommand]
    private void AddRow()
    {
        var newRow = new ExchangeResourceMappingModel(DataTable.Count + 1, string.Empty, string.Empty);
        DataTable.Add(newRow);

        SelectedRow = newRow;
    }

    /// <summary>
    /// 選択中の行を削除するコマンド
    /// </summary>
    [RelayCommand]
    private void RemoveSelected(ExchangeResourceMappingModel? selected)
    {
        if (selected != null)
        {
            DataTable.Remove(selected);
        }

        // 行番号を再採番
        DataTable.Select((item, index) => new { item, index })
         .ToList()
         .ForEach(x => x.item.No = x.index + 1);
    }

    /// <summary>
    /// ファイル選択サービス
    /// </summary>
    private readonly IFilePickerService _filePickerService = filePickerService;

    /// <summary>
    /// 選択されたファイル名
    /// </summary>
    [ObservableProperty]
    private string? selectedFiles;

    /// <summary>
    /// ファイル選択ダイアログを表示するコマンド
    /// </summary>
    [RelayCommand]
    private async Task SelectFilesAsync()
    {
        SelectedFiles = await _filePickerService.PickFilesAsync(null);
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
