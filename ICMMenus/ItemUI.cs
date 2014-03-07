using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI.SDK.Content;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;

namespace TAPI.PoroCYon.ICM.Menus
{
    /// <summary>
    /// All Item categories. Enumeration is marked as Flags.
    /// </summary>
    [Flags]
    public enum Category : ushort
    {
        /// <summary>
        /// Nothing
        /// </summary>
        None = 0,

        /// <summary>
        /// Melee weapons
        /// </summary>
        Melee = 1,
        /// <summary>
        /// Ranged weapons
        /// </summary>
        Ranged = 2,
        /// <summary>
        /// Magic weapons
        /// </summary>
        Magic = 4,

        /// <summary>
        /// Helmets
        /// </summary>
        Helmet = 8,
        /// <summary>
        /// Torsos
        /// </summary>
        Torso = 16,
        /// <summary>
        /// Leggings
        /// </summary>
        Leggings = 32,

        /// <summary>
        /// Vanity items
        /// </summary>
        Vanity = 64,
        /// <summary>
        /// Accessories
        /// </summary>
        Accessories = 128,
        /// <summary>
        /// Tools (pickaxes, axes and hammers)
        /// </summary>
        Tools = 256,

        /// <summary>
        /// Ammo
        /// </summary>
        Ammunition = 1024,
        /// <summary>
        /// Materials (used to craft something else)
        /// </summary>
        Materials = 2048,
        /// <summary>
        /// Consumables (stack decreases when used)
        /// </summary>
        Consumables = 4096,

        /// <summary>
        /// Tiles (Item places a tile when used)
        /// </summary>
        Tiles = 8192,
        /// <summary>
        /// Walls (Item places a wall when used)
        /// </summary>
        Walls = 16384,
        /// <summary>
        /// Anything else
        /// </summary>
        Others = 32768,

        /// <summary>
        /// All categories
        /// </summary>
        All = Melee | Ranged | Magic | Helmet | Torso | Leggings | Vanity | Accessories
            | Tools | Ammunition | Materials | Consumables | Tiles | Walls | Others
    }

    /// <summary>
    /// The ICM Item cheat UI
    /// </summary>
    public sealed class ItemUI : CheatUI
    {
        /// <summary>
        /// The amount of items displayed
        /// </summary>
        public const int ITEM_LIST_LENGTH = 20;

        static TextBox searchBox = new TextBox();
        static ControlGroup items = new ControlGroup();

        static Texture2D left, right;

        /// <summary>
        /// The position of the Item list
        /// </summary>
        public static int Position = 0;
        /// <summary>
        /// The current category
        /// </summary>
        public static Category Category = Category.All;

        /// <summary>
        /// The ItemUI singleton instance
        /// </summary>
        public static ItemUI Interface;

        /// <summary>
        /// Creates a new instance of the ItemUI class
        /// </summary>
        public ItemUI()
            : base(InterfaceType.Item)
        {

        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public override void Open()
        {
            RefillItemSlots();
        }
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            items.Controls.Clear();
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            left = ObjectLoader.LoadTexture(Mod.ModInstance.files["Sprites/Left.png"]);
            right = ObjectLoader.LoadTexture(Mod.ModInstance.files["Sprites/Right.png"]);

            base.Init();

            AddControl(items);
            RefillItemSlots();

            AddControl(new ImageButton(left) 
            {
                Position = new Vector2(160f, Main.screenHeight - 208),
                Click = (b) =>
                {
                    Position -= 4;
                    if (Position < -45) // lowest netID
                        Position = -45;
                }
            });
            AddControl(new ImageButton(right)
            {
                Position = new Vector2(380f, Main.screenHeight - 208),
                Click = (b) =>
                {
                    Position += 4;
                    if (Position >= Defs.items.Count)
                        Position = Defs.items.Count - 1;
                }
            });
        }

