using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.Input;
using PoroCYon.ICM.Controls;
using PoroCYon.ICM.Menus;
using PoroCYon.ICM.Menus.Sub;

namespace PoroCYon.ICM
{
    /// <summary>
    /// All interface types
    /// </summary>
    public enum InterfaceType : sbyte
    {
        /// <summary>
        /// Editing global NPC variables
        /// </summary>
        [Obsolete("This code is not used.")]
        EditGlobalNPC = -2,
        /// <summary>
        /// Editing an Item instance
        /// </summary>
        [Obsolete("This code is not used.")]
        EditItem = -1,

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
        World = 6
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
                        return ItemUI.Instance;
                    case InterfaceType.Buff:
                        return BuffUI.Instance;
                    case InterfaceType.Prefix:
                        return PrefixUI.Instance;
                    case InterfaceType.NPC:
                        return NpcUI.Instance;
                    case InterfaceType.Player:
                        return PlayerUI.Instance;
                    case InterfaceType.World:
                        return WorldUI.Instance;

                    //case InterfaceType.EditGlobalNPC:
                    //    return EditGlobalNPCUI.Interface;
                    //case InterfaceType.EditItem:
                    //    return EditItemUI.Interface;
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
            Visibility = Visibility.Inventory;
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
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.itemTexture[1], c.Position, null, Color.White, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);
                }
            });
            #endregion
            #region Buff    
            AddControl(new ICMButton(InterfaceType.Buff)
            {
                Tooltip = "Buffs",
                Position = PositionOf(InterfaceType.Buff),
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.buffTexture[1], c.Position, null, Color.White, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);
                }
            });
            #endregion
            #region Prefix  
            AddControl(new ICMButton(InterfaceType.Prefix)
            {
                Tooltip = "Reforge",
                Position = PositionOf(InterfaceType.Prefix),
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.npcHeadTexture[9], c.Position, null, Color.White, c.Rotation, c.Origin, c.Scale, c.SpriteEffects, c.LayerDepth);
                }
            });
            #endregion
            #region NPC     
            AddControl(new ICMButton(InterfaceType.NPC)
            {
                Tooltip = "NPCs",
                Position = PositionOf(InterfaceType.NPC),
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    Main.LoadNPC(1);
                    sb.Draw(Main.npcTexture[1], c.Position, new Rectangle(0, 0, Main.npcTexture[1].Width, 24),
                        new Color(0, 80, 255, 200), c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);
                }
            });
            #endregion
            #region Player  
            AddControl(new ICMButton(InterfaceType.Player)
            {
                Tooltip = "Player",
                Position = PositionOf(InterfaceType.Player),
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.playerHeadTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerHeadTexture.Width, 30),
                        Main.localPlayer.skinColor, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);

                    sb.Draw(Main.playerEyeWhitesTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyeWhitesTexture.Width, 30),
                        Color.White, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);

                    sb.Draw(Main.playerEyesTexture, c.Position + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyesTexture.Width, 30),
                        Main.localPlayer.eyeColor, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);

                    sb.Draw(Main.playerHairTexture[Main.localPlayer.hair], c.Position + new Vector2(-4, 2), new Rectangle(0, 6,
                        Main.playerHairTexture[Main.localPlayer.hair].Width, 30),
                        Main.localPlayer.hairColor, c.Rotation, c.Origin, c.Scale * 0.75f, c.SpriteEffects, c.LayerDepth);

                }
            });
            #endregion
            #region World   
            AddControl(new ICMButton(InterfaceType.World)
            {
                Tooltip = "World",
                Position = PositionOf(InterfaceType.World),
                //Colour = WithAlpha(SettingsPage.ColourTheme, 255),

                OnDraw = (c, sb) =>
                {
                    sb.Draw(Main.dayTime ? (Main.eclipse ? Main.sun3Texture : Main.sunTexture) : (Main.pumpkinMoon ? Main.pumpkinMoonTexture : Main.moonTexture[0]),
                        c.Position + (Main.dayTime ? new Vector2(-17f) : Vector2.Zero), Main.dayTime ? null :
                        new Rectangle?(new Rectangle(0, Main.moonPhase * Main.moonTexture[0].Width, Main.moonTexture[0].Width, Main.moonTexture[0].Width)),
                        Color.White, c.Rotation, c.Origin, c.Scale * 0.5f, c.SpriteEffects, c.LayerDepth);
                }
            });
            #endregion
        }

        /// <summary>
        /// Draws the CustomUI
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw things</param>
        public override void Draw(SpriteBatch sb)
        {
            // change sb mode
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            base.Draw(sb);

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
            if (Current != null)
                Current.Close();

            UIType = UIType == type ? InterfaceType.None : type;

            if (Current != null)
                Current.Open();
        }

        /// <summary>
        /// Makes a colour more gray-ish when the mouse is not hovering on it
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        [TargetedPatchingOptOut(TPOOReason)]
        public static Color GrayColour(Rectangle hitbox, Color colour)
        {
            return Color.Lerp(colour, Color.Black, hitbox.Intersects(GInput.Mouse.Rectangle) ? 0.5f : 0f);
        }

        /// <summary>
        /// Gets the position of the specified interface button
        /// </summary>
        /// <param name="button">The interface button</param>
        /// <returns>The position of the interface button</returns>
        [TargetedPatchingOptOut(TPOOReason)]
        public static Vector2 PositionOf(InterfaceType button)
        {
            return new Vector2(100f + 40f * (int)button, Main.screenHeight - 100f);
        }

        /// <summary>
        /// Sets the alpha channel of a Color and returns it
        /// </summary>
        /// <param name="colour">The colour to change</param>
        /// <param name="alpha">The new alpha channel value</param>
        /// <returns>The colour with a new alpha value</returns>
        [TargetedPatchingOptOut(TPOOReason)]
        public static Color WithAlpha(Color colour, byte alpha)
        {
            return new Color(colour.R, colour.G, colour.B, alpha);
        }
    }
}
