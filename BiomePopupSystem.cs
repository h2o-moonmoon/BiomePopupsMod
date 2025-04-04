using System.Collections.Generic;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

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