//using Microsoft.Xna.Framework;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ModLoader;
//using Terraria.UI;

//namespace BiomePopupsMod;

//internal class BiomePopupMod: Mod
//{

//    private BiomePopupUI _popupState;

//    public override void Load()
//    {
//        Util.Logger.Instance = this;
//        if (Main.dedServ) return;

//        _popupState = new();

//        On_Main.DrawInterface_30_Hotbar += Draw;
//        On_Main.Update += Update;
//    }

//    public override void Unload()
//    {
//        Util.Logger.Instance = null;
//        if (Main.dedServ) return;

//        _popupState = null;

//        On_Main.DrawInterface_30_Hotbar -= Draw;
//        On_Main.Update -= Update;
//    }

//    private void Update(On_Main.orig_Update orig, Main self, GameTime gameTime)
//    {
//        if (Main.dedServ) return;

//        _popupState.PostUpdatePlayers();
//    }

//    private void Draw(On_Main.orig_DrawInterface_30_Hotbar orig, Main self)
//    {
//        if (_popupState.IsVisible)
//        {
//            _popupState.Draw(Main.spriteBatch);
//        }
//    }



//}
