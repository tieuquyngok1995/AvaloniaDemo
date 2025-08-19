using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ConfigGenerator.Models;

public partial class SensorDataCollectorUiState : ObservableObject
{
    // JSONサーバーが有効な場合のカラム位置を返します。
    public int JsonServerColumn => UseJsonServer ? 0 : (UseCloud ? 0 : 0);

    // クラウドが有効な場合のカラム位置を返します。
    public int CloudColumn => UseCloud ? (UseJsonServer ? 1 : 0) : 1;

    // 出力モードが「JSONファイル」の場合にtrueを返します。
    public bool UseJsonFileVisible => SelectedDataOutputMode?.Key == 0;

    // 出力モードが「JSONサーバー」の場合にtrueを返します。
    public bool UseJsonServerVisible => SelectedDataOutputMode?.Key == 1;

    // データ出力モードの選択肢リストです。
    public ObservableCollection<ComboBoxModel> DataOutputModes { get; } = AppData.DataOutputModes;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JsonServerColumn))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _useJsonServer = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JsonServerColumn))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _useCloud = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UseJsonFileVisible))]
    [NotifyPropertyChangedFor(nameof(UseJsonServerVisible))]
    private ComboBoxModel? _selectedDataOutputMode;

    [ObservableProperty]
    private string _jsonServerEndpoints = string.Empty;

    [ObservableProperty]
    public string _roomSenseEndpoints = string.Empty;
}
