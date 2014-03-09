using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Graphics;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = PoroCYon.ICM.Menus.ItemUI.Categories;

    /// <summary>
    /// A button used to toggle an Item category filter on or off
    /// </summary>
    public sealed class ItemCategoryButton : ImageButton
    {
        int id = 0;

        /// <summary>
        /// The category of the ItemCategoryButton
        /// </summary>
        public Categories Category;

        /// <summary>
        /// The hitbox of the Control
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                Vector2 pos = Position - (Main.inventoryBackTexture.Size() / 2f - PicAsTexture.Size() / 2f);

                return new Rectangle((int)pos.X, (int)pos.Y, 52, 52);
            }
        }

        /// <summary>
        /// Creates a new instance of the ItemCategoryButton class
        /// </summary>
        public ItemCategoryButton()
            : this(Categories.All)
        {

        }

        /// <summary>
        /// Creates a new instance of the ItemCategoryButton class
        /// </summary>
        /// <param name="cat">The category of the ItemCategoryButton</param>
        public ItemCategoryButton(Categories cat)
            : base(MctUI.WhitePixel)
        {
            HasBackground = true;

            switch (Category = cat)
            {
                case Categories.Accessory:
                    id = 49; // band of regeneration
                    break;
                case Categories.Ammunition:
                    id = 40; // wooden arrow
                    break;
                case Categories.Axe:
                    id = 10; // iron axe
                    break;
                case Categories.Buff:
                    id = 298; // shine potion
                    break;
                case Categories.Potion:
                    id = 28; // leser healing potion
                    break;
                case Categories.Dye:
                    id = 1007; // red dye
                    break;
                case Categories.Hammer:
                    id = 7; // iron hammer
                    break;
                case Categories.Helmet:
                    id = 727; // wood helmet
                    break;
                case Categories.Leggings:
                    id = 729; // wood greaves
                    break;
                case Categories.Magic:
                    id = 165; // water bolt
                    break;
                case Categories.Material:
                    id = 22; // iron bar
                    break;
                case Categories.Melee:
                    id = 4; // iron broadsword
                    break;
                case Categories.Other:
                    Picture = Main.confuseTexture; // question mark
                    break;
                case Categories.Paint:
                    id = 1073; // red paint
                    break;
                case Categories.Pet:
                    id = 603; // carrot
                    break;
                case Categories.Pickaxe:
                    id = 1; // iron pickaxe
                    break;
                case Categories.Ranged:
                    id = 39; // wooden bow
                    break;
                case Categories.Summon:
                    id = 1157; // pygmy staff
                    break;
                case Categories.Tile:
                    id = 2; // dirt block
                    break;
                case Categories.Torso:
                    id = 728; // wood breastplate
                    break;
                case Categories.Vanity:
                    id = 239; // top hat
                    break;
                case Categories.Wall:
                    id = 26; // stone wall
                    break;
            }

            Tooltip = cat.ToString();

            if (id > 0)
            {
                Picture = Defs.items[Defs.itemNames[id]].GetTexture();
                Colour = Color.Lerp(Defs.items[Defs.itemNames[id]].GetTextureColor(), new Color(0, 0, 0, 0), (ItemUI.Category & Category) != 0 ? 0f : 0.5f);
            }
            else
                Colour = (ItemUI.Category & Category) == 0 ? new Color(127, 127, 127, 0) : new Color(255, 255, 255, 0);
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((ItemUI.Category & Category) != 0)
                ItemUI.Category ^= Category;
            else
                ItemUI.Category |= Category;

            ItemUI.Interface.Position = 0;

            ItemUI.Interface.ResetItemList();
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (id > 0)
            {
                Picture = Defs.items[Defs.itemNames[id]].GetTexture();
                Colour = Color.Lerp(Defs.items[Defs.itemNames[id]].GetTextureColor(), new Color(0, 0, 0, 0), (ItemUI.Category & Category) != 0 ? 0f : 0.5f);
            }
            else
                Colour = (ItemUI.Category & Category) == 0 ? new Color(127, 127, 127, 0) : new Color(255, 255, 255, 0);
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);

            base.Draw(sb);
        }
    }
}
