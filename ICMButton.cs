using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;
using TAPI.SDK.GUI.Controls.Primitives;
using TAPI.PoroCYon.ICM.Menus;

namespace TAPI.PoroCYon.ICM
{
    /// <summary>
    /// A button that changes the current interface type
    /// </summary>
    public sealed class ICMButton : Button
    {
        /// <summary>
        /// The InterfaceType the interface will change to when the button is clicked
        /// </summary>
        public InterfaceType ChangeTo = InterfaceType.None;

        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 24, 24);
            }
        }

        /// <summary>
        /// Creates a new instance of the ICMButton class
        /// </summary>
        public ICMButton()
            : base()
        {
            StayFocused = StaysPressed = false;
        }
        /// <summary>
        /// Creates a new instance of the ICMButton class with the specified InterfaceType value
        /// </summary>
        /// <param name="to">The InterfaceType the interface will change to when the button is clicked</param>
        public ICMButton(InterfaceType to)
            : this()
        {
            ChangeTo = to;
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(SdkUI.WhitePixel, Position, null, MainUI.GrayColour(Hitbox, SettingsUI.ColourAccent), 0f, Vector2.Zero, 24f, SpriteEffects.None, 0f);

            base.Draw(sb);
        }

        /// <summary>
        /// Called when the Button is clicked
        /// </summary>
        protected override void Clicked()
        {
            MainUI.ChangeToUI(ChangeTo);

            base.Clicked();
        }
    }
}
