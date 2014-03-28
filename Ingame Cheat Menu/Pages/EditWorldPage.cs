using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Graphics;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.MenuItems;

namespace PoroCYon.ICM.Pages
{
    /// <summary>
    /// The MenuPage used to edit a world
    /// </summary>
    public sealed class EditWorldPage : Page
    {
        internal static string selectedWorld, selectedWorldPath;

        /// <summary>
        /// Creates a new instance of the EditWorldPage class
        /// </summary>
        public EditWorldPage()
            : base()
        {

        }

        /// <summary>
        /// Initializes the Page
        /// </summary>
        protected override void Init()
        {
            base.Init();

            MenuAnchor
                aLeft = new MenuAnchor()  
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(-105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aRight = new MenuAnchor() 
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aCentre = new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(0f, 200f),
                    offset_button = new Vector2(0f, 50f)
                };

            int bid = 0;

            anchors.AddRange(new List<MenuAnchor>() { aLeft, aRight, aCentre });

            buttons.Add(new MenuButton(0, "Back", "World Select").Where(mb => mb.SetAutomaticPosition(aCentre, bid++)));
        }
    }
}
