using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using TAPI.SDK;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.Interface.Controls;
using TAPI.SDK.UI.Interface.Controls.Primitives;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A 'readonly' ItemContainer... 
    /// </summary>
    public sealed class CheatItemContainer : Button
    {
        Texture2D bgTex = Main.inventoryBackTexture;
        int invBackNum = 1;

        /// <summary>
        /// The Item the CheatItem contains
        /// </summary>
        public Item Item;

        /// <summary>
        /// Gets or sets [n] where [n] is Main.inventoryBack[n]Texture
        /// </summary>
        public int InventoryBackTextureNum
        {
            get
            {
                return invBackNum;
            }
            set
            {
                if (value < 1 || value > 12)
                    throw new ArgumentOutOfRangeException("value");

                switch (invBackNum = value)
                {
                    case 1:
                        bgTex = Main.inventoryBackTexture;
                        break;
                    case 2:
                        bgTex = Main.inventoryBack2Texture;
                        break;
                    case 3:
                        bgTex = Main.inventoryBack3Texture;
                        break;
                    case 4:
                        bgTex = Main.inventoryBack4Texture;
                        break;
                    case 5:
                        bgTex = Main.inventoryBack5Texture;
                        break;
                    case 6:
                        bgTex = Main.inventoryBack6Texture;
                        break;
                    case 7:
                        bgTex = Main.inventoryBack7Texture;
                        break;
                    case 8:
                        bgTex = Main.inventoryBack8Texture;
                        break;
                    case 9:
                        bgTex = Main.inventoryBack9Texture;
                        break;
                    case 10:
                        bgTex = Main.inventoryBack10Texture;
                        break;
                    case 11:
                        bgTex = Main.inventoryBack11Texture;
                        break;
                    case 12:
                        bgTex = Main.inventoryBack12Texture;
                        break;
                }
            }
        }

        /// <summary>
        /// The hitbox of the Control
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(bgTex.Width * Scale.X), (int)(bgTex.Height * Scale.Y));
            }
        }

        /// <summary>
        /// Creates a new instance of the CheatItemContainer class
        /// </summary>
        public CheatItemContainer()
            : this(new Item())
        {

        }
        /// <summary>
        /// Creates a new instance of the CheatItemContainer class
        /// </summary>
        /// <param name="i">The Item of the CheatItemContainer</param>
        public CheatItemContainer(Item i)
        {
            Item = i;
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if (Main.mouseItem.type == 0 || Main.mouseItem.stack == 0)
            {
                ItemUI.CopyItem(Item, ref Main.mouseItem);
                Main.mouseItem.stack = Main.mouseItem.maxStack;

                Main.PlaySound(7);
            }
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.Draw(bgTex, Position, null, MainUI.WithAlpha(Color.White, 150), Rotation, Origin, Scale, SpriteEffects, LayerDepth);

            if (!Item.IsBlank())
                sb.Draw(Item.GetTexture(), Position + (bgTex.Size() / 2f - Main.itemTexture[Item.type].Size() / 2f), null, Item.GetTextureColor(),
                    Rotation, Origin, Scale, SpriteEffects, LayerDepth);

            if (IsHovered)
                ItemUI.TooltipToDisplay = Item;
        }
    }
}
