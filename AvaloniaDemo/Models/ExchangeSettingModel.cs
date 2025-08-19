using ConfigGenerator.Utils;

namespace ConfigGenerator.Models;

/// <summary>
/// Exchange サーバーの設定情報を管理するクラス。
/// </summary>
public class ExchangeSettingModel
{
    public int ExchangeConnectionSource { get; set; }

    //  SmartRoomsサイネージの設定ファイルを読み込む
    public string SignageSettingsFilePath { get; set; } = string.Empty;

    // アプリで記録された情報
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string EndPointUrl { get; set; } = string.Empty;

    // SmartRooms管理サイトから取得する
    public string SiteManagerApiUrl { get; set; } = string.Empty;
    public string ConsumerKey { get; set; } = string.Empty;
    public string ConsumerSecret { get; set; } = string.Empty;
    // EndPointUrl をサイトマネージャーにマッピングする
    public string ExchangeOnlineEndpointUrl { get; set; } = string.Empty;

    // ExchangeServer on-premises 用、今後拡張予定。
    public string UserId { get; set; } = string.Empty;
    public string UserPassword { get; set; } = string.Empty;

    public string UserMailAddress = AppConstants.EXCHANGE_USER_MAIL_ADDRESS;
    public int ParallCount = AppConstants.EXCHANGE_PARALL_COUNT;
    public int MaxRetry = AppConstants.EXCHANGE_MAX_RETRY;

    public static ExchangeSettingModel Default => new();
}
