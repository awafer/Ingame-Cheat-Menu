﻿using System;
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
    /// The ICM NPC cheat menu
    /// </summary>
    public sealed class NPCUI : CheatUI<NPC>
    {
        /// <summary>
        /// The NPCUI singleton instance
        /// </summary>
        public static NPCUI Interface;

        /// <summary>
        /// Creates a new instance of the NPCUI class
        /// </summary>
        public NPCUI()
            : base(InterfaceType.NPC)
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
