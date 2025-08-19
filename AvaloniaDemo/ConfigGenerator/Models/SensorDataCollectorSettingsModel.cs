using ConfigGenerator.Utils;

namespace ConfigGenerator.Models;

public class SensorDataCollectorSettingsModel
{
    public int ExecutionIntervalInSeconds { get; set; }

    public int DataInputMode { get; set; }

    public string[] JsonServerGetEndpoints { get; set; } = [];

    public string[] RoomSenseGetEndpoints { get; set; } = [];

    public int DataOutputMode { get; set; }

    public string JsonFilePath { get; set; } = string.Empty;

    public string JsonServerUrl { get; set; } = string.Empty;

    public int MaxRetry { get; set; }

    public void UpdateEndpointsAndRetry(string[] jsonServerEndpoints, string[] roomSenseEndpoints)
    {
        JsonServerGetEndpoints = jsonServerEndpoints;
        RoomSenseGetEndpoints = roomSenseEndpoints;
        MaxRetry = AppConstants.SENSOR_MAX_RETRY;
    }
}
