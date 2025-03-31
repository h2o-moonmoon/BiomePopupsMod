using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;

namespace BiomePopupsMod.Scanners;

internal class BiomeScanner
{

    List<BiomeScan> Scanners = [];

    public BiomeScanner()
    {
        Init();
    }

    public void Init()
    {
        // Automatically find and add all BiomeScan-derived classes
        AddAllBiomeScans();
        Scanners.Add(new ScanVanilla());
    }

    public void LoadAllTextures()
    {
        foreach (var scanner in Scanners)
        {
            scanner.LoadAllTextures();
        }
    }

    private void AddAllBiomeScans()
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Find all types that inherit from BiomeScan and are not abstract
        var biomeScanTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ModScan)));

        // Instantiate each found type and add to Scanners list
        foreach (var type in biomeScanTypes)
        {
            var instance = Activator.CreateInstance(type) as BiomeScan;
            if (instance != null)
            {
                Scanners.Add(instance);
            }
        }
    }

    public void Clear()
    {
        Scanners.Clear();
    }

    public string DetectBiome(Player player)
    {
        string biome = "";
        if (player.townNPCs > 2)
        {
            biome = ScanBiomes(scanner => scanner.ScanTowns(player));
            if (!string.IsNullOrEmpty(biome))
            {
                return biome;
            }
        }

        biome = ScanBiomes(scanner => scanner.ScanMiniBiomes(player));
        if (!string.IsNullOrEmpty(biome))
        {
            return biome;
        }

        biome = ScanBiomes(scanner => scanner.ScanBiomes(player));
        return biome;
    }

    private string ScanBiomes(Func<BiomeScan, string> scanMethod)
    {
        foreach (BiomeScan scanner in Scanners)
        {
            string biome = scanMethod(scanner);
            if (!string.IsNullOrEmpty(biome))
            {
                return biome;
            }
        }
        return "";
    }


}
