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
    /// The ICM Buff cheat UI
    /// </summary>
    public sealed class BuffUI : CheatUI
    {
        /// <summary>
        /// The BuffUI singleton instance
        /// </summary>
        public static BuffUI Interface;

        /// <summary>
        /// Creates a new instance of the BuffUI class
        /// </summary>
        public BuffUI()
            : base(InterfaceType.Buff)
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