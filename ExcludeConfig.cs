using System.Text.Json.Serialization;

namespace DraxKeys;

public class ExcludeConfig
{
    [JsonPropertyName("ExcludeItemIds")]
    public List<string> ExcludeItemIds { get; set; } = new List<string>();
}

