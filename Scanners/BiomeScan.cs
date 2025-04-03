using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace BiomePopupsMod.Scanners;

internal abstract class BiomeScan
{
    public abstract string ScanBiomes(Player player);
    public abstract string ScanMiniBiomes(Player player);
    public abstract string ScanTowns(Player player);

    // Method to load textures for all constants
    public void LoadAllTextures()
    {
        // Get all constants from this class and any derived classes
        Type currentType = this.GetType();

        // Traverse the type hierarchy to get all string constants
        while (currentType != null)
        {
            // Get string constants from the current type
            var stringFields = currentType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string));

            // Call GetTexture for each constant
            foreach (var field in stringFields)
            {
                string value = (string)field.GetValue(null); // Get the constant value
                GetTexture(value);  // Call the method to get the texture
                if (value.EndsWith("_gif"))
                {
                    string baseUri = value.Substring(0, value.Length - 4); // Remove "_gif"
                    GetTexture(baseUri);  // Call the method to get the texture
                }
            }

            // Move to the base type to continue the search
            currentType = currentType.BaseType;
        }
    }

    private void GetTexture(String uri)
    {
        string texturePath = "BiomePopupsMod/Assets/Textures/" + uri;
        try
        {
            if (ModContent.HasAsset(texturePath))
            {
                Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            }
            else
            {
                Util.Logger.Log(Util.LogType.Warning, "Assets", $"Texture not found {texturePath}");
            }
        }
        catch (Exception e)
        {
            Util.Logger.Log(Util.LogType.Warning, "Assets", $"Texture not found {texturePath}");
        }
    }
}