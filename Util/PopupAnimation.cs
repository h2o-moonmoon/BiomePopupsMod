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
    protected const float MARGIN = 20f;

    protected abstract void AnimateIn(float progress, AnimationConfig config, BiomePopup popup);
    protected abstract void AnimateOut(float progress, AnimationConfig config, BiomePopup popup);
    protected abstract void Show(float progress, AnimationConfig config, BiomePopup popup);


    public virtual void Animate(float time, AnimationConfig config, BiomePopup popup)
    {
        bool infiniteStay = config.VisibleDuration < 0;
        float inTime = config.InDuration;
        float outTime = config.OutDuration;
        float stationaryTime = config.VisibleDuration;

        if (time == 1) SetupPopup(config, popup);

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

    protected void SetupPopup(AnimationConfig config, BiomePopup popup)
    {
        switch (config.PositionOption)
        {
            case PositionOption.Top:
                popup.MarginTop = MARGIN;
                break;
            case PositionOption.Bottom:
                popup.MarginBottom = MARGIN;
                break;
            case PositionOption.BottomLeft:
                popup.MarginBottom = MARGIN;
                popup.MarginLeft = MARGIN;
                break;
            case PositionOption.BottomRight:
                popup.MarginBottom = MARGIN;
                popup.MarginRight = MARGIN;
                break;
        }
    }

}

internal class AnimationNone : PopupAnimation
{
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
    private float _getTarget(AnimationConfig config)
    {
        bool isMid = config.PositionOption == (PositionOption.Top | PositionOption.Bottom);
        return isMid ? 0 : MARGIN;
    }
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(-popup.textureSize.X, _getTarget(config), progress);
        if (config.PositionOption == PositionOption.BottomRight)
            popup.MarginRight = margin;
        else popup.MarginLeft = margin;
        float alpha = MathHelper.SmoothStep(0, 1, progress);
        popup.Alpha = alpha;
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(_getTarget(config), popup.textureSize.X, progress);
        if (config.PositionOption == PositionOption.BottomRight)
            popup.MarginRight = margin;
        else popup.MarginLeft = margin;
        float alpha = MathHelper.SmoothStep(1, 0, progress);
        popup.Alpha = alpha;
    }

    protected override void Show(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = _getTarget(config);
        if (config.PositionOption == PositionOption.BottomRight)
            popup.MarginRight = margin;
        else popup.MarginLeft = margin;
        popup.Alpha = 1.0f;
    }
}

internal class AnimationSlide : PopupAnimation
{
    private float _getDistance(AnimationConfig config, BiomePopup popup)
    {
        float distance = 
            config.PositionOption == PositionOption.BottomRight ||
            config.PositionOption == PositionOption.BottomLeft ?
            popup.textureSize.X : 
            popup.textureSize.Y;
        return -distance;
    }
    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(_getDistance(config, popup), MARGIN, progress);
        _setMargin(margin, config, popup);
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(MARGIN, _getDistance(config, popup), progress);
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
    private float _getDistance(AnimationConfig config, BiomePopup popup)
    {
        float distance =
            config.PositionOption == PositionOption.BottomRight ||
            config.PositionOption == PositionOption.BottomLeft ?
            popup.textureSize.X :
            popup.textureSize.Y;
        return -distance;
    }

    protected override void AnimateIn(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(_getDistance(config, popup), 20f, progress);
        _setMargin(margin, config, popup);
        float alpha = MathHelper.SmoothStep(0, 1, progress);
        popup.Alpha = alpha;
    }

    protected override void AnimateOut(float progress, AnimationConfig config, BiomePopup popup)
    {
        float margin = MathHelper.SmoothStep(20f, _getDistance(config, popup), progress);
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