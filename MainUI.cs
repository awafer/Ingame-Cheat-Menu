using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Graphics;
using TAPI.SDK;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;
using TAPI.SDK.Input;
using TAPI.PoroCYon.ICM.Menus;
using TAPI.PoroCYon.ICM.Menus.Sub;

namespace TAPI.PoroCYon.ICM
{
    /// <summary>
    /// All interface types
    /// </summary>
    public enum InterfaceType : int
    {
        /// <summary>
        /// Editing an NPC instance
        /// </summary>
        EditNPC = -4,
        /// <summary>
        /// Editing an Item instance
        /// </summary>
        EditItem = -3,
        /// <summary>
        /// Editing global NPC variables
        /// </summary>
        EditGlobalNPC = -2,
        /// <summary>
        /// Editing the local Player instance
        /// </summary>
        EditPlayer = -1,

        /// <summary>
        /// No UI is shown
        /// </summary>
        None = 0,

        /// <summary>
        /// The Item menu
        /// </summary>
        Item = 1,
        /// <summary>
        /// The Buff menu
        /// </summary>
        Buff = 2,
        /// <summary>
        /// The Prefix menu
        /// </summary>
        Prefix = 3,
        /// <summary>
        /// The NPC menu
        /// </summary>
        NPC = 4,
        /// <summary>
        /// The Player menu
        /// </summary>
        Player = 5,
        /// <summary>
        /// The World menu
        /// </summary>
        World = 6,

        /// <summary>
        /// UI display settings
        /// </summary>
        Settings = 7
    }

    /// <summary>
    /// Displays the UI buttons
    /// </summary>
    public sealed class MainUI : CustomUI
    {
        // MethodImplOptions.AggressiveInlining is introduced in .NET 4.5
        /// <summary>
        /// The Reason property for all TargetPatchingOptOut attributes
        /// </summary>
        internal const string TPOOReason = "Performance critical to inline across NGen image boundaries";

        /// <summary>
        /// The MainUI singleton instance
        /// </summary>
        public static MainUI Interface = new MainUI();

        /// <summary>
        /// The current interface type
        /// </summary>
        public static InterfaceType UIType = InterfaceType.None;

        /// <summary>
        /// The current cheat menu. null is none. To change it, call MainUI.ChangeToUI
        /// </summary>
        public static CheatUI Current
        {
            get
            {
                switch (UIType)
                {
                    case InterfaceType.None:
                        return null;

                    case InterfaceType.Item:
                        return ItemUI.Interface;
                    case InterfaceType.Buff:
                        return BuffUI.Interface;
                    case InterfaceType.Prefix:
                        return PrefixUI.Interface;
                    case InterfaceType.NPC:
                        return NPCUI.Interface;
                    case InterfaceType.Player:
                        return PlayerUI.Interface;
                    case InterfaceType.World:
                        return WorldUI.Interface;

                    case InterfaceType.Settings:
                        return SettingsUI.Interface;

                    case InterfaceType.EditPlayer:
                        return EditPlayerUI.Interface;
                    case InterfaceType.EditGlobalNPC:
                        return EditGlobalNPCUI.Interface;
                    case InterfaceType.EditItem:
                        return EditItemUI.Interface;
                    case InterfaceType.EditNPC:
                        return EditNPCUI.Interface;
                }

                throw new ArgumentOutOfRangeException("UIType");
            }
        }

