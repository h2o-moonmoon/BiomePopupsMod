//using Terraria;
//using Terraria.ModLoader;

//namespace BiomePopupsMod.Scanners;

//internal class ScanRemnants : ModScan
//{
//    public const string
//        MARBLE =            "Marble Cave_gif",
//        GRANITE =           "Granite Cave",
//        JUNGLE_TEMPLE =     "Jungle Temple",

//        OCEAN =             "Remnants_Ocean Cave",
//        UNDERGROWTH =       "Remnants_Undergrowth",
//        AERIAL_GARDEN =     "Remnants_Aerial Garden",
//        FORGOTTEN_TOMB =    "Remnants_Forgotten Tomb",
//        MAGICAL_LAB =       "Remnants_Magical Lab",
//        ECHOING_HALLS =     "Remnants_Echoing Halls",
//        PYRAMID =           "Remnants_Pyramid",
//        HIVE =              "Remnants_Hive"
//        ;
//    public ScanRemnants() => _modName = "Remnants";

//    public override string ScanTowns(Player player)
//    {
//        return "";
//    }

//    public override string ScanMiniBiomes(Player player)
//    {
//        if (ModLoader.TryGetMod(_modName, out Mod mod))
//        {
//            if (CheckModBiome(player, mod, "MarbleCave")) return MARBLE;
//            if (CheckModBiome(player, mod, "GraniteCave")) return GRANITE;
//            if (CheckModBiome(player, mod, "OceanCave")) return OCEAN;
//            if (CheckModBiome(player, mod, "Undergrowth")) return UNDERGROWTH;
//            if (CheckModBiome(player, mod, "AerialGarden")) return AERIAL_GARDEN;
//            if (CheckModBiome(player, mod, "ForgottenTomb")) return FORGOTTEN_TOMB;
//            if (CheckModBiome(player, mod, "MagicalLab")) return MAGICAL_LAB;
//            if (CheckModBiome(player, mod, "EchoingHalls")) return ECHOING_HALLS;
//            if (CheckModBiome(player, mod, "Pyramid")) return PYRAMID;
//            if (CheckModBiome(player, mod, "Hive")) return HIVE;
//            if (CheckModBiome(player, mod, "JungleTemple")) return JUNGLE_TEMPLE;
//        }
//        return "";
//    }

//    public override string ScanBiomes(Player player)
//    {
//        return "";
//    }
//}
