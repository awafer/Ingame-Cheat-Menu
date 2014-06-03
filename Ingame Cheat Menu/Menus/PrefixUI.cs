using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM Prefix cheat UI
    /// </summary>
    public sealed class PrefixUI : CheatUI<Prefix>
    {
        /// <summary>
        /// The length of the Prefix list
        /// </summary>
        public const int PREFIX_LIST_LENGTH = 9;

        /// <summary>
        /// The 20 Prefix containers which contain the currently displayed Prefixes
        /// </summary>
        public static CheatPrefixContainer[] PrefixContainers
        {
            get;
            private set;
        }

        /// <summary>
        /// The checkbox wich is used to determinate wether to use safe prefix setting or not
        /// </summary>
        public static CheckBox AvoidWrong
        {
            get;
            private set;
        }
        /// <summary>
        /// The TextButton used to remove a Prefix from the Item
        /// </summary>
        public static TextButton Delete
        {
            get;
            private set;
        }

        /// <summary>
        /// The container which contains the Item where the Prefix should be set to
        /// </summary>
        public static SimpleItemContainer ToSet
        {
            get;
            private set;
        }

        /// <summary>
        /// The PrefixUI singleton instance
        /// </summary>
        public static PrefixUI Interface;

        /// <summary>
        /// The Item instance which is put in ToSet
        /// </summary>
        public static Item ItemToSet
        {
            get
            {
                return ToSet.Item;
            }
        }

        /// <summary>
        /// Creates a new instance of the PrefixUI class
        /// </summary>
        public PrefixUI()
            : base(InterfaceType.Prefix)
        {

        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();

            LeftButton.Position.X += 20f;
            RightButton.Position.X += 60f;

            AddControl(ToSet = new SimpleItemContainer()
            {
                InventoryBackTextureNum = 5,

                Position = new Vector2(550f, 500f)
            });
            AddControl(AvoidWrong = new CheckBox(true, "Use safe prefix reforging")
            {
                Tooltip = "With safe prefix setting, the game determinates\nwether the selected prefix can be applied to an Item or not.\nIf possible, it sets the prefix. Otherwise, the prefix is removed.",

                Position = new Vector2(550f, 650f)
            });

            for (int i = 0; i < FILTER_OPTIONS_LENGTH; i++)
                FilterOptions[i].Destroy();
        }

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            float backupScale = Main.inventoryScale;
            Main.inventoryScale = 1f;

            base.Update();

            Main.inventoryScale = backupScale;
        }

        /// <summary>
        /// Draws the Control
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            float backupScale = Main.inventoryScale;
            Main.inventoryScale = 1f;

            base.Draw(sb);

            Main.inventoryScale = backupScale;
        }

        /// <summary>
        /// Clears the Prefix list and fills it, with the current filters
        /// </summary>
        /// <param name="thisThread">Wether to load it on the current thread or on a new one</param>
        public override void ResetObjectList(bool thisThread = false)
        {
            ThreadStart start = () =>
            {
                objects.Clear();

                objects.AddRange(from Prefix p in Defs.prefixes.Values where IncludeInList(p) select p.Clone());

                ResetContainers();
            };

            if (ResetThread != null && ResetThread.ThreadState == ThreadState.Running)
                ResetThread.Abort();

            if (thisThread)
                start();
            else
                (ResetThread = new Thread(start)).Start();
        }
        /// <summary>
        /// Resets the BuffContainer content
        /// </summary>
        public override void ResetContainers()
        {
            for (int i = Position; i < Position + PREFIX_LIST_LENGTH; i++)
            {
                PrefixContainers[i - Position].Prefix = i >= objects.Count ? Prefix.None : objects[i];
                PrefixContainers[i - Position].CanFocus = i < objects.Count;
            }
        }

        /// <summary>
        /// Creates the object container list
        /// </summary>
        protected override void CreateContainers()
        {
            if (PrefixContainers == null)
                PrefixContainers = new CheatPrefixContainer[PREFIX_LIST_LENGTH];

            int row = 0, col = 0;
            for (int i = 0; i < PREFIX_LIST_LENGTH; i++, col++)
            {
                if (PrefixContainers[i] == null)
                    PrefixContainers[i] = new CheatPrefixContainer();

                if (col >= 3)
                {
                    row++;
                    col = 0;
                }

                PrefixContainers[i].Position = new Vector2(190f + row * 100f, (Main.screenHeight - 330f) + col * 60f);

                AddControl(PrefixContainers[i]);
            }
        }
        /// <summary>
        /// Disposes the object container list
        /// </summary>
        protected override void RemoveContainers()
        {
            for (int i = 0; i < PREFIX_LIST_LENGTH; i++)
                RemoveControl(PrefixContainers[i]);

            PrefixContainers = null;
        }

        /// <summary>
        /// Checks wether a Prefix is a result of the given search string
        /// </summary>
        /// <param name="p">The Prefix to check</param>
        /// <returns>true if the Prefix matches the current search string, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsSearchResult(Prefix p)
        {
            if (SearchBox.Text.IsEmpty() || !ChangedSearchText)
                return true;

            return IsSearchResult(p, SearchBox.Text);
        }
        /// <summary>
        /// Checks wether a Prefix is a result of the given search string
        /// </summary>
        /// <param name="p">The Prefix to check</param>
        /// <param name="search">The searched </param>
        /// <returns>true if the Prefix matches the search string, false otherwise.</returns>
        public static bool IsSearchResult(Prefix p, string search)
        {
            if (search.IsEmpty())
                return true;
            if (p.Equals(Prefix.None))
                return false;

            return ExcludeSpecialChars(p.displayName).ToLower().Contains(search);
        }

        /// <summary>
        /// Wether to include a Prefix in the item list or not
        /// </summary>
        /// <param name="p">A Prefix to check</param>
        /// <returns>true if the Prefix should be included, false otherwise.</returns>
        public bool IncludeInList(Prefix p)
        {
            //bool ret = IsInCategory(p);

            //if (FilterOptions[0].IsChecked)
            //    return ret && IsSearchResult(p);
            //if (FilterOptions[1].IsChecked)
            //    return ret || IsSearchResult(p);
            //if (FilterOptions[2].IsChecked)
            //    return ret ^ IsSearchResult(p);

            string disp = p.displayName;
            if (String.IsNullOrEmpty(disp))
            {
                string[] split = p.name.Split(':');

                if (split.Length <= 1)
                    return false;

                disp = split[1];

                for (int i = 2; i < split.Length; i++)
                    disp += ":" + split[i];
            }

            return !p.Equals(Prefix.None) && !String.IsNullOrEmpty(disp) && IsSearchResult(p);
        }
    }
}
