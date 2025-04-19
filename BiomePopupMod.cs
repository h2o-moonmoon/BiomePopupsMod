using BiomePopupsMod.Util;
using System;
using Terraria.ModLoader;

namespace BiomePopupsMod;

public class BiomePopupMod : Mod
{
    public override object Call(params object[] args)
    {
        try
        {
            if (args == null || args.Length == 0 || args[0] is not string message)
            {
                Util.Logger.Log(LogType.Warning, "ModCall", "Mod.Call received with invalid arguments.");
                return null;
            }

            switch (message)
            {
                case "GetCurrentBiomeName":
                    if (BiomePopupSystem._popupState == null)
                    {
                        Util.Logger.Log(LogType.Log, "ModCall", 
                            "Mod.Call 'GetCurrentBiomeName' received, but UI is not loaded. Returning null.");
                        return null;
                    }
                    return BiomePopupSystem._popupState._currentBiome;

                default:
                    Util.Logger.Log(LogType.Warning, "ModCall",
                        $"Mod.Call received with unknown message: {message}");
                    return null;
            }
        }
        catch (Exception e)
        {
            Util.Logger.Log(LogType.Fail, "ModCall", $"Error handling Mod.Call: {e.ToString}");
            return null;
        }
    }

}
