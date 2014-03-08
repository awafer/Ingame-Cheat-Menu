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
    /// <summary>
    /// A button used to toggle an Item category filter on or off
    /// </summary>
    public sealed class ItemCategoryButton : ImageButton
    {
        int id = 0;

        /// <summary>
        /// The category of the ItemCategoryButton
        /// </summary>
        public Category Category;

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
            : this(Category.All)
        {

        }

        /// <summary>
        /// Creates a new instance of the ItemCategoryButton class
        /// </summary>
        /// <param name="cat">The category of the ItemCategoryButton</param>
        public ItemCategoryButton(Category cat)
            : base(MctUI.WhitePixel)
        {
            HasBackground = true;

            switch (Category = cat)
            {
                case Category.Accessory:
                    id = 49; // band of regeneration
                    break;
                case Category.Ammunition:
                    id = 40; // wooden arrow
                    break;
                case Category.Axe:
                    id = 10; // iron axe
                    break;
                case Category.Buff:
                    id = 298; // shine potion
                    break;
                case Category.Potion:
                    id = 28; // leser healing potion
                    break;
                case Category.Dye:
                    id = 1007; // red dye
                    break;
                case Category.Hammer:
                    id = 7; // iron hammer
                    break;
                case Category.Helmet:
                    id = 727; // wood helmet
                    break;
                case Category.Leggings:
                    id = 729; // wood greaves
                    break;
                case Category.Magic:
                    id = 165; // water bolt
                    break;
                case Category.Material:
                    id = 22; // iron bar
                    break;
                case Category.Melee:
                    id = 4; // iron broadsword
                    break;
                case Category.Other:
                    Picture = Main.confuseTexture; // question mark
                    break;
                case Category.Paint:
                    id = 1073; // red paint
                    break;
                case Category.Pet:
                    id = 603; // carrot
                    break;
                case Category.Pickaxe:
                    id = 1; // iron pickaxe
                    break;
                case Category.Ranged:
                    id = 39; // wooden bow
                    break;
                case Category.Summon:
                    id = 1157; // pygmy staff
                    break;
                case Category.Tile:
                    id = 2; // dirt block
                    break;
                case Category.Torso:
                    id = 728; // wood breastplate
                    break;
                case Category.Vanity:
                    id = 239; // top hat
                    break;
                case Category.Wall:
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

            ItemUI.Position = 0;

            ItemUI.ResetItemList();
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
