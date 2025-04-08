using Terraria;

namespace BiomePopupsMod.Scanners;

internal class ScanThorium : ModScan
{
    public const string DEPTHS = "Thorium Aquatic Depths";
    public ScanThorium() => _modName = "ThoriumMod";

    public override string ScanTowns(Player player)
    {
        return "";
    }

    public override string ScanMiniBiomes(Player player)
    {
        return "";
    }

    public override string ScanBiomes(Player player)
    {
        if (TryCheckInBiome(_modName, "DepthsBiome")) return DEPTHS;
        return "";
    }
}
