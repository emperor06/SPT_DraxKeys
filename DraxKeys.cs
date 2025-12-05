using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using System.Reflection;
using Path = System.IO.Path;

namespace DraxKeys;

/**
 * Massively using JERO's food mod as template here
 * to ease porting my old mod to SPT 4.
 */
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 10)]
public class DraxKeys : IOnLoad {
    private readonly ISptLogger<DraxKeys> _logger;
    private readonly DatabaseServer _databaseServer;
    private readonly ModConfig _config;
    private readonly HashSet<string> _targetParentIds;
    private readonly HashSet<string> _excludedItemIds;

    public DraxKeys(ISptLogger<DraxKeys> logger, DatabaseServer databaseServer, ModHelper modHelper) {
        _logger = logger;
        _databaseServer = databaseServer;
        _logger.Info("[DRAX] DraxKeys is loading ...");

        string? modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (string.IsNullOrEmpty(modPath)) {
            _logger.Error("[DRAX] DraxKeys, ERROR: Could not determine mod path. The mod will not make any changes.");
            _config = new ModConfig();
            _targetParentIds = [];
            _excludedItemIds = [];
            return;
        }

        _config = modHelper.GetJsonDataFromFile<ModConfig>(modPath, Path.Join("config", "config.json")) ?? new ModConfig();

        if (_config.TargetParentIds.Count == 0)
            _logger.Warning("[DRAX] DraxKeys, WARNING: config.json not found or empty. The mod will not make any changes.");

        _targetParentIds = [.. _config.TargetParentIds];

        var excludeConfig = modHelper.GetJsonDataFromFile<ExcludeConfig>(modPath, Path.Join("config", "exclude.json")) ?? new ExcludeConfig();
        _excludedItemIds = [.. excludeConfig.ExcludeItemIds];

        if (_excludedItemIds.Count > 0)
            _logger.Info($"[DRAX] DraxKeys, {_excludedItemIds.Count} item(s) will be excluded from patching.");
    }

    public Task OnLoad() {
        var itemsDb = _databaseServer.GetTables().Templates.Items;
        int itemsAdjusted = 0;

        foreach (var item in itemsDb.Values.Where(item => _targetParentIds.Contains(item.Parent))) {
            if (_excludedItemIds.Contains(item.Id))
                continue;

            if (TryPatchItem(item))
                itemsAdjusted++;
        }

        _logger.Info($"[DRAX] DraxKeys, {itemsAdjusted} adjustments made!");
        return Task.CompletedTask;
    }

    private bool TryPatchItem(TemplateItem item) {
        if (item.Properties is null || item.Properties.Weight <= 0)
            return false;

        item.Properties.Weight = 0;
        return true;
    }
}
