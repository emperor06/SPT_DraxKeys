using System.Text.Json.Serialization;

namespace DraxKeys;

public class ModConfig
{
    [JsonPropertyName("TargetParentIds")]
    public List<string> TargetParentIds { get; set; } = new List<string>();
}
