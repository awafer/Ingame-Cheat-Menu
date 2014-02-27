﻿using System;
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
    /// The ICM World cheat menu
    /// </summary>
    public sealed class WorldUI : CheatUI
    {
        /// <summary>
        /// The WorldUI singleton instance
        /// </summary>
        public static WorldUI Interface;

        /// <summary>
        /// Creates a new instance of the WorldUI class
        /// </summary>
        public WorldUI()
            : base(InterfaceType.World)
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