        /// <summary>
        /// Clears the Item list and fills it, with the current filters
        /// </summary>
        public void RefillItemSlots()
        {
            items.Controls.Clear();

            int row = 0, col = 0, added = 0;

            for (int i = Position; i < Position + ITEM_LIST_LENGTH + added; i++, col++)
            {
                if (i > Defs.items.Count)
                    break; // the end of all item defs
                if (!Defs.itemNames.ContainsKey(i))
                    continue;

                if (!IncludeInList(Defs.items[Defs.itemNames[i]]))
                {
                    added++;
                    Position++;
                    continue;
                }

                if (col >= 4)
                {
                    row++;
                    col = 0;
                }

                items.AddControl(new AutoRefillingItemContainer(Defs.items[Defs.itemNames[i]])
                {
                    InventoryBackTextureNum = 7,
                    Position = new Vector2(160f + row * Main.inventoryBack7Texture.Width, (Main.screenHeight - 350f) + col * Main.inventoryBack7Texture.Height),
                    Colour = MainUI.WithAlpha(SettingsUI.ColourTheme, 150)
                });
            }

            // reset this, pretty important
            Position -= added;
        }

        /// <summary>
        /// Checks wether an Item is in the current category or not.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <returns>true if the Item is in the current category, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public static bool IsInCategory(Item i)
        {
            return IsInCategory(i, Category);
        }
        /// <summary>
        /// Checks wether an Item is in a certain category or not.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="cat">The Category to compare the Item with</param>
        /// <returns>true if the Item is in the category, false otherwise.</returns>
        public static bool IsInCategory(Item i, Category cat)
        {
            if (cat == Category.None)
                return false;
            if (cat == Category.All)
                return true;

            bool ret = true;

            if ((cat & Category.Melee) != 0)
                ret &= i.melee;
            if ((cat & Category.Ranged) != 0)
                ret &= i.ranged;
            if ((cat & Category.Magic) != 0)
                ret &= i.magic;

            if ((cat & Category.Helmet) != 0)
                ret &= i.headSlot > 0;
            if ((cat & Category.Torso) != 0)
                ret &= i.bodySlot > 0;
            if ((cat & Category.Leggings) != 0)
                ret &= i.legSlot > 0;

            if ((cat & Category.Vanity) != 0)
                ret &= i.vanity;
            if ((cat & Category.Accessories) != 0)
                ret &= i.accessory;
            if ((cat & Category.Tools) != 0)
                ret &= i.pick > 0 || i.hammer > 0 || i.axe > 0;

            if ((cat & Category.Ammunition) != 0)
                ret &= i.ammo > 0 && !i.notAmmo;
            if ((cat & Category.Materials) != 0)
                ret &= i.material && !i.notMaterial;
            if ((cat & Category.Consumables) != 0)
                ret &= i.consumable;

            if ((cat & Category.Tiles) != 0)
                ret &= i.createTile > 0;
            if ((cat & Category.Walls) != 0)
                ret &= i.createWall > 0;
            if ((cat & Category.Others) != 0)
                ret &= !ret;

            return ret;
        }

        /// <summary>
        /// Checks wether an Item is a result of the given search string
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <returns>true if the Item matches the current search string, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public static bool IsSearchResult(Item i)
        {
            if (searchBox.Text.IsEmpty())
                return true;

            return IsSearchResult(i, searchBox.Text);
        }
        /// <summary>
        /// Checks wether an Item is a result of the given search string
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="search">The searched </param>
        /// <returns>true if the Item matches the search string, false otherwise.</returns>
        public static bool IsSearchResult(Item i, string search)
        {
            if (search.IsEmpty())
                return true;

            return ExcludeSpecialChars(i.displayName).ToLower().Contains(search); // SDMG will also find 'S.D.M.G.'
        }

        /// <summary>
        /// Wether to include an Item in the item list or not
        /// </summary>
        /// <param name="i">An Item to check</param>
        /// <returns>true if the Item should be included, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public static bool IncludeInList(Item i)
        {
            return IsInCategory(i) && IsSearchResult(i);
        }

        /// <summary>
        /// Excludes all 'special' (not an alphanumerical character or a space) characters from a string
        /// </summary>
        /// <param name="s">The string to exclude all special characters from</param>
        /// <returns><paramref name="s"/> without any special characters</returns>
        public static string ExcludeSpecialChars(string s)
        {
            string ret = "";

            for (int i = 0; i < s.Length; i++)
                if ((s[i] >= '0' && s[i] <= '9') || (s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= 'a' && s[i] <= 'z') || s[i] == ' ') // 0-9, A-Z, a-z, space
                    ret += s[i];

            return ret;
        }
    }
}
