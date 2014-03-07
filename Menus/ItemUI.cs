using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using TAPI.SDK.Content;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// All Item categories. Enumeration is marked as Flags.
    /// </summary>
    [Flags]
    public enum Category : int
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
        /// Consumables (stack decreases when used)
        /// </summary>
        Consumable = 0x4000,

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
        Others = 0x200000,

        /// <summary>
        /// All categories
        /// </summary>
        All = Weapon | Armour | Vanity | Accessory | Ammunition | Material | Consumable | Tile | Wall | Dye | Paint | Pet | Summon | Tools | Others

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
            } while (!ItemUI.IncludeInList(((IEnumerator<Item>)this).Current));

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
    /// The ICM Item cheat UI
    /// </summary>
    public sealed class ItemUI : CheatUI
    {
        /// <summary>
        /// The amount of items displayed (ItemContainers.Length)
        /// </summary>
        public const int ITEM_LIST_LENGTH = 20;
        public const int CATEGORY_LIST_LENGTH = 22;

        /// <summary>
        /// The TextBox instance used to search Items
        /// </summary>
        public static TextBox SearchBox
        {
            get;
            private set;
        }
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
        /// The current list of Items (which match the search query)
        /// </summary>
        public static List<Item> Items
        {
            get;
            private set;
        }

        /// <summary>
        /// The left arrow Texture2D
        /// </summary>
        public static Texture2D LeftArrow
        {
            get;
            private set;
        }
        /// <summary>
        /// The right arrow Texture2D
        /// </summary>
        public static Texture2D RightArrow
        {
            get;
            private set;
        }

        internal static Item TooltipToDisplay = null;

        static bool changedSearchText = false;

        /// <summary>
        /// The position of the Item list
        /// </summary>
        public static int Position = 0;
        /// <summary>
        /// The current category
        /// </summary>
        public static Category Category = Category.None;

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
            SearchBox = new TextBox();
            ItemContainers = new CheatItemContainer[ITEM_LIST_LENGTH];
            Items = new List<Item>();
            CategoryButtons = new ItemCategoryButton[CATEGORY_LIST_LENGTH];
        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public override void Open()
        {
            CreateContainers();
            ResetItemList();
        }
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            Items.Clear();
            RemoveContainers();
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            Position = 0;
            Category = Category.None;

            LeftArrow = Mod.ModInstance.textures["Sprites/Left.png"];
            RightArrow = Mod.ModInstance.textures["Sprites/Right.png"];

            base.Init();

            CreateContainers();
            ResetItemList();

            AddControl(new ImageButton(LeftArrow) 
            {
                HasBackground = true,
                KeepFiring = true,

                Position = new Vector2(120f, Main.screenHeight - 208),

                OnClicked = (b) =>
                {
                    Position = Math.Max(Position - 4, 0);

                    ResetContainers();
                }
            });
            AddControl(new ImageButton(RightArrow)
            {
                HasBackground = true,
                KeepFiring = true,

                Position = new Vector2(420f, Main.screenHeight - 208),

                OnClicked = (b) =>
                {
                    if (Items.Count > 20)
                        Position = Math.Min(Position + 4, Items.Count - 1);

                    ResetContainers();
                }
            });

            AddControl(SearchBox = new TextBox("Search item...")
            {
                Position = new Vector2(170f, Main.screenHeight - 420f)
            });

            int col = 0, row = 0, index = 0;
            for (int i = 1; i <= 0x200000; i *= 2, col++, index++)
            {
                if (col >= 3)
                {
                    row++;
                    col = 0;
                }

                AddControl(CategoryButtons[index] = new ItemCategoryButton((Category)i)
                {
                    Position = new Vector2(480f + Main.inventoryBackTexture.Width * col,
                        Main.screenHeight - 440f + Main.inventoryBackTexture.Height * row)
                });
                CategoryButtons[index].Position += Main.inventoryBackTexture.Size() / 2f
                    - ((Texture2D)CategoryButtons[index].Picture).Size() / 2f;
            }
        }

        /// <summary>
        /// Clears the Item list and fills it, with the current filters
        /// </summary>
        public void ResetItemList()
        {
            Items.Clear();
            Items.AddRange(from i in Defs.items.Values where IncludeInList(i) select CopyItem(i));

            ResetContainers();
        }
        /// <summary>
        /// Resets the ItemContainer content
        /// </summary>
        public void ResetContainers()
        {
            for (int i = Position; i < Position + 20; i++)
            {
                if (i >= Items.Count)
                    ItemContainers[i - Position].Item = new Item();
                else
                {
                    ItemContainers[i - Position].Item = CopyItem(Items[i]);
                    ItemContainers[i - Position].Item.stack = ItemContainers[i - Position].Item.maxStack;
                }
            }
        }

        void CreateContainers()
        {
            if (ItemContainers == null)
                ItemContainers = new CheatItemContainer[ITEM_LIST_LENGTH];

            int row = 0, col = 0;
            for (int i = 0; i < ITEM_LIST_LENGTH; i++, col++)
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
        void RemoveContainers()
        {
            for (int i = 0; i < ITEM_LIST_LENGTH; i++)
                RemoveControl(ItemContainers[i]);

            ItemContainers = null;
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
            if (i.type == 0)
                return false;
            if (cat == Category.None)
                return true;

            bool ret = true;

            if ((cat & Category.Melee) != 0)
                ret &= i.melee;
            if ((cat & Category.Ranged) != 0)
                ret &= i.ranged;
            if ((cat & Category.Magic) != 0)
                ret &= i.magic;

            if ((cat & Category.Helmet) != 0)
                ret &= i.headSlot >= 0;
            if ((cat & Category.Torso) != 0)
                ret &= i.bodySlot >= 0;
            if ((cat & Category.Leggings) != 0)
                ret &= i.legSlot >= 0;

            if ((cat & Category.Vanity) != 0)
                ret &= i.vanity;
            if ((cat & Category.Accessory) != 0)
                ret &= i.accessory;

            if ((cat & Category.Pickaxe) != 0)
                ret &= i.pick > 0;
            if ((cat & Category.Axe) != 0)
                ret &= i.axe > 0;
            if ((cat & Category.Hammer) != 0)
                ret &= i.hammer > 0;

            if ((cat & Category.Ammunition) != 0)
                ret &= i.ammo > 0 && !i.notAmmo;
            if ((cat & Category.Material) != 0)
                ret &= i.material && !i.notMaterial;
            if ((cat & Category.Consumable) != 0)
                ret &= i.consumable;

            if ((cat & Category.Buff) != 0)
                ret &= i.buffType > 0;

            if ((cat & Category.Tile) != 0)
                ret &= i.createTile > 0;
            if ((cat & Category.Wall) != 0)
                ret &= i.createWall > 0;

            if ((cat & Category.Paint) != 0)
                ret &= i.paint > 0;
            if ((cat & Category.Dye) != 0)
                ret &= i.dye > 0;
            if ((cat & Category.Pet) != 0)
                ret &= Main.vanityPet[i.buffType] || Main.lightPet[i.buffType];
            if ((cat & Category.Summon) != 0)
                ret &= i.summon;

            if ((cat & Category.Others) != 0)
                return !i.melee && !i.ranged && !i.magic && i.headSlot < 0 && i.bodySlot < 0 && i.legSlot < 0 && !i.vanity && !i.accessory &&
                    i.pick <= 0 && i.hammer <= 0 && i.axe <= 0 && (i.ammo <= 0 || i.notAmmo) && (!i.material || i.notMaterial) && !i.consumable
                    && i.buffType <= 0 && i.createTile <= 0 && i.createWall <= 0 && i.paint < 0 && i.dye <= 0 && !Main.vanityPet[i.buffType]
                    && !Main.lightPet[i.buffType] && !i.summon;

            return ret;
        }
        /// <summary>
        /// Checks wether an Item is in a category or not.
        /// </summary>
        /// <param name="i">The Item to check</param>
        /// <param name="cat">The Category to compare the Item with</param>
        /// <returns>true if the Item is in the category, false otherwise.</returns>
        public static bool IsInCategoryOR(Item i, Category cat)
        {
            if (i.type == 0)
                return false;
            if (cat == Category.All)
                return true;
            if (cat == Category.None)
                return false;

            bool ret = false;

            if ((cat & Category.Melee) != 0)
                ret |= i.melee;
            if ((cat & Category.Ranged) != 0)
                ret |= i.ranged;
            if ((cat & Category.Magic) != 0)
                ret |= i.magic;

            if ((cat & Category.Helmet) != 0)
                ret |= i.headSlot >= 0;
            if ((cat & Category.Torso) != 0)
                ret |= i.bodySlot >= 0;
            if ((cat & Category.Leggings) != 0)
                ret |= i.legSlot >= 0;

            if ((cat & Category.Vanity) != 0)
                ret |= i.vanity;
            if ((cat & Category.Accessory) != 0)
                ret |= i.accessory;

            if ((cat & Category.Pickaxe) != 0)
                ret |= i.pick > 0;
            if ((cat & Category.Axe) != 0)
                ret |= i.axe > 0;
            if ((cat & Category.Hammer) != 0)
                ret |= i.hammer > 0;

            if ((cat & Category.Ammunition) != 0)
                ret |= i.ammo > 0 && !i.notAmmo;
            if ((cat & Category.Material) != 0)
                ret |= i.material && !i.notMaterial;
            if ((cat & Category.Consumable) != 0)
                ret |= i.consumable;

            if ((cat & Category.Buff) != 0)
                ret |= i.buffType > 0;

            if ((cat & Category.Tile) != 0)
                ret |= i.createTile > 0;
            if ((cat & Category.Wall) != 0)
                ret |= i.createWall > 0;

            if ((cat & Category.Paint) != 0)
                ret |= i.paint > 0;
            if ((cat & Category.Dye) != 0)
                ret |= i.dye > 0;
            if ((cat & Category.Pet) != 0)
                ret |= Main.vanityPet[i.buffType] || Main.lightPet[i.buffType];
            if ((cat & Category.Summon) != 0)
                ret |= i.summon;

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
            if (SearchBox.Text.IsEmpty() || !changedSearchText)
                return true;

            return IsSearchResult(i, SearchBox.Text);
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
            if (i.type == 0)
                return false;

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

        /// <summary>
        /// Updates the Control
        /// </summary>
        public override void Update()
        {
            string oldText = SearchBox.Text;

            base.Update();

            if (oldText != SearchBox.Text)
            {
                changedSearchText = true;
                ResetItemList();
            }
        }
        /// <summary>
        /// Draws the CustomUI
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            TooltipToDisplay = null;

            base.Draw(sb);

            if (TooltipToDisplay != null)
                SdkUI.MouseText(TooltipToDisplay);

            TooltipToDisplay = null;
        }
    }
}
