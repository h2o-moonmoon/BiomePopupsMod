using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BiomePopupsMod.Scanners;

internal class ScanVanilla : BiomeScan
    {
        public const string
        // TOWNS
        TOWN =                          "Town_gif",
        TOWN_FOREST =                   "Town_Forest",
        TOWN_OCEAN =                    "Town_Ocean_gif",
        TOWN_MUSHROOM =                 "Town_Mushroom",
        TOWN_JUNGLE =                   "Town_Jungle",
        TOWN_SNOW =                     "Town_Snow_gif",
        TOWN_DESERT =                   "Town_Desert",
        TOWN_HALLOW =                   "Town_Hallow",
        TOWN_CAVERN =                   "Town_Cavern",
        // Towers
        NEBULA =                        "Nebula",
		SOLAR =                         "Solar",
		STARDUST =                      "Stardust",
		VORTEX =                        "Vortex",
        // Mini Biome
        METEOR =                        "Meteor_gif",
		DUNGEON =                       "Dungeon_gif",
		JUNGLE_TEMPLE =                 "Jungle Temple",
        GRAVEYARD =                     "Graveyard",
		GRANITE =                       "Granite Cave",
		MARBLE =                        "Marble Cave_gif",
		SPIDER =                        "Spider Cave",
		BEEHIVE =                       "Beehive_gif",
		AETHER =                        "Aether",
		// Layer
		SPACE =                         "Space_gif",
		FOREST =                        "Forest",
		UNDERGROUND =                   "Underground Layer_gif",
		CAVERN =                        "Cavern Layer",
		UNDERWORLD =                    "Underworld_gif",
		// Biome
		MUSHROOM =                      "Glowing Mushroom",
        MUSHROOM_UNDERGROUND =          "Underground Glowing Mushroom",
        JUNGLE =                        "Jungle",
        JUNGLE_UNDERGROUND =            "Underground Jungle",
		SNOW =                          "Snow",
        SNOW_UNDERGROUND =              "Ice",
		DESERT =                        "Desert",
        DESERT_UNDERGROUND =            "Underground Desert",
		OCEAN =                         "Ocean_gif",
		// Corruption
		CORRUPTION =                    "Corrupted Biome",
        CORRUPTION_UNDERGROUND =        "Underground Corrupted Biome",
		CORRUPTION_SNOW =               "Corrupted Snow",
        CORRUPTION_SNOW_UNDERGROUND =   "Underground Corrupted Snow",
        CORRUPTION_DESERT =             "Corrupted Desert",
        CORRUPTION_DESERT_UNDERGROUND = "Underground Corrupted Desert",
        CORRUPTION_OCEAN =              "Corrupted Ocean_gif",
        // Crimson
        CRIMSON =                       "Crimson Biome",
		CRIMSON_UNDERGROUND =           "Underground Crimson Biome",
		CRIMSON_SNOW =                  "Crimson Snow",
		CRIMSON_SNOW_UNDERGROUND =      "Underground Crimson Snow",
		CRIMSON_DESERT =                "Crimson Desert",
		CRIMSON_DESERT_UNDERGROUND =    "Underground Crimson Desert",
		CRIMSON_OCEAN =                 "Crimson Ocean_gif",
		// Hallow
		HALLOW =                        "Hallowed Biome",
		HALLOW_UNDERGROUND =            "Underground Hallowed Biome",
		HALLOW_SNOW =                   "Hallowed Snow",
		HALLOW_SNOW_UNDERGROUND =       "Underground Hallowed Snow",
		HALLOW_DESERT =                 "Hallowed Desert",
		HALLOW_DESERT_UNDERGROUND =     "Underground Hallowed Desert",
		HALLOW_OCEAN =                  "Hallowed Ocean_gif"
        ;

        public override string ScanTowns(Player player)
        {
            if (player.ZoneForest) return TOWN_FOREST;
            if (player.ZoneBeach) return TOWN_OCEAN;
            if (player.ZoneGlowshroom
            && player.ZoneOverworldHeight) return TOWN_MUSHROOM;
            if (player.ZoneJungle) return TOWN_JUNGLE;
            if (player.ZoneSnow) return TOWN_SNOW;
            if (player.ZoneDesert) return TOWN_DESERT;
            if (player.ZoneHallow) return TOWN_HALLOW;
            if (player.ZoneRockLayerHeight) return TOWN_CAVERN;
            return TOWN;
        }

        public override string ScanMiniBiomes(Player player)
        {
            // Misc
            if (player.ZoneMeteor) return METEOR;
            if (player.ZoneHive) return BEEHIVE;
            if (player.ZoneGraveyard) return GRAVEYARD;
            if (player.ZoneGranite) return GRANITE;
            if (player.ZoneMarble) return MARBLE;

            // Detect Tile Biome
            Point tilePos = player.Center.ToTileCoordinates();
            Tile plrCenterTile = Main.tile[tilePos.X, tilePos.Y];
            if (plrCenterTile.WallType == WallID.SpiderUnsafe) return SPIDER;

            // Events
            if (player.ZoneTowerNebula) return NEBULA;
            if (player.ZoneTowerSolar) return SOLAR;
            if (player.ZoneTowerStardust) return STARDUST;
            if (player.ZoneTowerVortex) return VORTEX;

            // Structure
            if (player.ZoneShimmer) return AETHER;
            if (player.ZoneDungeon) return DUNGEON;
            if (player.ZoneLihzhardTemple) return JUNGLE_TEMPLE;
                        
            if (player.ZoneGlowshroom)
            {
                return player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight ? 
                    MUSHROOM_UNDERGROUND :
                    MUSHROOM;
            }
            return "";
        }

        public override string ScanBiomes(Player player)
        {
            if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight)
            {
                // Underground infection-independent
                if (player.ZoneJungle) return JUNGLE_UNDERGROUND;

                // Underground infectable biomes
                if (player.ZoneDesert)
                {
                    if (player.ZoneCorrupt) return CORRUPTION_DESERT_UNDERGROUND;
                    if (player.ZoneCrimson) return CRIMSON_DESERT_UNDERGROUND;
                    if (player.ZoneHallow) return HALLOW_DESERT_UNDERGROUND;
                    return DESERT_UNDERGROUND;
                }
                else if (player.ZoneSnow)
                {
                    if (player.ZoneCorrupt) return CORRUPTION_SNOW_UNDERGROUND;
                    if (player.ZoneCrimson) return CRIMSON_SNOW_UNDERGROUND;
                    if (player.ZoneHallow) return HALLOW_SNOW_UNDERGROUND;
                    return SNOW_UNDERGROUND;
                }
                else if (player.ZoneRockLayerHeight)
                {
                    if (player.ZoneCorrupt) return CORRUPTION_UNDERGROUND;
                    if (player.ZoneCrimson) return CRIMSON_UNDERGROUND;
                    if (player.ZoneHallow) return HALLOW_UNDERGROUND;
                }
                else
                {
                    if (player.ZoneCorrupt) return CORRUPTION;
                    if (player.ZoneCrimson) return CRIMSON;
                    if (player.ZoneHallow) return HALLOW;
                }
            }

            // Layer-independent
            if (player.ZoneBeach)
            {
                if (player.ZoneCorrupt) return CORRUPTION_OCEAN;
                if (player.ZoneCrimson) return CRIMSON_OCEAN;
                if (player.ZoneHallow) return HALLOW_OCEAN;
                return OCEAN;
            }

            // Layers
            if (player.ZoneSkyHeight) return SPACE;
            if (player.ZoneDirtLayerHeight) return UNDERGROUND;
            if (player.ZoneRockLayerHeight) return CAVERN;
            if (player.ZoneUnderworldHeight) return UNDERWORLD;

            // Non-underground infection-independent
            if (player.ZoneJungle) return JUNGLE;

            // Non-underground infectable biomes
            if (player.ZoneDesert)
            {
                if (player.ZoneCorrupt) return CORRUPTION_DESERT;
                if (player.ZoneCrimson) return CRIMSON_DESERT;
                if (player.ZoneHallow) return HALLOW_DESERT;
                return DESERT;
            }
            else if (player.ZoneSnow)
            {
                if (player.ZoneCorrupt) return CORRUPTION_SNOW;
                if (player.ZoneCrimson) return CRIMSON_SNOW;
                if (player.ZoneHallow) return HALLOW_SNOW;
                return SNOW;
            }
            else
            {
                if (player.ZoneCorrupt) return CORRUPTION;
                if (player.ZoneCrimson) return CRIMSON;
                if (player.ZoneHallow) return HALLOW;
            }

            return FOREST;
        }
    }
