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

namespace PoroCYon.ICM
{
    /// <summary>
    /// The base class of all cheat menus, provides automated hiding
    /// </summary>
    public abstract class CheatUI : CustomUI
    {
        /// <summary>
        /// The type of the cheat UI
        /// </summary>
        public InterfaceType Type = InterfaceType.None;

        /// <summary>
        /// Creates a new instace of the CheatUI class
        /// </summary>
        /// <param name="type">The type of the cheat menu</param>
        public CheatUI(InterfaceType type)
            : base()
        {
            Type = type;
        }

        /// <summary>
        /// Gets wether the interface is visible or not
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return base.IsVisible && MainUI.UIType == Type;
            }
        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public abstract void Close();
    }
}
