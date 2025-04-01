using System;
using Terraria;
using Terraria.ModLoader;


namespace BiomePopupsMod.Util;

internal enum LogType
{
    Log,
    Warning,
    Fail
}

internal class Logger
{
    internal static Mod Instance = null;
    private const bool _isDebug = true;

    internal static void Log(LogType type, string category, object message)
    {
        if (Instance != null)
        {
            string msg = $"[{category}] {message}";
            Chat(msg);
            switch (type)
            {
                case LogType.Log:
                Instance.Logger.Info(msg);
                    break;
                case LogType.Warning:
                Instance.Logger.Warn(msg);
                    break;
                case LogType.Fail:
                Instance.Logger.Error(msg);
                    break;
            }
        }
        else
        {
            Console.WriteLine($"[BTitles] [{type}] [{category}] {message}");
        }
    }
    internal static void Chat(string s)
    {
        if (_isDebug)
        {
            Main.NewText($"[Biome Popup] {s}");
        }
    }
}
