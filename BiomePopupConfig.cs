using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Terraria.ModLoader.Config;

namespace BiomePopupsMod
{
    public enum PositionOption
    {
        [Label("Top")]
        Top,

        [Label("Bottom")]
        Bottom,

        [Label("Bottom Left")]
        BottomLeft,

        [Label("Bottom Right")]
        BottomRight,

        //[Label("Custom")]
        //Custom
    }

    public enum ScaleOption
    {
        [Label("Small")]
        Small,

        [Label("Normal")]
        Normal,

        [Label("Big")]
        Big
    }

    public enum AnimationType
    {
        [Label("No animation")]
        None,
        
        [Label("Fade")]
        Fade,
        
        [Label("Fade with swipe")]
        FadeSwipe,
        
        [Label("Slide")]
        Slide,

        [Label("Fade with Slide")]
        FadeSlide
    }

    public class BiomePopupConfig : ModConfig
    {
        public delegate void PropertyChangedDelegate(string name);

        public event PropertyChangedDelegate OnPropertyChanged;
        // This sets the configuration to be client-side, as each user can have their own settings.
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Visuals")]
        
        [Label("Position")]
        [Tooltip("Choose the position of the Title")]
        [DefaultValue(PositionOption.Top)]
        [DrawTicks]
        public PositionOption Position;

        // Add a setting to enable/disable scaling with UI.
        [Label("Scale with UI")]
        [Tooltip("If enabled, the popup will scale along with the UI scale")]
        [DefaultValue(true)]
        public bool IsScaleWithUI;

        // Add a setting for custom scaling percentage.
        [Label("Custom Popup Scale")]
        [Tooltip("Choose the popup size scale, between 50% and 200%")]
        [Range(0.5f, 2f)] // Limits the range from 50% to 200%.
        [Increment(0.1f)]
        [DefaultValue(1f)] // Default is 100% (1.0).
        public float CustomScale;

        [Header("Animation")]
        
        [Label("Animation Type")]
        [Tooltip("Title appear and disappear animation")]
        [DefaultValue(AnimationType.Slide)]
        [DrawTicks]
        public AnimationType AnimationType;

        [Label("Display Duration")]
        [Tooltip("Duration (in tics) for the popup to remain stationary between animations (-1 means infinite)")]
        [Range(-1f, 300f)]
        [Increment(1f)]
        [DefaultValue(180f)] // Default value in frames
        public float VisibleDuration;
        
        [Label("Animation-In Duration")]
        [Tooltip("Duration (in tics) for the popup to animate in")]
        [Range(30f, 120f)]
        [Increment(1f)]
        [DefaultValue(30f)] // Default value in frames
        public float AnimateInDuration;

        [Label("Animation-Out Duration")]
        [Tooltip("Duration (in tics) for the popup to animate out (disappear)")]
        [Range(30f, 120f)]
        [Increment(1f)]
        [DefaultValue(30f)] // Default value in frames
        public float AnimationOutTime;

        [Header("Advanced")]

        [Label("Hide While Inventory Open")]
        [Tooltip("Should popup be hidden when the player inventory is open")]
        [DefaultValue(false)]
        public bool IsHideWhileInventoryOpen;

        [Label("Hide While Boss Is Alive")]
        [Tooltip("Should popup be hidden while a boss is alive")]
        [DefaultValue(true)]
        public bool IsHideWhileBossAlive;

        [Label("Biome Check Interval")]
        [Tooltip("Interval (in ticks) between biome checks")]
        [Range(1f, 300f)]
        [Increment(1f)]
        [DefaultValue(30f)]
        public float BiomeCheckDelay;

        [JsonIgnore]
        private Dictionary<string, int> _cachedDataHashes = new Dictionary<string, int>();
        
        public override void OnChanged()
        {
            GetType()
                .GetFields()
                .Where(field => field.GetCustomAttribute(typeof(LabelAttribute)) != null)
                .Select(field => new { Name = field.Name, Value = field.GetValue(this) })
                .Where(field => !_cachedDataHashes.TryGetValue(field.Name, out int hash) || hash != field.Value.GetHashCode())
                .ToList()
                .ForEach(field =>
                {
                    _cachedDataHashes[field.Name] = field.Value.GetHashCode();
                    if (OnPropertyChanged != null) 
                        OnPropertyChanged(field.Name);
                });
        }
    }
}