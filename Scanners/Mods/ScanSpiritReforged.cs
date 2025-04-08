using Terraria;
using Terraria.ModLoader;

namespace BiomePopupsMod.Scanners;

internal class ScanSpiritReforged : ModScan
{
    public const string
        TOWN_SAVANNA =       "SpiritReforged Town Savanna",

        SAVANNA =            "SpiritReforged Savanna",
        SAVANNA_CORRUPTION = "SpiritReforged Corrupted Savanna",
        SAVANNA_CRIMSON =    "SpiritReforged Crimson Savanna",
        SAVANNA_HALLOW =     "SpiritReforged Hallowed Savanna"
        ;
    public ScanSpiritReforged() => _modName = "SpiritReforged";

    public override string ScanTowns(Player player)
    {
        if (TryCheckInBiome(_modName, "SavannaBiome")) return TOWN_SAVANNA;
        if (TryCheckInBiome(_modName, "HallowSavanna")) return TOWN_SAVANNA;
        return "";
    }

    public override string ScanMiniBiomes(Player player)
    {
        if (ModLoader.TryGetMod(_modName, out Mod mod))
        {
            if (CheckModBiome(player, mod, "HallowSavanna")) return SAVANNA_HALLOW;
            if (CheckModBiome(player, mod, "SavannaBiome"))
            {
                if (player.ZoneCorrupt) return SAVANNA_CORRUPTION;
                if (player.ZoneCrimson) return SAVANNA_CRIMSON;
                if (player.ZoneHallow) return SAVANNA_HALLOW;
                return SAVANNA;
            }
        }
        return "";
    }

    public override string ScanBiomes(Player player)
    {
        return "";
    }
}
