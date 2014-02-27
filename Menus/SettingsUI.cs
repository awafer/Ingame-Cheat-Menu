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
        /// Lime (#00FF00)
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
        public static ThemeColour ThemeColour = Menus.ThemeColour.Black;

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
                        return new Color(32, 160, 224);
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
                return ThemeColour == ThemeColour.Black ? Color.Black : Color.White;
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
    }
}
