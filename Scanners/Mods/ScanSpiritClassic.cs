using Terraria;
using Terraria.ModLoader;

namespace BiomePopupsMod.Scanners;

internal class ScanSpiritClassic : ModScan
{
    public const string
        TOWN_ASTEROID =             "SpiritMod Town Asteroid_gif",
        TOWN_HYPERSPACE =           "SpiritMod Town Hyperspace",
        TOWN_BRIAR=                 "SpiritMod Town Briar",
        TOWN_SPIRIT =               "SpiritMod Town Spirit_gif",

        ASTEROID =                  "SpiritMod Asteroid Field",
        HYPERSPACE =                "SpiritMod Hyperspace",

        BRIAR_FOREST =              "SpiritMod Briar Forest",
        BRIAR_CAVES =               "SpiritMod Briar Caves",

        SPIRIT =                    "SpiritMod Spirit Surface",
        SPIRIT_UNDERGROUND =        "SpiritMod Spirit Underground",
        SPIRIT_OCEAN =              "SpiritMod Spirit Ocean_gif",
        SPIRIT_DESERT =             "SpiritMod Spirit Desert",
        SPIRIT_UNDERGROUND_DESERT = "SpiritMod Spirit Underground", // SpiritMod Underground Spirit Desert
        SPIRIT_SNOW =               "SpiritMod Spirit Snow",
        SPIRIT_UNDERGROUND_SNOW =   "SpiritMod Underground Spirit Snow"
        ;
    public ScanSpiritClassic() => _modName = "SpiritMod";

    public override string ScanTowns(Player player)
    {
        if (ModLoader.TryGetMod(_modName, out Mod mod))
        {
            if (CheckModBiome(player, mod, "AsteroidBiome")) return TOWN_ASTEROID;
            if (CheckModBiome(player, mod, "SynthwaveSurfaceBiome")) return TOWN_HYPERSPACE;
            if (CheckModBiome(player, mod, "BriarSurfaceBiome")) return TOWN_BRIAR;
            if (CheckModBiome(player, mod, "BriarUndergroundBiome")) return TOWN_BRIAR;
            if (CheckModBiome(player, mod, "SpiritSurfaceBiome")) return TOWN_SPIRIT;
            if (CheckModBiome(player, mod, "SpiritUndergroundBiome")) return TOWN_SPIRIT;
        }
        return "";
    }

    public override string ScanMiniBiomes(Player player)
    {
        if (ModLoader.TryGetMod(_modName, out Mod mod))
        {
            if (CheckModBiome(player, mod, "AsteroidBiome")) return ASTEROID;
            if (CheckModBiome(player, mod, "SynthwaveSurfaceBiome")) return HYPERSPACE;
        }
        return "";
    }

    public override string ScanBiomes(Player player)
    {
        if (ModLoader.TryGetMod(_modName, out Mod mod))
        {
            if (CheckModBiome(player, mod, "BriarSurfaceBiome")) return BRIAR_FOREST;
            if (CheckModBiome(player, mod, "BriarUndergroundBiome")) return BRIAR_CAVES;

            if (CheckModBiome(player, mod, "SpiritSurfaceBiome"))
            {
                if (player.ZoneBeach) return SPIRIT_OCEAN;
                bool isUnderground = player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight;
                if (player.ZoneDesert) return isUnderground ?
                        SPIRIT_UNDERGROUND_DESERT :
                        SPIRIT_DESERT;
                if (player.ZoneSnow) return isUnderground ?
                        SPIRIT_UNDERGROUND_SNOW :
                        SPIRIT_SNOW;
                return isUnderground ?
                        SPIRIT_UNDERGROUND :
                        SPIRIT;
            }
            if (CheckModBiome(player, mod, "SpiritUndergroundBiome"))
            {
                if (player.ZoneBeach) return SPIRIT_OCEAN;
                if (player.ZoneDesert) return SPIRIT_UNDERGROUND_DESERT;
                if (player.ZoneSnow) return SPIRIT_UNDERGROUND_SNOW;
                return SPIRIT_UNDERGROUND;
            }
        }
        return "";
    }
}
