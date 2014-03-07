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
    /// The ICM Edit Item menu
    /// </summary>
    public sealed class EditItemUI : CheatUI
    {
        /// <summary>
        /// The EditItemUI singleton instance
        /// </summary>
        public static EditItemUI Interface;

        /// <summary>
        /// Creates a new instance of the EditItemUI class
        /// </summary>
        public EditItemUI()
            : base(InterfaceType.EditItem)
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
