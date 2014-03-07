using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Graphics;
using TAPI;
using TAPI.SDK.UI;
using TAPI.SDK.UI.MenuItems;

namespace PoroCYon.ICM
{
    /// <summary>
    /// All possible accent (foreground) colours
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
    /// All possible theme (background) colours
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
    public sealed class SettingsPage : Page
    {
        readonly static Color
            Black = new Color(20, 20, 20),
            Cobalt = new Color(32, 160, 224);

        /// <summary>
        /// The SettingsUI singleton instance
        /// </summary>
        public static SettingsPage PageInstance;

        /// <summary>
        /// The current accent (foreground) colour
        /// </summary>
        public static AccentColour AccentColour = AccentColour.Lime;
        /// <summary>
        /// The current theme (background) colour
        /// </summary>
        public static ThemeColour ThemeColour = ThemeColour.Black;

        /// <summary>
        /// The current accent (foreground) colour as a Color
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
        /// The current theme (background) colour as a Color
        /// </summary>
        public static Color ColourTheme
        {
            get
            {
                return ThemeColour == ThemeColour.Black ? Black : Color.White;
            }
        }

        /// <summary>
        /// Gets a colour from the specified AccentColour
        /// </summary>
        /// <param name="accent">The AccentColour to 'convert' to a Color</param>
        /// <returns>The AccentColour as a Color</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public static Color FromAccentEnum(AccentColour accent)
        {
            switch (accent)
            {
                case AccentColour.Cobalt:
                    return Cobalt;
                case AccentColour.Lime:
                    return Color.Lime;
                case AccentColour.OrangeRed:
                    return Color.OrangeRed;
            }

            throw new ArgumentOutOfRangeException("accent");
        }
        /// <summary>
        /// Gets a colour from the specified ThemeColour
        /// </summary>
        /// <param name="theme">The ThemeColour to 'convert' to a Color</param>
        /// <returns>The ThemeColour as a Color</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public static Color FromThemeEnum(ThemeColour theme)
        {
            return theme == ThemeColour.Black ? Black : Color.White;
        }

        /// <summary>
        /// Creates a new instance of the SettingsUI class
        /// </summary>
        public SettingsPage()
            : base()
        {

        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        protected override void Init()
        {
            base.Init();

            MenuAnchor
                aLeft = new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(-105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aRight = new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aCentre = new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(0f, 200f),
                    offset_button = new Vector2(0f, 50f)
                };

            anchors.AddRange(new List<MenuAnchor>() { aLeft, aRight, aCentre });

            buttons.Add(new MenuButton(0, "Accent colour:", "", "", () => { }).With(mb =>
            {
                mb.canMouseOver = false;

                mb.SetAutomaticPosition(aLeft, 0);

                mb.Update += () => mb.colorText = ColourAccent;
            }));

            buttons.Add(new Image(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                IsButton = true,
                size = new Vector2(40f) / 24f,
                colorText = FromAccentEnum(AccentColour.Cobalt),
                description = "Cobalt"
            }
            .With(mb =>
            {
                mb.SetAutomaticPosition(aLeft, 1);

                mb.Click += () => AccentColour = AccentColour.Cobalt;
            }));
            buttons.Add(new Image(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                IsButton = true,
                size = new Vector2(40f) / 24f,
                colorText = FromAccentEnum(AccentColour.Lime),
                description = "Lime"
            }
            .With(mb =>
            {
                mb.SetAutomaticPosition(aLeft, 2);

                mb.Click += () => AccentColour = AccentColour.Lime;
            }));
            buttons.Add(new Image(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                IsButton = true,
                size = new Vector2(40f) / 24f,
                colorText = FromAccentEnum(AccentColour.OrangeRed),
                description = "Orange-red"
            }
            .With(mb =>
            {
                mb.SetAutomaticPosition(aLeft, 3);

                mb.Click += () => AccentColour = AccentColour.OrangeRed;
            }));

            // ---

            buttons.Add(new MenuButton(0, "Theme colour:", "", "", () => { }).With(mb =>
            {
                mb.canMouseOver = false;

                mb.SetAutomaticPosition(aRight, 0);

                mb.Update += () => mb.colorText = ColourAccent;
            }));

            buttons.Add(new Image(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                IsButton = true,
                size = new Vector2(40f) / 24f,
                colorText = FromThemeEnum(ThemeColour.Black),
                description = "Black"
            }
            .With((mb) =>
            {
                mb.SetAutomaticPosition(aRight, 1);

                mb.Click += () => ThemeColour = ThemeColour.Black;
            }));
            buttons.Add(new Image(SdkUI.WhitePixel)
            {
                Scale = new Vector2(24f),
                IsButton = true,
                size = new Vector2(40f) / 24f,
                colorText = FromThemeEnum(ThemeColour.White),
                description = "White"
            }
            .With((mb) =>
            {
                mb.SetAutomaticPosition(aRight, 2);

                mb.Click += () => ThemeColour = ThemeColour.White;
            }));

            // ---

            buttons.Add(new MenuButton(0, "Back", "Options").With(mb => mb.SetAutomaticPosition(aCentre, 4)));
        }
    }
}
