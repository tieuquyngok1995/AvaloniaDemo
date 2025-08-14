using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ConfigGenerator.Models;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// センサーデータコレクター設定画面用のViewModelクラスです。
/// 出力モードや各種表示状態の管理を行います。
/// </summary>
public partial class SensorDataCollectorSettingsViewModel : ViewModelBase
{
    /// <summary>
    /// JSONサーバーが有効な場合のカラム位置を返します。
    /// </summary>
    public int JsonServerColumn => IsJsonServer ? 0 : (IsCloud ? 0 : 0);

    /// <summary>
    /// クラウドが有効な場合のカラム位置を返します。
    /// </summary>
    public int CloudColumn => IsCloud ? (IsJsonServer ? 1 : 0) : 1;

    /// <summary>
    /// 出力モードが「JSONファイル」の場合にtrueを返します。
    /// </summary>
    public bool IsJsonFileVisible => SelectedDataOutputMode?.Key == 0;

    /// <summary>
    /// 出力モードが「JSONサーバー」の場合にtrueを返します。
    /// </summary>
    public bool IsJsonServerVisible => SelectedDataOutputMode?.Key == 1;

    /// <summary>
    /// データ出力モードの選択肢リストです。
    /// </summary>
    public ObservableCollection<ComboBoxModel> DataOutputModes { get; } = AppData.DataOutputModes;

    /// <summary>
    /// JSONサーバーが有効かどうかを示します。
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JsonServerColumn))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _isJsonServer = false;

    /// <summary>
    /// クラウドが有効かどうかを示します。
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonServer))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _isCloud = false;

    /// <summary>
    /// 選択中のデータ出力モードです。
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonFileVisible))]
    [NotifyPropertyChangedFor(nameof(IsJsonServerVisible))]
    private ComboBoxModel? _selectedDataOutputMode;
}