        /// <summary>
        /// Creates a new instance of the MainUI class
        /// </summary>
        public MainUI()
            : base()
        {
            Visibility = Visibility.IngameInv;
            IsDrawnAfter = true;
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();

            #region Item    
            AddControl(new ICMButton(InterfaceType.Item)
            {
                Tooltip = "Items",
                Position = PositionOf(InterfaceType.Item),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.itemTexture[1], c.Position, null, GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                }
            });
            #endregion
            #region Buff    
            AddControl(new ICMButton(InterfaceType.Buff)
            {
                Tooltip = "Buffs",
                Position = PositionOf(InterfaceType.Buff),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.buffTexture[1], c.Position, null, GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                }
            });
            #endregion
            #region Prefix  
            AddControl(new ICMButton(InterfaceType.Prefix)
            {
                Tooltip = "Reforge",
                Position = PositionOf(InterfaceType.Prefix),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.npcHeadTexture[9], c.Position, null, GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            });
            #endregion
            #region NPC     
            AddControl(new ICMButton(InterfaceType.NPC)
            {
                Tooltip = "NPCs",
                Position = PositionOf(InterfaceType.NPC),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.npcTexture[1], c.Position, new Rectangle(0, 0, Main.npcTexture[1].Width, 24),
                        GrayColour(c.Hitbox, new Color(0, 80, 255, 200)), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                }
            });
            #endregion
            #region Player  
            AddControl(new ICMButton(InterfaceType.Player)
            {
                Tooltip = "Player",
                Position = PositionOf(InterfaceType.Player),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.playerHeadTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerHeadTexture.Width, 30),
                        GrayColour(c.Hitbox, Main.localPlayer.skinColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                    sb.Draw(Main.playerEyeWhitesTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyeWhitesTexture.Width, 30),
                        GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                    sb.Draw(Main.playerEyesTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyesTexture.Width, 30),
                        GrayColour(c.Hitbox, Main.localPlayer.eyeColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                    sb.Draw(Main.playerHairTexture[Main.localPlayer.hair], c.Position + new Vector2(-4, 2), new Rectangle(0, 6,
                        Main.playerHairTexture[Main.localPlayer.hair].Width, 30),
                        GrayColour(c.Hitbox, Main.localPlayer.hairColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                }
            });
            #endregion
            #region World   
            AddControl(new ICMButton(InterfaceType.World)
            {
                Tooltip = "World",
                Position = PositionOf(InterfaceType.World),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.dayTime ? (Main.eclipse ? Main.sun3Texture : Main.sunTexture) : (Main.pumpkinMoon ? Main.pumpkinMoonTexture : Main.moonTexture[0]),
                        c.Position + (Main.dayTime ? new Vector2(-17f) : Vector2.Zero), Main.dayTime ? null :
                        new Rectangle?(new Rectangle(0, Main.moonPhase * Main.moonTexture[0].Width, Main.moonTexture[0].Width, Main.moonTexture[0].Width)),
                        GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                }
            });
            #endregion
            #region Settings
            AddControl(new ICMButton(InterfaceType.Settings)
            {
                Tooltip = "Settings",
                Position = PositionOf(InterfaceType.Settings),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.itemTexture[1344], c.Position + new Vector2(1f), null, GrayColour(c.Hitbox, Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            });
            #endregion
        }

        /// <summary>
        /// Draws the CustomUI
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw things</param>
        /// <param name="after">Wether it is called in Pre- or PostDrawInterface</param>
        public override void Draw(SpriteBatch sb, bool after)
        {
            // change sb mode
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            base.Draw(sb, after);

            sb.End();
            sb.Begin();
        }

        /// <summary>
        /// Changes the current interface to the specified type,
        /// or, when it's already the current, resets it to None
        /// </summary>
        /// <param name="type">The new interface type</param>
        public static void ChangeToUI(InterfaceType type)
        {
            Current.Close();

            UIType = UIType == type ? InterfaceType.None : type;

            Current.Open();
        }

        /// <summary>
        /// Makes a colour more gray-ish when the mouse is not hovering on it
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        [TargetedPatchingOptOut(TPOOReason)] // small+simple enough to be inlined
        public static Color GrayColour(Rectangle hitbox, Color colour)
        {
            return Color.Lerp(colour, Color.Black, hitbox.Intersects(GInput.Mouse.Rectangle) ? 0.5f : 0f);
        }

        /// <summary>
        /// Gets the position of the specified interface button
        /// </summary>
        /// <param name="button">The interface button</param>
        /// <returns>The position of the interface button</returns>
        [TargetedPatchingOptOut(TPOOReason)] // small+simple enough to be inlined
        public static Vector2 PositionOf(InterfaceType button)
        {
            return new Vector2(100f + 24f * (int)button, Main.screenHeight - 100f);
        }
    }
}
