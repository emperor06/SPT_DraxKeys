using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace DraxKeys;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.draxar.draxkeys";
    public override string Name { get; init; } = "Drax Keys";
    public override string Author { get; init; } = "Drax";
    public override List<string>? Contributors { get; init; } = new() { "Megan Rain" };
    public override SemanticVersioning.Version Version { get; init; } = new("2.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.2");
    public override string? License { get; init; } = "IV";
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = false;
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override List<string>? Incompatibilities { get; init; }
}