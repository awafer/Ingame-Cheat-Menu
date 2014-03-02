using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;

namespace TAPI.PoroCYon.ICM.Menus
{
    /// <summary>
    /// All possible accent colours
    /// </summary>
    public enum AccentColour : byte
    {
        /// <summary>
        /// Lime (0, 255, 0)
        /// </summary>
        Lime = 0,
        /// <summary>
        /// OrangeRed (255, 69, 0)
        /// </summary>
        OrangeRed = 1,
        /// <summary>
        /// Cobalt (32, 160, 224)
        /// </summary>
        Cobalt = 2
    }
    /// <summary>
    /// All possible theme colours
    /// </summary>
    public enum ThemeColour : byte
    {
        /// <summary>
        /// Black (#000000)
        /// </summary>
        Black = 0,
        /// <summary>
        /// White (#FFFFFF)
        /// </summary>
        White = 1
    }

    /// <summary>
    /// The ICM settings UI
    /// </summary>
    public sealed class SettingsUI : CheatUI
    {
        readonly static Color
            Black = new Color(50, 50, 50),
            Cobalt = new Color(32, 160, 224);

        /// <summary>
        /// The SettingsUI singleton instance
        /// </summary>
        public static SettingsUI Interface;

        /// <summary>
        /// The current accent colour
        /// </summary>
        public static AccentColour AccentColour = AccentColour.Lime;
        /// <summary>
        /// The current theme colour
        /// </summary>
        public static ThemeColour ThemeColour = ThemeColour.Black;

        /// <summary>
        /// The current accent colour as a Color
        /// </summary>
        public static Color ColourAccent
        {
            get
            {
                switch (AccentColour)
                {
                    case AccentColour.Cobalt:
                        return Cobalt;
                    case AccentColour.Lime:
                        return Color .Lime;
                    case AccentColour.OrangeRed:
                        return Color .OrangeRed;
                }

                throw new ArgumentOutOfRangeException("AccentColour");
            }
        }
        /// <summary>
        /// The current theme colour as a Color
        /// </summary>
        public static Color ColourTheme
        {
            get
            {
                return ThemeColour == ThemeColour.Black ? Black : Color.White;
            }
        }

        /// <summary>
        /// Creates a new instance of the SettingsUI class
        /// </summary>
        public SettingsUI()
            : base(InterfaceType.Settings)
        {

        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public override void Open()
        {
            
        }
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();

            Controls.Add(new TextBlock("Theme colour:")
            {
                Position = new Vector2(300f, Main.screenHeight - 300f),
                Colour = ColourAccent,
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, ColourAccent)
            });
            Controls.Add(new ImageButton(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                HasBackground = false,
                Colour = Black,
                Tooltip = "Black",
                Click = (b) => ThemeColour = ThemeColour.Black,
                Position = new Vector2(300f, Main.screenHeight - 270f),
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, Black)
            });
            Controls.Add(new ImageButton(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                HasBackground = false,
                Colour = Color.White,
                Tooltip = "White",
                Click = (b) => ThemeColour = ThemeColour.White,
                Position = new Vector2(326f, Main.screenHeight - 270f),
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, Color.White)
            });

            Controls.Add(new TextBlock("Accent colour:")
            {
                Position = new Vector2(300f, Main.screenHeight - 220f),
                Colour = ColourAccent,
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, ColourAccent)
            });
            Controls.Add(new ImageButton(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                HasBackground = false,
                Colour = Cobalt,
                Tooltip = "Cobalt",
                Click = (b) => AccentColour = AccentColour.Cobalt,
                Position = new Vector2(300f, Main.screenHeight - 190f),
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, Cobalt)
            });
            Controls.Add(new ImageButton(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                HasBackground = false,
                Colour = Color.Lime,
                Tooltip = "Lime",
                Click = (b) => AccentColour = AccentColour.Lime,
                Position = new Vector2(326f, Main.screenHeight - 190f),
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, Color.Lime)
            });
            Controls.Add(new ImageButton(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                HasBackground = false,
                Tooltip = "OrangeRed",
                Colour = Color.OrangeRed,
                Click = (b) => AccentColour = AccentColour.OrangeRed,
                Position = new Vector2(352f, Main.screenHeight - 190f),
                OnUpdate = (c) => c.Colour = MainUI.GrayColour(c.Hitbox, Color.OrangeRed)
            });
        }
    }
}
