using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions.Geometry;
using PoroCYon.XnaExtensions.Graphics;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI.Interface.Controls.Primitives;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    using Categories = PoroCYon.ICM.Menus.NPCUI.Categories;

    /// <summary>
    /// A button used to toggle an NPC category filter on or off
    /// </summary>
    public sealed class NPCCategoryButton : Button
    {
        int id = 0;

        Rectangle oneFrame
        {
            get
            {
                Main.LoadNPC(id);
                return new Rectangle(0, 0, Main.npcTexture[id].Width, Main.npcTexture[id].Height / Main.npcFrameCount[id]);
            }
        }

        /// <summary>
        /// The category of the NPCCategoryButton
        /// </summary>
        public Categories Category;

        /// <summary>
        /// The hitbox of the Control
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                Vector2 pos = Position - Main.inventoryBackTexture.Size() / 4f;

                return new Rectangle((int)Position.X, (int)Position.Y, 52, 52);
            }
        }

        /// <summary>
        /// Creates a new instance of the ItemCategoryButton class
        /// </summary>
        public NPCCategoryButton()
            : this(Categories.All)
        {

        }

        /// <summary>
        /// Creates a new instance of the NPCCategoryButton class
        /// </summary>
        /// <param name="cat">The category of the NPCCategoryButton</param>
        public NPCCategoryButton(Categories cat)
            : base()
        {
            HasBackground = true;

            switch (Category = cat)
            {
                case Categories.Boss:
                    id = 4; // eye of ctulhu
                    break;
                case Categories.Friendly:
                    id = 46; // bunny
                    break;
                case Categories.Hostile:
                    id = 2; // demon eye
                    break;
                case Categories.Town:
                    id = 22; // guide
                    break;
            }

            Tooltip = cat.ToString();
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if ((NPCUI.Category & Category) != 0)
                NPCUI.Category ^= Category;
            else
                NPCUI.Category |= Category;

            NPCUI.Interface.Position = 0;

            NPCUI.Interface.ResetObjectList();
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (id > 0)
                Colour = Color.Lerp(Defs.npcs[Defs.npcNames[id]].GetAlpha(Color.White),
                    new Color(0, 0, 0, 0), (NPCUI.Category & Category) != 0 ? 0f : 0.5f);
            else
                Colour = (NPCUI.Category & Category) == 0 ? new Color(127, 127, 127, 0) : new Color(255, 255, 255, 0);
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);

            base.Draw(sb);

            Main.LoadNPC(id);
            sb.Draw(Main.npcTexture[id], Position + Hitbox.Size() / 2f - (oneFrame.Size() * (id == 4 ? 0.25f : 1f)) / 2f,
                oneFrame, Colour, Rotation, Origin, Scale * (id == 4 ? 0.25f : 1f), SpriteEffects, LayerDepth);
        }
    }
}
