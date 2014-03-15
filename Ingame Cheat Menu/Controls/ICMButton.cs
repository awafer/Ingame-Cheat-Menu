using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.MCT.UI.Interface.Controls.Primitives;
using PoroCYon.MCT.Input;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
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
                return new Rectangle((int)Position.X + 4, (int)Position.Y + 4, 40, 40);
            }
        }

        /// <summary>
        /// Creates a new instance of the ICMButton class
        /// </summary>
        public ICMButton()
            : this(InterfaceType.None)
        {

        }
        /// <summary>
        /// Creates a new instance of the ICMButton class with the specified InterfaceType value
        /// </summary>
        /// <param name="to">The InterfaceType the interface will change to when the button is clicked</param>
        public ICMButton(InterfaceType to)
            : base()
        {
            StayFocused = false;
            Tooltip = (ChangeTo = to).ToString();
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            if (HasBackground)
            {
                Rectangle bg =  new Rectangle((int)Position.X - 8, (int)Position.Y - 8, 40, 40);
                Drawing.DrawBlueBox(sb, bg.X, bg.Y, bg.Width, bg.Height, IsHovered || MainUI.UIType == ChangeTo ? 0.85f : 0.75f);
            }

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
