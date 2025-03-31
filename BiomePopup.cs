using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using ReLogic.Content;

namespace BiomePopupsMod;

class BiomePopup : UIElement
{
    public Color backgroundColor = Color.White;
    public float Alpha
    {
        get 
        {
            return backgroundColor.A;
        } set
        {
            backgroundColor.A = Convert.ToByte(value*255);
        } 
    }
    private Texture2D _backgroundTexture;
    public Vector2 textureSize;

    public BiomePopup(string uri)
    {
        if (_backgroundTexture == null)
        {
            try
            {
                if (!ModContent.HasAsset(uri))
                {
                    return;
                }
                _backgroundTexture = ModContent.Request<Texture2D>(uri).Value;
                CalculateSize();
            }
            catch (Exception e)
            {
                Util.Logger.Log(Util.LogType.Warning, "Assets", $"Texture not found {uri}");
            }
        }
    }

    public void CalculateSize()
    {
        // Get the scale from the configuration.
        var config = ModContent.GetInstance<BiomePopupConfig>();
        float scale = config.CustomScale;
        if (!config.ScaleWithUI) scale /= Main.UIScale;

        float textureWidth = _backgroundTexture.Width * scale;
        float textureHeight = _backgroundTexture.Height * scale;

        textureSize = new Vector2(textureWidth, textureHeight);

        Width.Set(textureSize.X, 0);
        Height.Set(textureSize.Y, 0);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        CalculatedStyle dimensions = GetDimensions();
        Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
        int width = (int)Math.Ceiling(dimensions.Width);
        int height = (int)Math.Ceiling(dimensions.Height);
        spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
    }
}