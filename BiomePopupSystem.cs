using System.Collections.Generic;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

using BiomePopupsMod.Scanners;


/*
TODO:
    Gif Popups
	Mod Biomes
        Verdant
		Remnants
		The Stars Above
		Mod of Redemption
		Spirit Mod
		Arbour
		Spooky
		The Depths
		Confection Rebaked
		Starlight River
		Bob Blender
		Aequus
		Friends, Furniture and Fun
		Calamity Infernum
		Rothur Mod
		Everjade
		Awakened Light
		Spirit Reforged
		Lunar Veil
*/
namespace BiomePopupsMod;

public class BiomePopupSystem : ModSystem
{
    private string currentBiome = "";
    private int displayTime = 0;
    private int MaxDisplayTime = 210;
    private UIState uiState;
    int currentFrame = 0; // For handling GIF animation
    private BiomePopup popup;
    private float currentY;
    private BiomeScanner _biomeScanner = new();
    private BiomePopupUI _popupState;

    public override void OnModLoad()
    {
        Util.Logger.Instance = Mod;
        if (Main.dedServ) return;

        uiState = new();
        _popupState = new();
        _biomeScanner.LoadAllTextures();
    }

    public override void Unload()
    {
        Util.Logger.Instance = null;
        if (Main.dedServ) return;

        uiState = null;
        _popupState = null;
        _biomeScanner.Clear();
    }

    bool IsAnimating = true;

    public override void PostUpdatePlayers()
    {
        if (Main.dedServ) return;

        _popupState.PostUpdatePlayers();
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (_popupState.IsVisible)
        {
            layers.Add(new LegacyGameInterfaceLayer(
                "YourModName: BiomePopup",
                delegate
                {
                    //uiState.Draw(Main.spriteBatch);
                    _popupState.Draw(Main.spriteBatch);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}