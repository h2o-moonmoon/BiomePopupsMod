using System.Collections.Generic;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BiomePopupsMod.Util;


/*
TODO:
    Tics to Seconds
    Better Transparency
    Gif Popups
	Mod Biomes:
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
    private BiomePopupUI _popupState;

    public override void OnModLoad()
    {
        Util.Logger.Instance = Mod;
        if (Main.dedServ) return;

        _popupState = new();
    }

    public override void Unload()
    {
        Util.Logger.Instance = null;
        if (Main.dedServ) return;

        _popupState = null;
    }

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