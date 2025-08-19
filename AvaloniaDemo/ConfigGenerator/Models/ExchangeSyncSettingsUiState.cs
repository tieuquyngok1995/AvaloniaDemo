using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ConfigGenerator.Models;

public partial class ExchangeSyncSettingsUiState : ObservableObject
{
    // Exchangeリソースマッピングテーブルのデータコレクションです。
    public ObservableCollection<ExchangeResourceMappingModel> DataTable { get; } = [];

    // Exchange接続元の選択肢リストです。
    public ObservableCollection<ComboBoxModel> ExchangeConnectionSources { get; } = AppData.ExchangeConnectionSources;

    // データ出力モードの選択肢リストです。
    public ObservableCollection<ComboBoxModel> DataOutputModes { get; } = AppData.DataOutputModes;

    // 「SmartRoomsサイネージ」選択時にtrueを返します。
    public bool IsSignageVisible => SelectedExchangeConnectionSource?.Key == 0;

    // 「SmartRooms管理サイト」選択時にtrueを返します。
    public bool IsManagementSiteVisible => SelectedExchangeConnectionSource?.Key == 1;

    // 「独自設定」選択時にtrueを返します。
    public bool IsCustomVisible => SelectedExchangeConnectionSource?.Key == 2;

    // 出力モードが「JSONファイル」の場合にtrueを返します。
    public bool IsJsonFileVisible => SelectedDataOutputMode?.Key == 0;

    // 出力モードが「JSONサーバー」の場合にtrueを返します。
    public bool IsJsonServerVisible => SelectedDataOutputMode?.Key == 1;

    /// 選択中のテーブル行
    [ObservableProperty]
    private ExchangeResourceMappingModel? _selectedRow;

    // 選択中のExchange接続元
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSignageVisible))]
    [NotifyPropertyChangedFor(nameof(IsManagementSiteVisible))]
    [NotifyPropertyChangedFor(nameof(IsCustomVisible))]
    private ComboBoxModel? _selectedExchangeConnectionSource;

    // 選択されたファイル名
    [ObservableProperty]
    private string? _selectedFiles;

    // 選択中のデータ出力モード
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
}
