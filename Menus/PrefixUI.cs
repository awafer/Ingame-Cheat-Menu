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
    public sealed class PrefixUI : CheatUI<Prefix>
    {
        /// <summary>
        /// The PrefixUI singleton instance
        /// </summary>
        public static PrefixUI Interface;

        /// <summary>
        /// Creates a new instance of the PrefixUI class
        /// </summary>
        public PrefixUI()
            : base(InterfaceType.Prefix)
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
