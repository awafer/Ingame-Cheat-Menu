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

namespace PoroCYon.ICM.Menus.Sub
{
    /// <summary>
    /// The ICM Edit NPC menu
    /// </summary>
    public sealed class EditNPCUI : CheatUI
    {
        /// <summary>
        /// The EditNPCUI singleton instance
        /// </summary>
        public static EditNPCUI Interface;

        /// <summary>
        /// Creates a new instance of the EditNPCUI class
        /// </summary>
        public EditNPCUI()
            : base(InterfaceType.EditNPC)
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
