﻿
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using Terraria;
using tModPorter;

namespace BiomePopupsMod.Util;
public class AnimationConfig
{
    public BiomePopupConfig Config
    {
        set
        {
            PositionOption = value.Position;
            VisibleDuration = value.VisibleDuration;
            InDuration = value.AnimateInDuration;
            OutDuration = value.AnimationOutTime;
        }
    }

    public PositionOption PositionOption;

    // Total animation duration in seconds
    public float VisibleDuration;

    // In transition duration in seconds
    public float InDuration;

    // Out transition duration in seconds
    public float OutDuration;
}

internal abstract class PopupAnimation
{

    protected abstract void AnimateIn(float progress, AnimationConfig config, BiomePopup popup);
    protected abstract void AnimateOut(float progress, AnimationConfig config, BiomePopup popup);
    protected abstract void Show(float progress, AnimationConfig config, BiomePopup popup);


    public virtual void Animate(float time, AnimationConfig config, BiomePopup popup)
    {
        bool infiniteStay = config.VisibleDuration < 0;
        float inTime = config.InDuration;
        float outTime = config.OutDuration;
        float stationaryTime = config.VisibleDuration;

        if (time < inTime)
        {
            float progress = time / inTime;
            AnimateIn(progress, config, popup);
        }
        else if (time < stationaryTime + inTime || infiniteStay)
        {
            float progress = (time - inTime) / stationaryTime;
            Show(progress, config, popup);
        }
        else
        {
            float progress = (time - stationaryTime - inTime) / outTime;
            AnimateOut(progress, config, popup);
        }

        popup.Recalculate();
    }

}

internal class AnimationNone : PopupAnimation
{
    public override void Animate(float time, AnimationConfig config, BiomePopup popup) { }
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup) { }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup) { }

    protected override void Show(float progress, AnimationConfig config, BiomePopup popup) { }
}

internal class AnimationFade : PopupAnimation
{
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float alpha = MathHelper.SmoothStep(0, 1, progress);
        popup.Alpha = alpha;
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float alpha = MathHelper.SmoothStep(1, 0, progress);
        popup.Alpha = alpha;
    }

    protected override void Show(float progress, AnimationConfig config, BiomePopup popup)
    {
        popup.Alpha = 1.0f;
    }
}

internal class AnimationFadeSwipe : PopupAnimation
{
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(-50f, 0, progress);
        popup.MarginLeft = margin;
        float alpha = MathHelper.SmoothStep(0, 1, progress);
        popup.Alpha = alpha;
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(0, 50f, progress);
        popup.MarginLeft = margin;
        float alpha = MathHelper.SmoothStep(1, 0, progress);
        popup.Alpha = alpha;
    }

    protected override void Show(float progress, AnimationConfig config, BiomePopup popup)
    {
        popup.MarginLeft = 0;
        popup.Alpha = 1.0f;
    }
}

internal class AnimationSlide : PopupAnimation
{
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(-popup.textureSize.Y, 20f, progress);
        _setMargin(margin, config, popup);
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(20f, -popup.textureSize.Y, progress);
        _setMargin(margin, config, popup);
    }

    protected override void Show(float progress, AnimationConfig config, BiomePopup popup)
    {
        _setMargin(20f, config, popup);
    }
    
    private void _setMargin(float margin, AnimationConfig config, BiomePopup popup)
    {
        switch (config.PositionOption)
        {
            case PositionOption.Top:
                popup.MarginTop = margin;
                break;
            case PositionOption.Bottom:
                popup.MarginBottom = margin;
                break;
            case PositionOption.BottomLeft:
                popup.MarginLeft = margin;
                break;
            case PositionOption.BottomRight:
                popup.MarginRight = margin;
                break;
        }
    }
}

internal class AnimationFadeSlide : PopupAnimation
{
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(-popup.textureSize.Y, 20f, progress);
        _setMargin(margin, config, popup);
        float alpha = MathHelper.SmoothStep(0, 1, progress);
        popup.Alpha = alpha;
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(20f, -popup.textureSize.Y, progress);
        _setMargin(margin, config, popup);
        float alpha = MathHelper.SmoothStep(1, 0, progress);
        popup.Alpha = alpha;
    }

    protected override void Show(float progress, AnimationConfig config,BiomePopup popup)
    {
        _setMargin(20f, config, popup);
        popup.Alpha = 1.0f;
    }

    private void _setMargin(float margin, AnimationConfig config, BiomePopup popup)
    {
        switch (config.PositionOption)
        {
            case PositionOption.Top:
                popup.MarginTop = margin;
                break;
            case PositionOption.Bottom:
                popup.MarginBottom = margin;
                break;
            case PositionOption.BottomLeft:
                popup.MarginLeft = margin;
                break;
            case PositionOption.BottomRight:
                popup.MarginRight = margin;
                break;
        }
    }
}