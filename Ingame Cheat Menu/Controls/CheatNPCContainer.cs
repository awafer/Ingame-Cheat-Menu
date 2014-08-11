using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions;
using PoroCYon.Extensions.Xna.Geometry;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI.Interface.Controls.Primitives;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A control which spawns NPCs
    /// </summary>
    public sealed class CheatNPCContainer : Button
    {
        static WrapperDictionary<string, bool> changesFrame = new WrapperDictionary<string, bool>();

        /// <summary>
        /// The amount of NPCs to spawn
        /// </summary>
        public static int Amount = 0;
        /// <summary>
        /// The netID of the selected NPC to spawn
        /// </summary>
        public static int CurrentNetID = 0;

        Texture2D bgTex = Main.inventoryBackTexture;
        int invBackNum = 1;

        /// <summary>
        /// The NPC the CheatNPCContainer contains
        /// </summary>
        public NPC NPC;

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
        /// Creates a new instance of the CheatNPCContainer class
        /// </summary>
        public CheatNPCContainer()
            : this(new NPC())
        {

        }
        /// <summary>
        /// Creates a new instance of the CheatNPCContainer class
        /// </summary>
        /// <param name="n">The NPC of the CheatNPCContainer</param>
        public CheatNPCContainer(NPC n)
        {
            KeepFiring = true;

            NPC = n;
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            if (CurrentNetID == NPC.netID)
                Amount++;
            else
            {
                CurrentNetID = NPC.netID;
                Amount = 1;
            }
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (NPC.type > 0)
            {
                Main.LoadNPC(NPC.type);

                // frame stuff
                if (!changesFrame.ContainsKey(NPC.name))
                    changesFrame.Add(NPC.name, false);

                if (!changesFrame[NPC.name])
                    NPC.frame = new Rectangle(0, 0, Main.npcTexture[NPC.type].Width, Main.npcTexture[NPC.type].Height / Main.npcFrameCount[NPC.type]);

                double oldFrameCounter = NPC.frameCounter;

                NPC.FindFrame();

                if (oldFrameCounter != NPC.frameCounter)
                    changesFrame[NPC.name] = true;

                // display name
                Tooltip = NPC.displayName.IsEmpty() ? NPC.name : NPC.displayName;

                // internal name
                if (!NPC.displayName.IsEmpty() && NPC.displayName != NPC.name)
                    Tooltip += " (" + NPC.name + ")";
            }
            else
                Tooltip = "No NPC";
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.Draw(bgTex, Position, null, MainUI.WithAlpha(Color.White, 150), Rotation, Origin, Scale, SpriteEffects, LayerDepth);

            if (NPC.type > 0)
            {
                Main.LoadNPC(NPC.type);

                Color c = NPC.GetColor(Color.White);

                if (c == new Color(0, 0, 0, 0))
                    c = NPC.GetAlpha(Color.White);

                // make a bit smaller if too big (like EoC sized)
                Vector2
                    scale = Scale * NPC.scale,
                    size = NPC.frame.Size() * scale;

                //new Rectangle((int)Position.X - Math.Min(NPC.Width / 2, Hitbox.Width), (int)Position.Y - Math.Min(NPC.Height / 2, Hitbox.Width),
                //    Math.Min(NPC.Width, Hitbox.Width), Math.Min(NPC.Height, Hitbox.Width))

                if (size.X > 86f || size.Y > 86f)
                    scale *= size.X > size.Y ? Hitbox.Width / size.X : Hitbox.Height / size.Y;

                sb.Draw(Main.npcTexture[NPC.type], Position + (bgTex.Size() / 2f - (NPC.frame.Size() * scale) / 2f), NPC.frame,
                    c, Rotation, Origin, scale, SpriteEffects, LayerDepth);
            }
        }
    }
}
