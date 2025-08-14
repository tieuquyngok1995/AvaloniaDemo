using System.Collections.ObjectModel;

namespace ConfigGenerator.Models
{
    public static class AppData
    {
        public static readonly ObservableCollection<ComboBoxModel> DataOutputModes =
        [
            new ComboBoxModel(0, "JSONファイルに出力"),
            new ComboBoxModel(1, "JSONサーバーにアップデート")
        ];

        public static readonly ObservableCollection<ComboBoxModel> ExchangeConnectionSources =
        [
            new ComboBoxModel(0, "SmartRoomsサイネージ"),
            new ComboBoxModel(1, "SmartRooms管理サイト"),
            new ComboBoxModel(2, "独自設定")
        ];
    }
}
