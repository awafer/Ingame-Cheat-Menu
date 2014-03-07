using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.Interface.Controls;

namespace PoroCYon.ICM.Menus.Sub
{
    /// <summary>
    /// The ICM Customize Player menu
    /// </summary>
    public sealed class EditPlayerUI : CheatUI
    {
        /// <summary>
        /// The EditPlayerUI singleton instance
        /// </summary>
        public static EditPlayerUI Interface;

        /// <summary>
        /// Creates a new instance of the EditPlayerUI class
        /// </summary>
        public EditPlayerUI()
            : base(InterfaceType.EditPlayer)
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
