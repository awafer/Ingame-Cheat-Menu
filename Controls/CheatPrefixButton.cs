﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A button which is used to apply a Prefix to an Item
    /// </summary>
    public sealed class CheatPrefixButton : TextButton
    {
        /// <summary>
        /// The Prefix of the CheatPrefixButton
        /// </summary>
        public Prefix Prefix = Prefix.None;

        /// <summary>
        /// Creates a new instance of the CheatPrefixButton class
        /// </summary>
        public CheatPrefixButton()
            : this(Prefix.None)
        {

        }
        /// <summary>
        /// Creates a new instance of the CheatPrefixButton class
        /// </summary>
        /// <param name="i">The Prefix of the CheatPrefixButton</param>
        public CheatPrefixButton(Prefix i)
            : base("")
        {
            Size = new Vector2(80f, 30f);

            Prefix = i;
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            Text = Prefix.DisplayName();

            if (PrefixUI.AvoidWrong.IsChecked && !Prefix.CanApplyToItem(PrefixUI.ItemToSet))
            {
                Tooltip = "This prefix cannot be set to that Item.\n";
                Colour = Color.Red;
            }
            else
            {
                Colour = Color.White;

                Tooltip = "";
                foreach (Tuple<string, bool> t in Prefix.TooltipText(PrefixUI.ItemToSet))
                    Tooltip += t.Item1 + "\n";
            }
            Tooltip += Prefix.type;
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if (PrefixUI.ToSet.Item.Prefix(Prefix.name, PrefixUI.AvoidWrong.IsChecked))
                Main.PlaySound(2, -1, -1, 37);
        }
    }
}
