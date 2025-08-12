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
    public int JsonServerColumn => IsJsonServer ? 0 : (IsCloud ? 0 : 0);
    public int CloudColumn => IsCloud ? (IsJsonServer ? 2 : 0) : 2;

    public bool IsJsonFileVisible => SelectedOutputMode?.Key == 0;
    public bool IsJsonServerVisible => SelectedOutputMode?.Key == 1;

    public ObservableCollection<ComboBoxModel> OutputModes { get; } =
    [
        new ComboBoxModel(0, "JSONファイルに出力"),
        new ComboBoxModel(1, "JSONサーバーにアップデート")
    ];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(JsonServerColumn))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _isJsonServer = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonServer))]
    [NotifyPropertyChangedFor(nameof(CloudColumn))]
    private bool _isCloud = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJsonFileVisible))]
    [NotifyPropertyChangedFor(nameof(IsJsonServerVisible))]
    private ComboBoxModel? _selectedOutputMode;
}
