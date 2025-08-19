using System;
using System.IO;

namespace ConfigGenerator.Utils;

public class AppConstants
{
    public static readonly String SENSOR_DATA_COLLECTOR_CONFIG_FOLDER = Path.Combine(AppContext.BaseDirectory, "SensorDataCollector");
    public static readonly string SENSOR_DATA_COLLECTOR_CONFIG_FILE = Path.Combine(SENSOR_DATA_COLLECTOR_CONFIG_FOLDER, "config.json");

    public static readonly String EXCHANGE_DATA_SYNC_CONFIG_FOLDER = Path.Combine(AppContext.BaseDirectory, "ExchangeDataSync");
    public static readonly string EXCHANGE_DATA_SYNC_CONFIG_FILE = Path.Combine(EXCHANGE_DATA_SYNC_CONFIG_FOLDER, "config.json");

    public static readonly String SERVICE_MANAGER_CONFIG_FOLDER = Path.Combine(AppContext.BaseDirectory, "ServiceManager");
    public static readonly string SERVICE_MANAGER_CONFIG_FILE = Path.Combine(SERVICE_MANAGER_CONFIG_FOLDER, "config.json");

    #region [SensorDataCollector] 初期値設定
    public const int SENSOR_MAX_RETRY = 2;
    #endregion

    #region [ExchangeDataSync] 初期値設定
    public const int EXCHANGE_GROUP_WARE = 0;
    public const int EXCHANGE_PARALL_COUNT = 3;
    public const int EXCHANGE_MAX_RETRY = 5;
    public const string EXCHANGE_USER_MAIL_ADDRESS = "SR_Admin@uchidasns.onmicrosoft.com";
    #endregion
}
