﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = ItemUI.Categories;

    /// <summary>
    /// A Button used to toggle an Item category filter on or off.
    /// </summary>
    public sealed class ItemCategoryButton : CategoryButton<Categories>
    {
        /// <summary>
        /// Creates a new instance of the ItemCategoryButton class.
        /// </summary>
        /// <param name="cat">The category of the ItemCategoryButton.</param>
        public ItemCategoryButton(Categories cat)
            : base(cat)
        {

        }

        /// <summary>
        /// Clicks the Button.
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((ItemUI.Category & Category) != 0)
                ItemUI.Category ^= Category;
            else
                ItemUI.Category |= Category;

            ItemUI.Instance.Position = 0;

            ItemUI.Instance.ResetObjectList();
        }

        /// <summary>
        /// Gets the image to display.
        /// </summary>
        /// <returns>The image to display. null to display nothing.</returns>
        protected override Texture2D GetImage()
        {
            switch (Category)
            {
                case Categories.Accessory:
                    return Main.itemTexture[49  ]; // band of regeneration
                case Categories.Ammunition:
                    return Main.itemTexture[40  ]; // wooden arrow
                case Categories.Axe:
                    return Main.itemTexture[10  ]; // iron axe
                case Categories.Buff:
                    return Main.itemTexture[298 ]; // shine potion
                case Categories.Potion:
                    return Main.itemTexture[28  ]; // leser healing potion
                case Categories.Dye:
                    return Main.itemTexture[1007]; // red dye
                case Categories.Hammer:
                    return Main.itemTexture[7   ]; // iron hammer
                case Categories.Helmet:
                    return Main.itemTexture[727 ]; // wood helmet
                case Categories.Leggings:
                    return Main.itemTexture[729 ]; // wood greaves
                case Categories.Magic:
                    return Main.itemTexture[165 ]; // water bolt
                case Categories.Material:
                    return Main.itemTexture[22  ]; // iron bar
                case Categories.Melee:
                    return Main.itemTexture[4   ]; // iron broadsword
                case Categories.Other:
                    return Main.confuseTexture   ; // question mark
                case Categories.Paint:
                    return Main.itemTexture[1073]; // red paint
                case Categories.Pet:
                    return Main.itemTexture[603 ]; // carrot
                case Categories.Pickaxe:
                    return Main.itemTexture[1   ]; // iron pickaxe
                case Categories.Ranged:
                    return Main.itemTexture[39  ]; // wooden bow
                case Categories.Summon:
                    return Main.itemTexture[1157]; // pygmy staff
                case Categories.Tile:
                    return Main.itemTexture[2   ]; // dirt block
                case Categories.Torso:
                    return Main.itemTexture[728 ]; // wood breastplate
                case Categories.Vanity:
                    return Main.itemTexture[239 ]; // top hat
                case Categories.Wall:
                    return Main.itemTexture[26  ]; // stone wall
            }

            return null;
        }
        /// <summary>
        /// Gets whether the filter is switched on or off.
        /// </summary>
        /// <returns>Whether the category filter flags has the associated filter flag.</returns>
        protected override bool      GetIsSelected()
        {
            return (ItemUI.Category & Category) != 0;
        }
        /// <summary>
        /// Gets the colour of the image.
        /// </summary>
        /// <returns>The colour of the image.</returns>
        protected override Color     GetImageColour()
        {
            switch (Category)
            {
                case Categories.Accessory:
                    return ItemDef.byType[49  ].GetTextureColor(); // band of regeneration
                case Categories.Ammunition:
                    return ItemDef.byType[40  ].GetTextureColor(); // wooden arrow
                case Categories.Axe:
                    return ItemDef.byType[10  ].GetTextureColor(); // iron axe
                case Categories.Buff:
                    return ItemDef.byType[298 ].GetTextureColor(); // shine potion
                case Categories.Potion:
                    return ItemDef.byType[28  ].GetTextureColor(); // leser healing potion
                case Categories.Dye:
                    return ItemDef.byType[1007].GetTextureColor(); // red dye
                case Categories.Hammer:
                    return ItemDef.byType[7   ].GetTextureColor(); // iron hammer
                case Categories.Helmet:
                    return ItemDef.byType[727 ].GetTextureColor(); // wood helmet
                case Categories.Leggings:
                    return ItemDef.byType[729 ].GetTextureColor(); // wood greaves
                case Categories.Magic:
                    return ItemDef.byType[165 ].GetTextureColor(); // water bolt
                case Categories.Material:
                    return ItemDef.byType[22  ].GetTextureColor(); // iron bar
                case Categories.Melee:
                    return ItemDef.byType[4   ].GetTextureColor(); // iron broadsword
                case Categories.Other:
                    return base.GetImageColour(); // question mark
                case Categories.Paint:
                    return ItemDef.byType[1073].GetTextureColor(); // red paint
                case Categories.Pet:
                    return ItemDef.byType[603 ].GetTextureColor(); // carrot
                case Categories.Pickaxe:
                    return ItemDef.byType[1   ].GetTextureColor(); // iron pickaxe
                case Categories.Ranged:
                    return ItemDef.byType[39  ].GetTextureColor(); // wooden bow
                case Categories.Summon:
                    return ItemDef.byType[1157].GetTextureColor(); // pygmy staff
                case Categories.Tile:
                    return ItemDef.byType[2   ].GetTextureColor(); // dirt block
                case Categories.Torso:
                    return ItemDef.byType[728 ].GetTextureColor(); // wood breastplate
                case Categories.Vanity:
                    return ItemDef.byType[239 ].GetTextureColor(); // top hat
                case Categories.Wall:
                    return ItemDef.byType[26  ].GetTextureColor(); // stone wall
            }

            return base.GetImageColour();
        }
    }
}
