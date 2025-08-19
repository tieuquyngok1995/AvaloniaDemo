using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ConfigGenerator.Models;

namespace ConfigGenerator.ViewModels;

/// <summary>
/// センサーデータコレクター設定画面用のViewModelクラスです。
/// 出力モードや各種表示状態の管理を行います。
/// </summary>
public partial class SensorDataCollectorSettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private SensorDataCollectorUiState _uiState = new();

    [ObservableProperty]
    private SensorDataCollectorSettingsModel _dataModel = new();

    public void SaveToModel()
    {
        DataModel.UpdateEndpointsAndRetry(
            UiState.JsonServerEndpoints.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries),
            UiState.RoomSenseEndpoints.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries));
    }
}
