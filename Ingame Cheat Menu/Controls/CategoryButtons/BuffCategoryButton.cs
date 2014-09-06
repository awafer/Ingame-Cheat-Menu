using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = BuffUI.Categories;

    /// <summary>
    /// A Button used to toggle a Buff category filter on or off.
    /// </summary>
    public sealed class BuffCategoryButton : CategoryButton<Categories>
    {
        /// <summary>
        /// Creates a new instance of the BuffCategoryButton class
        /// </summary>
        /// <param name="cat">The category of the BuffCategoryButton</param>
        public BuffCategoryButton(Categories cat)
            : base(cat)
        {

        }

        /// <summary>
        /// Clicks the Button.
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((BuffUI.Category & Category) != 0)
                BuffUI.Category ^= Category;
            else
                BuffUI.Category |= Category;

            BuffUI.Instance.Position = 0;

            BuffUI.Instance.ResetObjectList();
        }

        /// <summary>
        /// Gets the image to display.
        /// </summary>
        /// <returns>The image to display. null to display nothing.</returns>
        protected override Texture2D GetImage()
        {
            switch (Category)
            {
                case Categories.Buff:
                    return Main.buffTexture[1 ]; // obsidian skin
                case Categories.Debuff:
                    return Main.buffTexture[24]; // on fire!
                case Categories.LightPet:
                    return Main.buffTexture[27]; // fairy
                case Categories.VanityPet:
                    return Main.buffTexture[40]; // bunny
                case Categories.WeaponBuff:
                    return Main.buffTexture[74]; // weapon imbue: fire
            }

            return null;
        }
        /// <summary>
        /// Gets whether the filter is switched on or off.
        /// </summary>
        /// <returns>Whether the category filter flags has the associated filter flag.</returns>
        protected override bool      GetIsSelected()
        {
            return (BuffUI.Category & Category) != 0;
        }
    }
}
