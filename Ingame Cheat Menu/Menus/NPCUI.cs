using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.Xna.Framework;
using PoroCYon.Extensions;
using Terraria;
using TAPI;
using PoroCYon.MCT.Input;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM NPC cheat menu
    /// </summary>
    public sealed class NpcUI : CheatEntityUI<NPC>
    {
        /// <summary>
        /// All NPC categories. Enumeration is marked as Flags.
        /// </summary>
        [Flags]
        public enum Categories : int
        {
            /// <summary>
            /// Nothing
            /// </summary>
            None = 0,

            /// <summary>
            /// The NPC is friendly (eg. Bunny, Bird).
            /// </summary>
            Friendly = 1,
            /// <summary>
            /// The NPC is hostile (eg. Slime, Zombie).
            /// </summary>
            Hostile = 2,
            /// <summary>
            /// The NPC is a boss (eg. Eye of Ctulhu).
            /// </summary>
            Boss = 4,
            /// <summary>
            /// The NPC is a town NPC (eg. the Guide).
            /// </summary>
            Town = 8,

            /// <summary>
            /// All categories
            /// </summary>
            All = Friendly | Hostile | Boss | Town

            // Count = 4
        }

        /// <summary>
        /// The amount of categories
        /// </summary>
        public const int CATEGORY_LIST_LENGTH = 4;
        /// <summary>
        /// The highest value in the Categories enum
        /// </summary>
        public const int HIGHEST_CATEGORY_VALUE = 8;

        /// <summary>
        /// The 20 NPC containers which contain the currently displayed NPCs
        /// </summary>
        public static CheatNPCContainer[] NPCContainers
        {
            get;
            private set;
        }
        /// <summary>
        /// The 4 category buttons
        /// </summary>
        public static NPCCategoryButton[] CategoryButtons
        {
            get;
            private set;
        }

        /// <summary>
        /// The current category
        /// </summary>
        public static Categories Category = Categories.None;

        /// <summary>
        /// The NPCUI singleton instance
        /// </summary>
        public static NpcUI Instance
        {
            get;
            internal set;
        }

        /// <summary>
        /// Creates a new instance of the NPCUI class
        /// </summary>
        public NpcUI()
            : base(InterfaceType.NPC)
        {

        }

        static NpcUI()
        {
            NPCContainers   = new CheatNPCContainer[LIST_LENGTH];
            CategoryButtons = new NPCCategoryButton[CATEGORY_LIST_LENGTH];
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();

            AddControl(new TextBlock("Found NPCs: " + objects.Count + ", Filter: " + Category + ", Amount: " + CheatNPCContainer.Amount)
            {
                Position = new Vector2(170f, Main.screenHeight - 460f),

                OnUpdate = (c) => ((TextBlock)c).Text = "Found NPCs: " + objects.Count + ", Filter: " + Category + ", Amount: " + CheatNPCContainer.Amount
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

                AddControl(CategoryButtons[index] = new NPCCategoryButton((Categories)i)
                {
                    Position = new Vector2(480f + Main.inventoryBackTexture.Width * col,
                        Main.screenHeight - 440f + Main.inventoryBackTexture.Height * row)
                });
                CategoryButtons[index].Position += Main.inventoryBackTexture.Size() / 2f
                    - CategoryButtons[index].oneFrame.Size() * CategoryButtons[index].scale / 2f;
            }

            ModFilters = new ModFilter<NPC>[Mods.mods.Count];

            col = 0; row = 0;
            for (int i = 0; i < Mods.mods.Count; i++, col++)
			{
				if (!NPCDef.byType.Any(kvp => kvp.Value.modEntities.Any(mn => mn.modBase == Mods.mods[i].modBase)))
                    continue;

                if (col >= 2)
                {
                    row++;
                    col = 0;
                }

                AddControl(ModFilters[i] = new NpcFilter(Mods.mods[i].modBase)
                {
                    Position = new Vector2(640f + Main.inventoryBackTexture.Width * col,
                        Main.screenHeight - 440f + Main.inventoryBackTexture.Height * row)
                });
                ModFilters[i].Position += Main.inventoryBackTexture.Size() / 2f - ModFilters[i].Hitbox.Size() / 2f;
            }
        }

        /// <summary>
        /// Clears the NPC list and fills it, with the current filters
        /// </summary>
        /// <param name="thisThread">Wether to load it on the current thread or on a new one</param>
        public override void ResetObjectList(bool thisThread = false)
		{
			ThreadStart start = () =>
            {
                objects.Clear();

                objects.AddRange(from NPC n in NPCDef.byType.Values where IncludeInList(n) select CopyNPC(n));

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
        /// Resets the NPCContainers content
        /// </summary>
        public override void ResetContainers()
		{
			ReallocatingObjectList = true;

			for (int i = Position; i < Position + LIST_LENGTH; i++)
            {
                NPCContainers[i - Position].NPC = i >= objects.Count ? new NPC() : objects[i];
                NPCContainers[i - Position].CanFocus = i < objects.Count;
            }

			ReallocatingObjectList = false;
		}

        /// <summary>
        /// Creates the object container list
        /// </summary>
        protected override void CreateContainers()
        {
            if (NPCContainers == null)
                NPCContainers = new CheatNPCContainer[LIST_LENGTH];

            int row = 0, col = 0;
            for (int i = 0; i < LIST_LENGTH; i++, col++)
            {
                if (NPCContainers[i] == null)
                    NPCContainers[i] = new CheatNPCContainer();

                if (col >= 4)
                {
                    row++;
                    col = 0;
                }

                NPCContainers[i].Position = new Vector2(160f + row * Main.inventoryBackTexture.Width, (Main.screenHeight - 350f) + col * Main.inventoryBackTexture.Height);

                AddControl(NPCContainers[i]);
            }
        }
        /// <summary>
        /// Disposes the object container list
        /// </summary>
        protected override void RemoveContainers()
        {
            for (int i = 0; i < LIST_LENGTH; i++)
                RemoveControl(NPCContainers[i]);

            NPCContainers = null;
        }

        /// <summary>
        /// Checks wether an NPC is in the current category or not.
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <returns>true if the NPC is in the current category, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsInCategory(NPC n)
        {
            if (FilterOptions[0].IsChecked)
                return IsInCategoryAND(n, Category);
            if (FilterOptions[1].IsChecked)
                return IsInCategoryOR(n, Category);
            if (FilterOptions[2].IsChecked)
                return IsInCategoryXOR(n, Category);

            throw new YoureAHackerException(new Exception("There should be at least one of the filter RadioButtons checked..."));
        }

        /// <summary>
        /// Checks wether an NPC is in a certain category or not, using AND filtering.
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <param name="cat">The Category to compare the NPC with</param>
        /// <returns>true if the NPC is in the category, false otherwise.</returns>
        public static bool IsInCategoryAND(NPC n, Categories cat)
        {
            if (n.type == 0)
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = true;

            if ((cat & Categories.Boss) != 0)
                ret &= n.boss;
            if ((cat & Categories.Friendly) != 0)
                ret &= n.friendly;
            if ((cat & Categories.Hostile) != 0)
                ret &= !n.friendly;
            if ((cat & Categories.Town) != 0)
                ret &= n.townNPC;

            return ret;
        }
        /// <summary>
        /// Checks wether an NPC is in a certain category or not, using OR filtering.
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <param name="cat">The Category to compare the NPC with</param>
        /// <returns>true if the NPC is in the category, false otherwise.</returns>
        public static bool IsInCategoryOR(NPC n, Categories cat)
        {
            if (n.type == 0 || cat == Categories.None)
                return false;
            if (cat == Categories.All)
                return true;

            bool ret = false;

            if ((cat & Categories.Boss) != 0)
                ret |= n.boss;
            if ((cat & Categories.Friendly) != 0)
                ret |= n.friendly;
            if ((cat & Categories.Hostile) != 0)
                ret |= !n.friendly;
            if ((cat & Categories.Town) != 0)
                ret |= n.townNPC;

            return ret;
        }
        /// <summary>
        /// Checks wether an NPC is in a certain category or not, using XOR filtering.
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <param name="cat">The Category to compare the NPC with</param>
        /// <returns>true if the NPC is in the category, false otherwise.</returns>
        public static bool IsInCategoryXOR(NPC n, Categories cat)
        {
            if (n.type == 0 || cat == Categories.All)
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = false;

            if ((cat & Categories.Boss) != 0)
                ret ^= n.boss;
            if ((cat & Categories.Friendly) != 0)
                ret ^= n.friendly;
            if ((cat & Categories.Hostile) != 0)
                ret ^= !n.friendly;
            if ((cat & Categories.Town) != 0)
                ret ^= n.townNPC;

            return ret;
        }

        /// <summary>
        /// Checks wether an NPC is a result of the given search string
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <returns>true if the NPC matches the current search string, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsSearchResult(NPC n)
        {
            if (SearchBox.Text.IsEmpty())
                return true;

            return IsSearchResult(n, SearchBox.Text);
        }
        /// <summary>
        /// Checks wether an NPC is a result of the given search string
        /// </summary>
        /// <param name="n">The NPC to check</param>
        /// <param name="search">The searched text</param>
        /// <returns>true if the NPC matches the search string, false otherwise.</returns>
        public static bool IsSearchResult(NPC n, string search)
        {
            if (search.IsEmpty())
                return true;
            if (n.type == 0)
                return false;

            return ExcludeSpecialChars(n.displayName).ToLower().Contains(search);
        }

        /// <summary>
        /// Wether to include an NPC in the item list or not
        /// </summary>
        /// <param name="n">An NPC to check</param>
        /// <returns>true if the NPC should be included, false otherwise.</returns>
        public bool IncludeInList(NPC n)
        {
            bool ret = IsInCategory(n) && (CurrentModFilter == null ? true : CurrentModFilter.Filter(n));

            if (FilterOptions[0].IsChecked)
                return ret && IsSearchResult(n);
            if (FilterOptions[1].IsChecked)
                return ret || IsSearchResult(n);
            if (FilterOptions[2].IsChecked)
                return ret ^  IsSearchResult(n);

            return false;
        }

        /// <summary>
        /// Updates the CustomUI
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (GInput.Mouse.JustClickedLeft && CheatNPCContainer.Amount > 0 && CheatNPCContainer.CurrentNetID != 0 && !Main.localPlayer.mouseInterface)
            {
                Main.localPlayer.mouseInterface = true;

                for (int i = 0; i < CheatNPCContainer.Amount; i++)
                {
                    Vector2 p = GInput.Mouse.WorldPosition;

                    p += new Vector2(
                        (float)Math.Cos(Main.rand.Next(628) / 100d) * Main.rand.Next(CheatNPCContainer.Amount * 10),
                        (float)Math.Sin(Main.rand.Next(628) / 100d) * Main.rand.Next(CheatNPCContainer.Amount * 10));

                    Main.npc[NPC.NewNPC((int)p.X, (int)p.Y, 1)].netDefaults(CheatNPCContainer.CurrentNetID);
                }

                Main.PlaySound(12);

                CheatNPCContainer.CurrentNetID = CheatNPCContainer.Amount = 0;
            }
        }

        /// <summary>
        /// Creates a copy of the NPC
        /// </summary>
        /// <param name="toCopy">NPC Item to copy</param>
        /// <returns>The copied NPC</returns>
        public static NPC CopyNPC(NPC toCopy)
        {
            NPC ret = new NPC();

            CopyNPC(toCopy, ref ret);

            return ret;
        }
        /// <summary>
        /// Creates a copy of the NPC
        /// </summary>
        /// <param name="toCopy">The NPC to copy</param>
        /// <param name="copyTo">The NPC to copy all data to</param>
        public static void CopyNPC(NPC toCopy, ref NPC copyTo)
        {
            copyTo.netDefaults(toCopy.netID);
        }
    }
}
