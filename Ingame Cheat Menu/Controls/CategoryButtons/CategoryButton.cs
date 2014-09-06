using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface.Controls;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A Button used to toggle a category filter of a CheatUI on or off.
    /// </summary>
    /// <typeparam name="TEnum">The filter flags.</typeparam>
    public abstract class CategoryButton<TEnum> : ImageButton
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// The category of the CategoryButton.
        /// </summary>
        public TEnum Category;

        /// <summary>
        /// Gets the hitbox of the Control.
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
        /// Creates a new instance of the CategoryButton class.
        /// </summary>
        /// <param name="cat">The category filter associated with the Button.</param>
        protected CategoryButton(TEnum cat)
            : base(MctUI.WhitePixel)
        {
            Category = cat;

            HasBackground = true;

            Tooltip = cat.ToString();

            Texture2D tex = GetImage();

            if (tex != null)
            {
                Picture.Item = tex;
                Colour = Color.Lerp(new Color(255, 255, 255, 0), new Color(0, 0, 0, 0), GetIsSelected() ? 0f : 0.5f);
            }
            else
                Colour = GetIsSelected() ? new Color(255, 255, 255, 0) : new Color(127, 127, 127, 0);
        }

        /// <summary>
        /// Updates the Control.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (GetImage() != null)
                Colour = Color.Lerp(GetImageColour(), new Color(0, 0, 0, 0), GetIsSelected() ? 0f : 0.5f);
            else
                Colour = GetIsSelected() ? new Color(255, 255, 255, 0) : new Color(127, 127, 127, 0);
        }
        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);

            base.Draw(sb);
        }

        /// <summary>
        /// Gets the image to display.
        /// </summary>
        /// <returns>The image to display. null to display nothing.</returns>
        protected abstract Texture2D GetImage();
        /// <summary>
        /// Gets whether the filter is switched on or off.
        /// </summary>
        /// <returns>Whether the category filter flags has the associated filter flag.</returns>
        protected abstract bool      GetIsSelected();
        /// <summary>
        /// Gets the colour of the image.
        /// </summary>
        /// <returns>The colour of the image.</returns>
        protected virtual  Color     GetImageColour()
        {
            return new Color(255, 255, 255, 0);
        }
    }
}
