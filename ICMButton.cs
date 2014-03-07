using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.Interface.Controls;
using TAPI.SDK.UI.Interface.Controls.Primitives;
using TAPI.SDK.Input;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM
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

        /// <summary>
        /// The hitbox of the Control
        /// </summary>
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
            StayFocused = false;
        }
        /// <summary>
        /// Creates a new instance of the ICMButton class with the specified InterfaceType value
        /// </summary>
        /// <param name="to">The InterfaceType the interface will change to when the button is clicked</param>
        public ICMButton(InterfaceType to)
            : this()
        {
            Tooltip = (ChangeTo = to).ToString();
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(SdkUI.WhitePixel, Position, null, Color.Lerp(Colour, Color.Black, Hitbox.Intersects(GInput.Mouse.Rectangle)
                || MainUI.UIType == ChangeTo ? 0f : 0.5f), 0f, Vector2.Zero, 24f, SpriteEffects.None, 0f);

            base.Draw(sb);
        }

        /// <summary>
        /// Clicks the Button
        /// </summary>
        protected override void Click()
        {
            base.Click();

            MainUI.ChangeToUI(ChangeTo);
        }
    }
}
