using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConfigGenerator.Models;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(SensorDataCollectorSettingsModel))]
internal partial class JsonSerializationContext : JsonSerializerContext
{
}
