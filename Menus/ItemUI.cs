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
    /// The ICM Item cheat UI
    /// </summary>
    public sealed class ItemUI : CheatUI
    {
        /// <summary>
        /// The ItemUI singleton instance
        /// </summary>
        public static ItemUI Interface;

        /// <summary>
        /// Creates a new instance of the ItemUI class
        /// </summary>
        public ItemUI()
            : base(InterfaceType.Item)
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
