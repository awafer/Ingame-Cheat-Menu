using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM Buff cheat UI
    /// </summary>
    public sealed class BuffUI : CheatUI<Buff>
    {
        /// <summary>
        /// All Buff categories. Enumeration is marked as Flags.
        /// </summary>
        [Flags]
        public enum Categories : byte
        {
            None = 0,

            Buff = 1,
            Debuff = 2,
            WeaponBuff = 4,

            All = Buff | Debuff | WeaponBuff
        }

        /// <summary>
        /// The amount of categories
        /// </summary>
        public const int CATEGORY_LIST_LENGTH = 3;

        /// <summary>
        /// The current Category
        /// </summary>
        public static Categories Category = Categories.None;

        /// <summary>
        /// The BuffUI singleton instance
        /// </summary>
        public static BuffUI Interface;

        /// <summary>
        /// Creates a new instance of the BuffUI class
        /// </summary>
        public BuffUI()
            : base(InterfaceType.Buff)
        {

        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public override void Open()
        {
            
        }
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();
        }
    }
}
