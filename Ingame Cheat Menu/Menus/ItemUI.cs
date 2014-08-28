using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM Item cheat UI
    /// </summary>
    public sealed class ItemUI : CheatUI<Item>
    {
        /// <summary>
        /// All Item categories. Enumeration is marked as Flags.
        /// </summary>
        [Flags]
        public enum Categories : int
        {
            // hex ftw

            /// <summary>
            /// Nothing
            /// </summary>
            None = 0x0,

            /// <summary>
            /// Melee weapons
            /// </summary>
            Melee = 0x1,
            /// <summary>
            /// Ranged weapons
            /// </summary>
            Ranged = 0x2,
            /// <summary>
            /// Magic weapons
            /// </summary>
            Magic = 0x4,
            /// <summary>
            /// A weapon (melee, ranged or magic)
            /// </summary>
            Weapon = Melee | Ranged | Magic,

            /// <summary>
            /// Helmets
            /// </summary>
            Helmet = 0x8,
            /// <summary>
            /// Torsos
            /// </summary>
            Torso = 0x10,
            /// <summary>
            /// Leggings
            /// </summary>
            Leggings = 0x20,
            /// <summary>
            /// Helmets, Torsos and Leggings
            /// </summary>
            Armour = Helmet | Torso | Leggings,

            /// <summary>
            /// Vanity items
            /// </summary>
            Vanity = 0x40,
            /// <summary>
            /// Accessories
            /// </summary>
            Accessory = 0x80,
            /// <summary>
            /// The item gives the player a buff or debuff
            /// </summary>
            Buff = 0x100,

            /// <summary>
            /// Pickaxes
            /// </summary>
            Pickaxe = 0x200,
            /// <summary>
            /// Axes
            /// </summary>
            Axe = 0x400,
            /// <summary>
            /// Hammers
            /// </summary>
            Hammer = 0x800,
            /// <summary>
            /// Tools (pickaxes, axes and hammers)
            /// </summary>
            Tools = Pickaxe | Axe | Hammer,

            /// <summary>
            /// Ammo
            /// </summary>
            Ammunition = 0x1000,
            /// <summary>
            /// Materials (used to craft something else)
            /// </summary>
            Material = 0x2000,
            /// <summary>
            /// The Item heals life or mana
            /// </summary>
            Potion = 0x4000,

            /// <summary>
            /// Tiles (Item places a tile when used)
            /// </summary>
            Tile = 0x8000,
            /// <summary>
            /// Walls (Item places a wall when used)
            /// </summary>
            Wall = 0x10000,
            /// <summary>
            /// Paint
            /// </summary>
            Paint = 0x20000,

            /// <summary>
            /// The item is a dye
            /// </summary>
            Dye = 0x40000,
            /// <summary>
            /// Pet summoning item
            /// </summary>
            Pet = 0x80000,
            /// <summary>
            /// Summon (defender, not a boss) item
            /// </summary>
            Summon = 0x100000,

            /// <summary>
            /// Anything else
            /// </summary>
            Other = 0x200000,

            /// <summary>
            /// All categories
            /// </summary>
            All = Weapon | Armour | Vanity | Accessory | Ammunition | Material | Potion | Tile | Wall | Dye | Paint | Pet | Summon | Tools | Other

            // Count = 22
        }

        /// <summary>
        /// An IEnumerable which provides an iterator over all available Items.
        /// foreach (Item i in new ItemEnumerable()) { ... }
        /// </summary>
        [Obsolete("This class is no longer used, but works perfectly fine.\n"
            + "You could also use '(from i in Defs.items.Values where ItemUI.IncludeInList(i) select ItemUI.CopyItem(i))'")]
        public sealed class ItemEnumerable : IEnumerable<Item>
        {
            /// <summary>
            /// Gets the generic enumerator of this IEnumerable instance
            /// </summary>
            /// <returns>The generic enumerator of this IEnumerable instance</returns>
            IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
            {
                return new ItemEnumerator();
            }
            /// <summary>
            /// Gets the enumerator of this IEnumerable instance
            /// </summary>
            /// <returns>The enumerator of this IEnumerable instance</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ItemEnumerator();
            }
        }

        sealed class ItemEnumerator : IEnumerator<Item>
        {
            int offset = 0, current = -1;

            internal int ID
            {
                get
                {
                    return offset + current;
                }
            }

            internal ItemEnumerator(int startID = 0)
            {
                offset = startID;
                current = -1;
            }

            Item IEnumerator<Item>.Current
            {
                get
                {
                    Item ret = new Item();
                    ret.netDefaults(ID);
                    return ret;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    Item ret = new Item();
                    ret.netDefaults(ID);
                    return ret;
                }
            }

            public bool MoveNext()
            {
                do
                {
                    if (++current >= Defs.items.Count)
                        return false;
                } while (!ItemUI.Interface.IncludeInList(((IEnumerator<Item>)this).Current));

                return true;
            }

            public void Reset()
            {
                current = -1;
            }

            public void Dispose()
            {
                // nothing to dispose...
            }
        }

        /// <summary>
        /// The amount of categories
        /// </summary>
        public const int CATEGORY_LIST_LENGTH = 22;
        /// <summary>
        /// The highest value in the Categories enum
        /// </summary>
        public const int HIGHEST_CATEGORY_VALUE = 0x200000;

        /// <summary>
        /// The 20 Item containers which contain the currently displayed Items
        /// </summary>
        public static CheatItemContainer[] ItemContainers
        {
            get;
            private set;
        }
        /// <summary>
        /// The 22 category buttons
        /// </summary>
        public static ItemCategoryButton[] CategoryButtons
        {
            get;
            private set;
        }

        /// <summary>
        /// The current category
        /// </summary>
        public static Categories Category = Categories.None;

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

        static ItemUI()
        {
            ItemContainers = new CheatItemContainer[LIST_LENGTH];
            CategoryButtons = new ItemCategoryButton[CATEGORY_LIST_LENGTH];
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            Category = Categories.None;

            base.Init();

            AddControl(new TextBlock("Found items: " + objects.Count + ", Filter: " + Category)
            {
                Position = new Vector2(170f, Main.screenHeight - 460f),

                OnUpdate = (c) => ((TextBlock)c).Text = "Found items: " + objects.Count + ", Filter: " + Category
            });

            FilterOptions[0].OnChecked += (ca) =>
            {
                if (Category == Categories.All)
                    Category = Categories.None;
                else if (Category == Categories.None)
                    Category = Categories.All;

                ResetObjectList();
            };
            FilterOptions[1].OnChecked += (ca) =>
            {
                if (Category == Categories.All)
                    Category = Categories.None;
                else if (Category == Categories.None)
                    Category = Categories.All;

                ResetObjectList();
            };
            FilterOptions[2].OnChecked += (ca) =>
            {
                if (Category == Categories.All)
                    Category = Categories.None;
                else if (Category == Categories.None)
                    Category = Categories.All;

                ResetObjectList();
            };

            int col = 0, row = 0, index = 0;
            for (int i = 1; i <= HIGHEST_CATEGORY_VALUE; i *= 2, col++, index++)
            {
                if (col >= 3)
                {
                    row++;
                    col = 0;
                }

                AddControl(CategoryButtons[index] = new ItemCategoryButton((Categories)i)
                {
                    Position = new Vector2(480f + Main.inventoryBackTexture.Width * col,
                        Main.screenHeight - 440f + Main.inventoryBackTexture.Height * row)
                });
                CategoryButtons[index].Position += Main.inventoryBackTexture.Size() / 2f
                    - ((Texture2D)CategoryButtons[index].Picture.Item).Size() / 2f;
            }
        }

        void _Reset()
        {
            objects.Clear();

            objects.AddRange(from Item i in Defs.items.Values where IncludeInList(i) select CopyItem(i));

            ResetContainers();
        }
        /// <summary>
        /// Clears the Item list and fills it, with the current filters
        /// </summary>
        /// <param name="thisThread">Wether to load it on the current thread or on a new one</param>
        public override void ResetObjectList(bool thisThread = false)
        {
            if (ResetThread != null && ResetThread.ThreadState == ThreadState.Running)
                ResetThread.Abort();

            if (thisThread)
                _Reset();
            else
                (ResetThread = new Thread(_Reset)).Start();
        }
        /// <summary>
        /// Resets the ItemContainer content
        /// </summary>
        public override void ResetContainers()
        {
            for (int i = Position; i < Position + 20; i++)
            {
                if (i >= objects.Count)
                    ItemContainers[i - Position].Item = new Item();
                else
                {
                    ItemContainers[i - Position].Item = CopyItem(objects[i]);
                    ItemContainers[i - Position].Item.stack = ItemContainers[i - Position].Item.maxStack;
                }
                ItemContainers[i - Position].CanFocus = i < objects.Count;
            }
        }

        /// <summary>
        /// Creates the object container list
        /// </summary>
        protected override void CreateContainers()
        {
            if (ItemContainers == null)
                ItemContainers = new CheatItemContainer[LIST_LENGTH];

            int row = 0, col = 0;
            for (int i = 0; i < LIST_LENGTH; i++, col++)
            {
                if (ItemContainers[i] == null)
                    ItemContainers[i] = new CheatItemContainer();

                if (col >= 4)
                {
                    row++;
                    col = 0;
                }

                ItemContainers[i].Position = new Vector2(160f + row * Main.inventoryBackTexture.Width, (Main.screenHeight - 350f) + col * Main.inventoryBackTexture.Height);

                AddControl(ItemContainers[i]);
            }
        }
        /// <summary>
        /// Disposes the object container list
        /// </summary>
        protected override void RemoveContainers()
        {
            for (int i = 0; i < LIST_LENGTH; i++)
                RemoveControl(ItemContainers[i]);

            ItemContainers = null;
        }

        /// <summary>
        /// Checks wether an Item is in the current category or not.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <returns>true if the Item is in the current category, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsInCategory(Item i)
        {
            if (FilterOptions[0].IsChecked)
                return IsInCategoryAND(i, Category);
            if (FilterOptions[1].IsChecked)
                return IsInCategoryOR(i, Category);
            if (FilterOptions[2].IsChecked)
                return IsInCategoryXOR(i, Category);

            throw new YoureAHackerException(new Exception("There should be at least one of the filter RadioButtons checked..."));
        }

        /// <summary>
        /// Checks wether an Item is in a certain category or not, using AND filtering.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="cat">The Category to compare the Item with</param>
        /// <returns>true if the Item is in the category, false otherwise.</returns>
        public static bool IsInCategoryAND(Item i, Categories cat)
        {
            if (i.IsBlank())
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = true;

            if ((cat & Categories.Other) != 0)
                ret &= IsOther(i);


            if ((cat & Categories.Melee) != 0)
                ret &= i.melee;
            if ((cat & Categories.Ranged) != 0)
                ret &= i.ranged;
            if ((cat & Categories.Magic) != 0)
                ret &= i.magic;

            if ((cat & Categories.Helmet) != 0)
                ret &= i.headSlot >= 0;
            if ((cat & Categories.Torso) != 0)
                ret &= i.bodySlot >= 0;
            if ((cat & Categories.Leggings) != 0)
                ret &= i.legSlot >= 0;

            if ((cat & Categories.Vanity) != 0)
                ret &= i.vanity;
            if ((cat & Categories.Accessory) != 0)
                ret &= i.accessory;

            if ((cat & Categories.Pickaxe) != 0)
                ret &= i.pick > 0;
            if ((cat & Categories.Axe) != 0)
                ret &= i.axe > 0;
            if ((cat & Categories.Hammer) != 0)
                ret &= i.hammer > 0;

            if ((cat & Categories.Ammunition) != 0)
                ret &= i.ammo > 0;
            if ((cat & Categories.Material) != 0)
                ret &= i.material;
            if ((cat & Categories.Potion) != 0)
                ret &= i.healLife > 0 || i.healMana > 0 || i.buffType > 0 && i.buffTime > 0;

            if ((cat & Categories.Buff) != 0)
                ret &= i.buffType > 0;

            if ((cat & Categories.Tile) != 0)
                ret &= i.createTile > 0;
            if ((cat & Categories.Wall) != 0)
                ret &= i.createWall > 0;

            if ((cat & Categories.Paint) != 0)
                ret &= i.paint > 0;
            if ((cat & Categories.Dye) != 0)
                ret &= i.dye > 0;
            if ((cat & Categories.Pet) != 0)
                ret &= Main.vanityPet[i.buffType] || Main.lightPet[i.buffType];
            if ((cat & Categories.Summon) != 0)
                ret &= i.summon;

            return ret;
        }
        /// <summary>
        /// Checks wether an Item is in a certain category or not, using OR filtering.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="cat">The Category to compare the Item with</param>
        /// <returns>true if the Item is in the category, false otherwise.</returns>
        public static bool IsInCategoryOR(Item i, Categories cat)
        {
            if (i.IsBlank() || cat == Categories.None)
                return false;
            if (cat == Categories.All)
                return true;

            bool ret = false;

            if ((cat & Categories.Other) != 0)
                ret |= IsOther(i);

            if ((cat & Categories.Melee) != 0)
                ret |= i.melee;
            if ((cat & Categories.Ranged) != 0)
                ret |= i.ranged;
            if ((cat & Categories.Magic) != 0)
                ret |= i.magic;

            if ((cat & Categories.Helmet) != 0)
                ret |= i.headSlot >= 0;
            if ((cat & Categories.Torso) != 0)
                ret |= i.bodySlot >= 0;
            if ((cat & Categories.Leggings) != 0)
                ret |= i.legSlot >= 0;

            if ((cat & Categories.Vanity) != 0)
                ret |= i.vanity;
            if ((cat & Categories.Accessory) != 0)
                ret |= i.accessory;

            if ((cat & Categories.Pickaxe) != 0)
                ret |= i.pick > 0;
            if ((cat & Categories.Axe) != 0)
                ret |= i.axe > 0;
            if ((cat & Categories.Hammer) != 0)
                ret |= i.hammer > 0;

            if ((cat & Categories.Ammunition) != 0)
                ret |= i.ammo > 0 && !i.notAmmo;
            if ((cat & Categories.Material) != 0)
                ret |= i.material && !i.notMaterial;
            if ((cat & Categories.Potion) != 0)
                ret |= i.consumable;

            if ((cat & Categories.Buff) != 0)
                ret |= i.buffType > 0;

            if ((cat & Categories.Tile) != 0)
                ret |= i.createTile > 0;
            if ((cat & Categories.Wall) != 0)
                ret |= i.createWall > 0;

            if ((cat & Categories.Paint) != 0)
                ret |= i.paint > 0;
            if ((cat & Categories.Dye) != 0)
                ret |= i.dye > 0;
            if ((cat & Categories.Pet) != 0)
                ret |= Main.vanityPet[i.buffType] || Main.lightPet[i.buffType];
            if ((cat & Categories.Summon) != 0)
                ret |= i.summon;

            return ret;
        }
        /// <summary>
        /// Checks wether an Item is in a certain category or not, using XOR filtering.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="cat">The Category to compare the Item with</param>
        /// <returns>true if the Item is in the category, false otherwise.</returns>
        public static bool IsInCategoryXOR(Item i, Categories cat)
        {
            if (i.IsBlank() || cat == Categories.All)
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = false;

            if ((cat & Categories.Other) != 0)
                ret ^= IsOther(i);

            if ((cat & Categories.Melee) != 0)
                ret ^= i.melee;
            if ((cat & Categories.Ranged) != 0)
                ret ^= i.ranged;
            if ((cat & Categories.Magic) != 0)
                ret ^= i.magic;

            if ((cat & Categories.Helmet) != 0)
                ret ^= i.headSlot >= 0;
            if ((cat & Categories.Torso) != 0)
                ret ^= i.bodySlot >= 0;
            if ((cat & Categories.Leggings) != 0)
                ret ^= i.legSlot >= 0;

            if ((cat & Categories.Vanity) != 0)
                ret ^= i.vanity;
            if ((cat & Categories.Accessory) != 0)
                ret ^= i.accessory;

            if ((cat & Categories.Pickaxe) != 0)
                ret ^= i.pick > 0;
            if ((cat & Categories.Axe) != 0)
                ret ^= i.axe > 0;
            if ((cat & Categories.Hammer) != 0)
                ret ^= i.hammer > 0;

            if ((cat & Categories.Ammunition) != 0)
                ret ^= i.ammo > 0 && !i.notAmmo;
            if ((cat & Categories.Material) != 0)
                ret ^= i.material && !i.notMaterial;
            if ((cat & Categories.Potion) != 0)
                ret ^= i.consumable;

            if ((cat & Categories.Buff) != 0)
                ret ^= i.buffType > 0;

            if ((cat & Categories.Tile) != 0)
                ret ^= i.createTile > 0;
            if ((cat & Categories.Wall) != 0)
                ret ^= i.createWall > 0;

            if ((cat & Categories.Paint) != 0)
                ret ^= i.paint > 0;
            if ((cat & Categories.Dye) != 0)
                ret ^= i.dye > 0;
            if ((cat & Categories.Pet) != 0)
                ret ^= Main.vanityPet[i.buffType] || Main.lightPet[i.buffType];
            if ((cat & Categories.Summon) != 0)
                ret ^= i.summon;

            return ret;
        }

        /// <summary>
        /// Gets wether an Item belongs to the Other category or not
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <returns>>ether the Item belongs to the Other category or not</returns>
        public static bool IsOther(Item i)
        {
            return !i.melee && !i.ranged && !i.magic && i.headSlot < 0 && i.bodySlot < 0 && i.legSlot < 0 && !i.vanity && !i.accessory &&
                    i.pick <= 0 && i.hammer <= 0 && i.axe <= 0 && i.ammo <= 0 && !i.material
                    && !(i.healLife > 0 || i.healMana > 0) && i.buffType <= 0 && i.createTile <= 0 && i.createWall <= 0 && i.paint < 0 && i.dye <= 0
                    && !Main.vanityPet[i.buffType] && !Main.lightPet[i.buffType] && !i.summon;
        }

        /// <summary>
        /// Checks wether an Item is a result of the given search string
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <returns>true if the Item matches the current search string, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsSearchResult(Item i)
        {
            if (SearchBox.Text.IsEmpty() || !ChangedSearchText)
                return true;

            return IsSearchResult(i, SearchBox.Text);
        }
        /// <summary>
        /// Checks wether an Item is a result of the given search string
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="search">The searched text</param>
        /// <returns>true if the Item matches the search string, false otherwise.</returns>
        public static bool IsSearchResult(Item i, string search)
        {
            if (search.IsEmpty())
                return true;
            if (i.type == 0)
                return false;

            return ExcludeSpecialChars(i.displayName).ToLower().Contains(search); // SDMG will also find 'S.D.M.G.'
        }

        /// <summary>
        /// Wether to include an Item in the item list or not
        /// </summary>
        /// <param name="i">An Item to check</param>
        /// <returns>true if the Item should be included, false otherwise.</returns>
        public bool IncludeInList(Item i)
        {
            if (i == null || i.IsBlank() || i.name.StartsWith("g:") /* item craft groups are actually items under the hood */ || i.name == "TAPI:Unloaded Item" /* also an item instance */)
                return false;

            bool ret = IsInCategory(i);

            if (FilterOptions[0].IsChecked)
                return ret && IsSearchResult(i);
            if (FilterOptions[1].IsChecked)
                return ret || IsSearchResult(i);
            if (FilterOptions[2].IsChecked)
                return ret ^  IsSearchResult(i);

            return false;
        }

        /// <summary>
        /// Creates a copy of the Item
        /// </summary>
        /// <param name="toCopy">The Item to copy</param>
        /// <returns>The copied Item</returns>
        public static Item CopyItem(Item toCopy)
        {
            Item ret = new Item();

            CopyItem(toCopy, ref ret);

            return ret;
        }
        /// <summary>
        /// Creates a copy of the Item
        /// </summary>
        /// <param name="toCopy">The Item to copy</param>
        /// <param name="copyTo">The Item to copy all data to</param>
        public static void CopyItem(Item toCopy, ref Item copyTo)
        {
            copyTo.netDefaults(toCopy.netID);
            copyTo.Prefix(toCopy.prefix.name);
            copyTo.stack = toCopy.stack;
        }
    }
}
