using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TAPI;

namespace PoroCYon.ICM
{
    /// <summary>
    /// The class used to modify the vanilla UI
    /// </summary>
    public sealed class ModUI : ModInterface
    {
        /// <summary>
        /// Creates a new instance of the ModUI class
        /// </summary>
        /// <param name="base">The ModBase of the ModInterface</param>
        public ModUI(ModBase @base)
            : base(@base)
        {

        }

        /// <summary>
        /// Called before the cafting list is shown, also used to determinate if the list should be drawn or not.
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the crafting list</param>
        /// <returns>true if the list should be drawn, false otherwise.</returns>
        public override bool PreDrawCrafting(SpriteBatch sb)
        {
            return MainUI.UIType == InterfaceType.None && base.PreDrawCrafting(sb);
        }
    }
}
