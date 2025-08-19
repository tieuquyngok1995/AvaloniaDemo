using ConfigGenerator.Utils;

namespace ConfigGenerator.Models;

public class ExchangeSyncSettingsModel
{
    public int GroupwareMode = AppConstants.EXCHANGE_GROUP_WARE;

    public ExchangeSettingModel ExchangeSetting { get; set; } = ExchangeSettingModel.Default;

    public string JsonFilePath { get; set; } = string.Empty;

    public string JsonServerUrl { get; set; } = string.Empty;

    public int ExecutionIntervalInSeconds { get; set; }

    public int ConfigUpdateIntervalInMinutes { get; set; }

    public string MeetingSubject { get; set; } = string.Empty;

    public int MeetingBookingTime { get; set; }

    public int MeetingExtendTime { get; set; }
}
