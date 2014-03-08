using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM Player cheat menu
    /// </summary>
    public sealed class PlayerUI : CheatUI
    {
        /// <summary>
        /// The PlayerUI singleton instance
        /// </summary>
        public static PlayerUI Interface;

        /// <summary>
        /// Creates a new instance of the PlayerUI class
        /// </summary>
        public PlayerUI()
            : base(InterfaceType.Player)
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
