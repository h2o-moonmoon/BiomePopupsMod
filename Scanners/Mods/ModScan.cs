using Terraria;
using Terraria.ModLoader;

namespace BiomePopupsMod.Scanners;

internal abstract class ModScan : BiomeScan
{
    protected string _modName;
    protected bool CheckModBiome(Player player, Mod mod, string biomeName)
    {
        if (mod.TryFind(biomeName, out ModBiome biome))
        {
            return player.InModBiome(biome);
        }
        return false;
    }
    protected bool CheckModBiomes(Player player, Mod mod, string[] biomeList)
    {
        foreach (string biomeName in biomeList)
        {
            if (mod.TryFind(biomeName, out ModBiome biome))
            {
                if (player.InModBiome(biome)) return true;
            }
        }
        return false;
    }

    protected bool TryCheckInBiome(string modName, string biomeName)
    {
        Player player = Main.LocalPlayer;
        if (ModLoader.TryGetMod(modName, out Mod mod))
        {
            if (mod.TryFind(biomeName, out ModBiome biome))
            {
                return player.InModBiome(biome);
            }

        }
        return false;
    }
}