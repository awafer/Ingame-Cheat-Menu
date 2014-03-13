using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.Content;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.MCT.UI.Interface.Controls.Primitives;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A Buff container which is used to apply or de-apply buffs to the Player
    /// </summary>
    public sealed class CheatBuffContainer : Button
    {
        Texture2D bgTex = Main.inventoryBackTexture;
        int invBackNum = 1;

        /// <summary>
        /// The Buff the CheatBuffContainer contains
        /// </summary>
        public Buff Buff;

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
        /// Creates a new instance of the CheatBuffContainer class
        /// </summary>
        public CheatBuffContainer()
            : this(new Buff())
        {

        }
        /// <summary>
        /// Creates a new instance of the CheatBuffContainer class
        /// </summary>
        /// <param name="i">The Buff of the CheatBuffContainer</param>
        public CheatBuffContainer(Buff i)
        {
            Buff = i;

            Tooltip = "";

            if (Buff.ID > 0)
                Tooltip = Buff.DisplayName + "\n" + Buff.Tooltip;
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if (Buff.ID <= 0)
                return;

            int bid = -1;
            if ((bid = Main.localPlayer.AnyBuff(Buff.ID)) > -1) // has buff
                Main.localPlayer.DelBuff(bid);
            else
                Main.localPlayer.AddBuff(Buff.ID, BuffUI.ToTicks(BuffUI.BuffTime));
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            InventoryBackTextureNum =
                Main.localPlayer.AnyBuff(Buff.ID) > -1 && Buff.ID > 0
                    ? (Buff.Type & BuffType.Debuff) != 0
                        ? 2
                        : 3
                    : 1;

            Tooltip = Buff.ID > 0 ? Buff.DisplayName + "\n" + Buff.Tooltip : "";
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.Draw(bgTex, Position, null, MainUI.WithAlpha(Color.White, 150), Rotation, Origin, Scale, SpriteEffects, LayerDepth);

            if (Buff.ID > 0)
                sb.Draw(Buff.Texture, Position + (bgTex.Size() / 2f - Buff.Texture.Size() / 2f), null, Color.White,
                    Rotation, Origin, Scale, SpriteEffects, LayerDepth);
        }
    }
}
