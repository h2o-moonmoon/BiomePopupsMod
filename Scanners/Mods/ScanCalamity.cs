using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BiomePopupsMod.Scanners;

internal class ScanCalamity : ModScan
{

    public const string
    TOWN_ASTRAL =               "Town_Astral",
    TOWN_CRAGS =                "Town_Crags",
    TOWN_SULPHUR =              "Town_Sulphur",
    TOWN_SUNKEN =               "Town_Sunken",
    SUNKEN_SEA =                "Calamity Sunken Sea",
    SULPHUR_SEA =                   "Calamity Sulphurous Sea",
    CRAG =                      "Calamity Brimstone Crag",
    ABYSS_1 =                   "Calamity Abyss 1",
    ABYSS_2 =                   "Calamity Abyss 2",
    ABYSS_3 =                   "Calamity Abyss 3",
    ABYSS_4 =                   "Calamity Abyss 4",
    ASTRAL =                    "Calamity Astral Biome",
    ASTRAL_UNDERGROUND =        "Calamity Underground Astral Biome",
    ASTRAL_OCEAN =              "Calamity Astral Ocean",
    ASTRAL_DESERT =             "Calamity Astral Desert",
    ASTRAL_UNDERGROUND_DESERT = "Calamity Underground Astral Desert",
    ASTRAL_SNOW =               "Calamity Astral Snow",
    ASTRAL_UNDERGROUND_SNOW =   "Calamity Underground Astral Snow"
    ;

    public ScanCalamity() => _modName = "CalamityMod";

    public override string ScanTowns(Player player)
    {
        if (ModLoader.TryGetMod(_modName, out Mod mod))
        {
            if (CheckModBiome(player, mod, "AstralInfectionBiome")) return TOWN_ASTRAL;
            if (CheckModBiome(player, mod, "BrimstoneCragsBiome"))  return TOWN_CRAGS;
            if (CheckModBiome(player, mod, "SulphurousSeaBiome"))   return TOWN_SULPHUR;
            if (CheckModBiome(player, mod, "SunkenSeaBiome"))       return TOWN_SUNKEN;
        }
        return "";
    }

    public override string ScanMiniBiomes(Player player)
    {
        return "";
    }

    public override string ScanBiomes(Player player)
    {

        if (ModLoader.TryGetMod("CalamityMod", out Mod mod))
        {
            if (CheckModBiome(player, mod, "SunkenSeaBiome"))       return SUNKEN_SEA;
            if (CheckModBiome(player, mod, "SulphurousSeaBiome"))   return SULPHUR_SEA;
            if (CheckModBiome(player, mod, "BrimstoneCragsBiome"))  return CRAG;
            if (CheckModBiome(player, mod, "AbyssLayer1Biome"))     return ABYSS_1;
            if (CheckModBiome(player, mod, "AbyssLayer2Biome"))     return ABYSS_2;
            if (CheckModBiome(player, mod, "AbyssLayer3Biome"))     return ABYSS_3;
            if (CheckModBiome(player, mod, "AbyssLayer4Biome"))     return ABYSS_4;


            if (CheckModBiome(player, mod, "AstralInfectionBiome"))
            {
                if (player.ZoneBeach) return ASTRAL_OCEAN;
                bool isUnderground = player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight;
                if (player.ZoneDesert) return isUnderground ?
                        ASTRAL_UNDERGROUND_DESERT :
                        ASTRAL_DESERT;
                if (player.ZoneSnow) return isUnderground ?
                        ASTRAL_UNDERGROUND_SNOW :
                        ASTRAL_SNOW;
                return isUnderground ?
                        ASTRAL_UNDERGROUND :
                        ASTRAL;
            }
        }
        return "";
    }
}