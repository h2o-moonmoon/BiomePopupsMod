using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using BiomePopupsMod.Util;

namespace BiomePopupsMod;

class BiomePopup : UIElement
{
    public Color backgroundColor = Color.White;
    public float Alpha
    {
        get => backgroundColor.A;
        set => backgroundColor = Color.White * value;
    }
    public Vector2 textureSize;

    private Texture2D _spriteSheet;
    private List<Rectangle> _frames = new();
    private int _currentFrame = 0;
    private double _frameTimer = 0;
    private double _frameDelay = 62; // Adjust for animation speed
    private bool _isAnimated = false;

    public BiomePopup(string uri)
    {
        LoadTexture(uri);
    }

    private void LoadTexture(string uri)
    {
        try
        {
            if (uri.EndsWith("_gif"))
            {
                string baseUri = uri.Substring(0, uri.Length - 4); // Remove "_gif"

                if (!ModContent.HasAsset(baseUri) || !ModContent.HasAsset(uri))
                {
                    Util.Logger.Log(Util.LogType.Warning, "Assets", $"Missing assets: {uri} or {baseUri}");
                    return;
                }

                // Load both textures
                _spriteSheet = ModContent.Request<Texture2D>(uri).Value;
                Texture2D referenceTexture = ModContent.Request<Texture2D>(baseUri).Value;

                // Extract frames based on reference texture height
                LoadVerticalSpriteSheetFrames(referenceTexture.Height);
                _isAnimated = true;
            }
            else
            {
                // Load a normal static image
                if (!ModContent.HasAsset(uri)) return;
                _spriteSheet = ModContent.Request<Texture2D>(uri).Value;
            }

            CalculateSize();
        }
        catch (Exception e)
        {
            Util.Logger.Log(Util.LogType.Warning, "Assets", $"Failed to load texture {uri}: {e.Message}");
        }
    }

    private void LoadVerticalSpriteSheetFrames(int frameHeight)
    {
        int frameWidth = _spriteSheet.Width; // Entire width is a single frame
        int totalFrames = _spriteSheet.Height / frameHeight;

        for (int i = 0; i < totalFrames; i++)
        {
            _frames.Add(new Rectangle(0, i * frameHeight, frameWidth, frameHeight));
        }
    }

    public void CalculateSize()
    {
        var config = ModContent.GetInstance<BiomePopupConfig>();
        float scale = config.CustomScale;
        if (!config.IsScaleWithUI) scale /= Main.UIScale;

        float textureWidth = _frames.Count > 0 ? _frames[0].Width : _spriteSheet.Width;
        float textureHeight = _frames.Count > 0 ? _frames[0].Height : _spriteSheet.Height;

        textureSize = new Vector2(textureWidth * scale, textureHeight * scale);
        Width.Set(textureSize.X, 0);
        Height.Set(textureSize.Y, 0);
    }

    public override void Update(GameTime gameTime)
    {
        if (_isAnimated && _frames.Count > 1)
        {
            _frameTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_frameTimer >= _frameDelay)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Count;
                _frameTimer = 0;
            }
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        Rectangle sourceRect = _isAnimated ? _frames[_currentFrame] : new Rectangle(0, 0, _spriteSheet.Width, _spriteSheet.Height);

        spriteBatch.Draw(_spriteSheet,
            new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height),
            sourceRect, backgroundColor);
    }
}
