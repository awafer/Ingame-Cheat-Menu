using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions.Xna.Geometry;
using PoroCYon.Extensions.Xna.Graphics;
using Terraria;
using TAPI;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = NpcUI.Categories;

    /// <summary>
    /// A Button used to toggle an NPC category filter on or off.
    /// </summary>
    public sealed class NPCCategoryButton : CategoryButton<Categories>
    {
        internal Rectangle oneFrame
        {
            get
            {
                Main.LoadNPC(GetID());
                return new Rectangle(0, 0, GetImage().Width, GetImage().Height / Main.npcFrameCount[GetID()]);
            }
        }
        internal float scale
        {
            get
            {
                return GetID() == 4 ? 0.25f : 1f;
            }
        }

        /// <summary>
        /// Gets the hitbox of the Control.
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                Vector2 pos = Position - (Main.inventoryBackTexture.Size() / 2f - oneFrame.Size() * scale / 2f);

                return new Rectangle((int)pos.X, (int)pos.Y, 52, 52);
            }
        }

        /// <summary>
        /// Creates a new instance of the NPCCategoryButton class.
        /// </summary>
        /// <param name="cat">The category of the NPCCategoryButton.</param>
        public NPCCategoryButton(Categories cat)
            : base(cat)
        {

        }

        int GetID()
        {
            switch (Category)
            {
                case Categories.Boss:
                    return 4; // eye of ctulhu
                case Categories.Friendly:
                    return 46; // bunny
                case Categories.Hostile:
                    return 2; // demon eye
                case Categories.Town:
                    return 22; // guide
            }

            return 0;
        }

        /// <summary>
        /// Clicks the Button.
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((NpcUI.Category & Category) != 0)
                NpcUI.Category ^= Category;
            else
                NpcUI.Category |= Category;

            NpcUI.Instance.Position = 0;

            NpcUI.Instance.ResetObjectList();
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control.</param>
        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);

            //base.Draw(sb);

            sb.Draw(GetImage(), Position + Hitbox.Size() / 2f - (oneFrame.Size() * scale) / 2f,
                oneFrame, Colour, Rotation, Origin, Scale * scale, SpriteEffects, LayerDepth);
        }

        /// <summary>
        /// Gets the image to display.
        /// </summary>
        /// <returns>The image to display. null to display nothing.</returns>
        protected override Texture2D GetImage()
        {
            if (GetID() != 0)
            {
                Main.LoadNPC(GetID());
                return Main.npcTexture[GetID()];
            }

            return null;
        }
        /// <summary>
        /// Gets whether the filter is switched on or off.
        /// </summary>
        /// <returns>Whether the category filter flags has the associated filter flag.</returns>
        protected override bool      GetIsSelected()
        {
            return (NpcUI.Category & Category) != 0;
        }
        /// <summary>
        /// Gets the colour of the image.
        /// </summary>
        /// <returns>The colour of the image.</returns>
        protected override Color     GetImageColour()
        {
            if (GetID() != 0)
                return Defs.npcs[Defs.npcNames[GetID()]].GetAlpha(Color.White);

            return base.GetImageColour();
        }
    }
}
