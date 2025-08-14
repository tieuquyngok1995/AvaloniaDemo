using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConfigGenerator.ViewModels;

namespace ConfigGenerator.Models
{
    /// <summary>
    /// アプリケーション全体で利用する定数データやリストを保持する静的クラスです。
    /// 例：コンボボックスの選択肢など。
    /// </summary>
    public static class AppData
    {
        /// <summary>
        /// データ出力モードの選択肢リストです。
        /// </summary>
        public static readonly ObservableCollection<ComboBoxModel> DataOutputModes =
        [
            new ComboBoxModel(0, "JSONファイルに出力"),
            new ComboBoxModel(1, "JSONサーバーにアップデート")
        ];

        /// <summary>
        /// Exchange接続元の選択肢リストです。
        /// </summary>
        public static readonly ObservableCollection<ComboBoxModel> ExchangeConnectionSources =
        [
            new ComboBoxModel(0, "SmartRoomsサイネージ"),
            new ComboBoxModel(1, "SmartRooms管理サイト"),
            new ComboBoxModel(2, "独自設定")
        ];

        /// <summary>
        /// ナビゲーションメニューに表示する画面(View)のリストです。
        /// 各項目は対応するViewModelの型、アイコンキー、表示名を持ちます。
        /// </summary>
        public static readonly List<NavigationMenuItem> Views =
        [
            new NavigationMenuItem(typeof(SensorDataCollectorSettingsViewModel), "SensorIcon", "Sensor Data Collector"),
            new NavigationMenuItem(typeof(ExchangeSyncSettingsViewModel), "ExchangeIcon", "Exchange Sync Settings"),
            new NavigationMenuItem(typeof(ServiceManagerSettingsViewModel), "ServiceManagerIcon", "Service Manager")
        ];
    }
}
