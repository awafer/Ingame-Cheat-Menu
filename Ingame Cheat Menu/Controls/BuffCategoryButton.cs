using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions.Graphics;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = PoroCYon.ICM.Menus.BuffUI.Categories;

    /// <summary>
    /// A button used to toggle a Buff category filter on or off
    /// </summary>
    public sealed class BuffCategoryButton : ImageButton
    {
        int id = 0;

        /// <summary>
        /// The category of the BuffCategoryButton
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
        /// Creates a new instance of the BuffCategoryButton class
        /// </summary>
        public BuffCategoryButton()
            : this(Categories.All)
        {

        }

        /// <summary>
        /// Creates a new instance of the BuffCategoryButton class
        /// </summary>
        /// <param name="cat">The category of the BuffCategoryButton</param>
        public BuffCategoryButton(Categories cat)
            : base(MctUI.WhitePixel)
        {
            HasBackground = true;

            switch (Category = cat)
            {
                case Categories.Buff:
                    id = 1; // obsidian skin
                    break;
                case Categories.Debuff:
                    id = 24; // on fire!
                    break;
                case Categories.LightPet:
                    id = 27; // fairy
                    break;
                case Categories.VanityPet:
                    id = 40; // bunny
                    break;
                case Categories.WeaponBuff:
                    id = 74; // weapon imbue: fire
                    break;
            }

            Tooltip = cat.ToString();

            if (id > 0)
            {
                Picture.Item = Main.buffTexture[id];
                Colour = Color.Lerp(new Color(255, 255, 255, 0), new Color(0, 0, 0, 0), (BuffUI.Category & Category) != 0 ? 0f : 0.5f);
            }
            else
                Colour = (BuffUI.Category & Category) == 0 ? new Color(127, 127, 127, 0) : new Color(255, 255, 255, 0);
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((BuffUI.Category & Category) != 0)
                BuffUI.Category ^= Category;
            else
                BuffUI.Category |= Category;

            BuffUI.Interface.Position = 0;

            BuffUI.Interface.ResetObjectList();
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (id > 0)
            {
                Picture.Item = Main.buffTexture[id];
                Colour = Color.Lerp(new Color(255, 255, 255, 0), new Color(0, 0, 0, 0), (BuffUI.Category & Category) != 0 ? 0f : 0.5f);
            }
            else
                Colour = (BuffUI.Category & Category) == 0 ? new Color(127, 127, 127, 0) : new Color(255, 255, 255, 0);
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
