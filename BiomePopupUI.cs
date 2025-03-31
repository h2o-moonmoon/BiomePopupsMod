﻿using System;

using BiomePopupsMod.Scanners;
using BiomePopupsMod.Util;

using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using System.Numerics;
using System.Linq;
using tModPorter;

namespace BiomePopupsMod;

internal class BiomePopupUI : UIState
{
    private bool _isInfiniteStay = false;
    private bool _isAnimating = true;
    private string _currentBiome = "";
    private int _displayTime = 0;
    private int _maxDisplayTime = 210;
    private int _currentFrame = 0; // For handling GIF animation
    private BiomePopup _popup;
    BiomePopupConfig _config;


    private BiomeScanner _biomeScanner = new();

    public InterfaceScaleType ScaleType
    {
        get
        {
            return _config.ScaleWithUI ?
                InterfaceScaleType.UI :
                InterfaceScaleType.None;
        }
    }
    private PositionOption _positionOption
    {
        get
        {
            return _config.Position;
        }
    }
    private AnimationConfig _animationConfig 
    {
        get
        {
            return new()
            {
                Config = _config
            };
        }
    }
    private Action<float, AnimationConfig, BiomePopup> _animateFunc
    {
        get
        {
            switch (_config.AnimationType)
            {
                case AnimationType.Fade:
                    return new AnimationFade().Animate;
                case AnimationType.FadeSwipe:
                    return new AnimationFadeSwipe().Animate;
                case AnimationType.Slide:
                    return new AnimationSlide().Animate;
                case AnimationType.FadeSlide:
                    return new AnimationSlide().Animate;
                case AnimationType.None:
                default:
                    return new AnimationNone().Animate;
            }
        }
    }
    public bool IsVisible
    {
        get
        {
            if (_config != null)
            {
                bool bossIsAlive = Main.npc.Any(npc => npc.active && npc.boss);
                if (_config.isHideWhileBossAlive && bossIsAlive) return false;
                if (_config.isHideWhileInventoryOpen && Main.playerInventory) return false;
            }
            return (_isInfiniteStay || _displayTime > 0) && _popup != null;
        }
    }

    public BiomePopupUI()
    {
        // setup Config
        _config = ModContent.GetInstance<BiomePopupConfig>();
        _config.OnPropertyChanged += _configPropertyChanged;
        Main.OnResolutionChanged += _setSize;
    }

    private void _setSize(Microsoft.Xna.Framework.Vector2 vector)
    {
        _popup.CalculateSize();
    }

    private void _setPosition()
    {
        if (_popup == null) return;

        _popup.MarginLeft = 0;
        _popup.MarginTop = 0;
        _popup.MarginRight = 0;
        _popup.MarginBottom = 0;
        _popup.Alpha = 1;
        _popup.CalculateSize();

        switch (_positionOption)
        {
            case PositionOption.Top:
                _popup.HAlign = 0.5f;
                _popup.VAlign = 0f;
                break;
            case PositionOption.Bottom:
                _popup.HAlign = 0.5f;
                _popup.VAlign = 1f;
                break;
            case PositionOption.BottomLeft:
                _popup.HAlign = 0f;
                _popup.VAlign = 1f;
                _popup.MarginBottom = 20f;
                break;
            case PositionOption.BottomRight:
                _popup.HAlign = 1f;
                _popup.VAlign = 1f;
                _popup.MarginBottom = 20f;
                break;
            case PositionOption.Custom:
                _popup.HAlign = 0.5f;
                _popup.VAlign = 1f;
                break;
        }
    }

    public void PostUpdatePlayers()
    {
        if (Main.dedServ) return;

        // Scan for new biome
        string newBiome = _checkBiomes();
        if (newBiome != _currentBiome)
        {
            Logger.Chat($"New Biome {newBiome}");
            bool success = _createPopup(newBiome);

            Logger.Chat($"Popup success: {success}");
        }

        // check if popup exists
        if (_popup == null)
        {
            return;
        }

        // return if timer expired
        if (_displayTime <= 0 && !_isInfiniteStay)
        {
            if (!_isAnimating)
            {
                Logger.Chat("Animation Done");
                _isAnimating = true;
            }
            RemoveChild(_popup); // Remove popup after time expires
            return;
        }

        _displayTime--;
        if (_isAnimating)
        {
            Logger.Chat("Animation Start");
            _isAnimating = false;
        }

        // Animate popup
        float time = _maxDisplayTime - _displayTime;
        _animateFunc(time, _animationConfig, _popup);
    }

    private bool _createPopup(string newBiome)
    {
        string uri = "BiomePopupsMod/Assets/Textures/" + newBiome;
        if (!ModContent.HasAsset(uri))
        {
            Logger.Log(LogType.Warning, "Assets", $"Texture not found {uri}");
            return false;
        }

        _currentBiome = newBiome;;

        if (_popup != null && HasChild(_popup))
        {
            RemoveChild(_popup); // Remove old popup
        }

        _popup = new BiomePopup(uri);

        _respawnPopup();
        return true;
    }

    private int _biomeTicks = 0;
    private string _checkBiomes()
    {
        int modulus = (int)_config.biomeCheckDelay;
        _biomeTicks++;
        _biomeTicks %= modulus;
        if (_biomeTicks == 0)
            return _biomeScanner.DetectBiome(Main.LocalPlayer);
        return _currentBiome;
    }

    private void _respawnPopup()
    {
        if (_popup == null) return;

        _setPosition();
        _isInfiniteStay = _config.VisibleDuration < 0;
        float totalTime = _config.VisibleDuration + _config.AnimateInDuration + _config.AnimationOutTime;
        _maxDisplayTime = (int)totalTime;
        _displayTime = _maxDisplayTime;
        if (!HasChild(_popup))
        {
            Append(_popup);
        }
        Recalculate();
    }

    private void _configPropertyChanged(string name)
    {
        switch (name)
        {
            case nameof(BiomePopupConfig.isHideWhileInventoryOpen):
            case nameof(BiomePopupConfig.isHideWhileBossAlive):
            case nameof(BiomePopupConfig.biomeCheckDelay):
                _animationConfig.Config = _config;
                break;
            case nameof(BiomePopupConfig.Position):
            case nameof(BiomePopupConfig.ScaleWithUI):
            case nameof(BiomePopupConfig.CustomScale):
            case nameof(BiomePopupConfig.AnimationType):
            case nameof(BiomePopupConfig.VisibleDuration):
            case nameof(BiomePopupConfig.AnimateInDuration):
            case nameof(BiomePopupConfig.AnimationOutTime):
                _animationConfig.Config = _config;
                _respawnPopup();
                break;
        }
    }

}