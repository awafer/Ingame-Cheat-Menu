using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;

namespace TAPI.PoroCYon.ICM.Menus.Sub
{
    /// <summary>
    /// The ICM Edit global NPC vars menu
    /// </summary>
    public sealed class EditGlobalNPCUI : CheatUI
    {
        /// <summary>
        /// The EditGlobalNPCUI singleton instance
        /// </summary>
        public static EditGlobalNPCUI Interface;

        /// <summary>
        /// Creates a new instance of the EditGlobalNPCUI class
        /// </summary>
        public EditGlobalNPCUI()
            : base(InterfaceType.EditGlobalNPC)
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
