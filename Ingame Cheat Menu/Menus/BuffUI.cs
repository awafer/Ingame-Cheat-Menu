﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions;
using PoroCYon.Extensions.Collections;
using Terraria;
using TAPI;
using PoroCYon.MCT.Content;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

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
            /// <summary>
            /// Nothing
            /// </summary>
            None = 0,

            /// <summary>
            /// The Buff grants positive effects
            /// </summary>
            Buff = 1,
            /// <summary>
            /// The Buff grants negative effects
            /// </summary>
            Debuff = 2,
            /// <summary>
            /// The Buff is a weapon imbue
            /// </summary>
            WeaponBuff = 4,

            /// <summary>
            /// Wether the Buff indicates that the player is having a pet that gives off light or not
            /// </summary>
            LightPet = 8,
            /// <summary>
            /// Wether the Buff indicates that the player is having a pet that is meant for vanity purposes or not
            /// </summary>
            VanityPet = 16,

            /// <summary>
            /// The buff is a light or vanity pet
            /// </summary>
            Pet = LightPet | VanityPet,

            /// <summary>
            /// Everything
            /// </summary>
            All = Buff | Debuff | WeaponBuff | Pet
        }

        /// <summary>
        /// The amount of categories
        /// </summary>
        public const int CATEGORY_LIST_LENGTH = 5;
        /// <summary>
        /// The highest value in the Categories enum
        /// </summary>
        public const int HIGHEST_CATEGORY_VALUE = 16;

        /// <summary>
        /// The 20 Buff containers which contain the currently displayed Buffs
        /// </summary>
        public static CheatBuffContainer[] BuffContainers
        {
            get;
            private set;
        }
        /// <summary>
        /// The 5 category buttons
        /// </summary>
        public static BuffCategoryButton[] CategoryButtons
        {
            get;
            private set;
        }

        /// <summary>
        /// Buttons used to show the minutes of the buff length
        /// </summary>
        public static PlusMinusButton Minutes;
        /// <summary>
        /// Buttons used to show the seconds of the buff length
        /// </summary>
        public static PlusMinusButton Seconds;

        /// <summary>
        /// The time of the buff to apply to the Player
        /// </summary>
        public static TimeSpan BuffTime = new TimeSpan(0, 1, 0);

        /// <summary>
        /// The current Category
        /// </summary>
        public static Categories Category = Categories.None;

        /// <summary>
        /// The BuffUI singleton instance
        /// </summary>
        public static BuffUI Instance
        {
            get;
            internal set;
        }

        /// <summary>
        /// Creates a new instance of the BuffUI class
        /// </summary>
        public BuffUI()
            : base(InterfaceType.Buff)
        {

        }

        static BuffUI()
        {
            BuffContainers  = new CheatBuffContainer[LIST_LENGTH];
            CategoryButtons = new BuffCategoryButton[CATEGORY_LIST_LENGTH];
        }

        /// <summary>
        /// Converts a TimeSpan instance to ticks (60/sec)
        /// </summary>
        /// <param name="span">The TimeSpan to convert</param>
        /// <returns>The TimeSpan as ticks</returns>
        public static int ToTicks(TimeSpan span)
        {
            return (int)(span.TotalSeconds * 60d);
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            Category = Categories.None;

            base.Init();

            AddControl(new TextBlock("Found buffs: " + objects.Count + ", Filter: " + Category)
            {
                Position = new Vector2(170f, Main.screenHeight - 460f),

                OnUpdate = (c) => ((TextBlock)c).Text = "Found buffs: " + objects.Count + ", Filter: " + Category
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

                AddControl(CategoryButtons[index] = new BuffCategoryButton((Categories)i)
                {
                    Position = new Vector2(480f + Main.inventoryBackTexture.Width * col,
                        Main.screenHeight - 440f + Main.inventoryBackTexture.Height * row)
                });
                CategoryButtons[index].Position += Main.inventoryBackTexture.Size() / 2f
                    - ((Texture2D)CategoryButtons[index].Picture.Item).Size() / 2f;
            }

            AddControl(Minutes = new PlusMinusButton(1f, "Minutes")
            {
                Position = new Vector2(480f, Main.screenHeight - 250f),

                Step = 1f,

                OnValueChanged = (pmb, ov, nv) =>
                {
                    int min = (int)(nv - ov);
                    int minOnly = min % 60;
                    int hr = (min - minOnly) / 60;

                    BuffTime += new TimeSpan(hr, minOnly, 0);

                    if (BuffTime.TotalSeconds < 1)
                        BuffTime = new TimeSpan(0, 0, 1);
                    if (BuffTime.TotalHours > 1)
                        BuffTime = new TimeSpan(1, 0, 0);
                }
            });
            AddControl(Seconds = new PlusMinusButton(0f, "Seconds")
            {
                Position = new Vector2(480f, Main.screenHeight - 250f + Minutes.Hitbox.Height),

                Step = 1f,

                OnValueChanged = (pmb, ov, nv) =>
                {
                    int sec = (int)(nv - ov);
                    int secOnly = sec % 60;
                    int min = (sec - secOnly) / 60;

                    BuffTime += new TimeSpan(0, min, secOnly);

                    if (BuffTime.TotalSeconds < 1)
                        BuffTime = new TimeSpan(0, 0, 1);
                    if (BuffTime.TotalHours > 1)
                        BuffTime = new TimeSpan(1, 0, 0);
                }
            });
        }

        /// <summary>
        /// Clears the Buff list and fills it, with the current filters
        /// </summary>
        /// <param name="thisThread">Wether to load it on the current thread or on a new one</param>
        public override void ResetObjectList(bool thisThread = false)
		{
			ThreadStart start = () =>
            {
                objects.Clear();

				objects.AddRange(BuffDef.byType.Keys.CastAll(i => new Buff(i)).Where(b => IncludeInList(b)));

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
			ReallocatingObjectList = true;

			for (int i = Position; i < Position + 20; i++)
            {
                BuffContainers[i - Position].Buff = i >= objects.Count ? new Buff() : CopyBuff(objects[i]);
                BuffContainers[i - Position].CanFocus = i < objects.Count;
			}

			ReallocatingObjectList = false;
		}

		/// <summary>
		/// Creates the object container list
		/// </summary>
		protected override void CreateContainers()
        {
            if (BuffContainers == null)
                BuffContainers = new CheatBuffContainer[LIST_LENGTH];

            int row = 0, col = 0;
            for (int i = 0; i < LIST_LENGTH; i++, col++)
            {
                if (BuffContainers[i] == null)
                    BuffContainers[i] = new CheatBuffContainer();

                if (col >= 4)
                {
                    row++;
                    col = 0;
                }

                BuffContainers[i].Position = new Vector2(160f + row * Main.inventoryBackTexture.Width, (Main.screenHeight - 350f) + col * Main.inventoryBackTexture.Height);

                AddControl(BuffContainers[i]);
            }
        }
        /// <summary>
        /// Disposes the object container list
        /// </summary>
        protected override void RemoveContainers()
        {
            for (int i = 0; i < LIST_LENGTH; i++)
                RemoveControl(BuffContainers[i]);

            BuffContainers = null;
        }

        /// <summary>
        /// Checks wether a Buff is in the current category or not.
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <returns>true if the Buff is in the current category, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsInCategory(Buff b)
        {
            if (FilterOptions[0].IsChecked)
                return IsInCategoryAND(b, Category);
            if (FilterOptions[1].IsChecked)
                return IsInCategoryOR(b, Category);
            if (FilterOptions[2].IsChecked)
                return IsInCategoryXOR(b, Category);

            throw new YoureAHackerException(new Exception("There should be at least one of the filter RadioButtons checked..."));
        }

        /// <summary>
        /// Checks wether a Buff is in a certain category or not, using AND filtering.
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <param name="cat">The Category to compare the Buff with</param>
        /// <returns>true if the Buff is in the category, false otherwise.</returns>
        public static bool IsInCategoryAND(Buff b, Categories cat)
        {
            if (b.ID == 0)
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = true;
            
            if ((cat & Categories.Buff) != 0)
                ret &= !Main.debuff[b.ID];
            if ((cat & Categories.Debuff) != 0)
                ret &= Main.debuff[b.ID];
            if ((cat & Categories.LightPet) != 0)
                ret &= b.LightPet;
            if ((cat & Categories.VanityPet) != 0)
                ret &= b.VanityPet;
            if ((cat & Categories.WeaponBuff) != 0)
                ret &= (b.Type & BuffType.WeaponBuff) != 0;

            return ret;
        }
        /// <summary>
        /// Checks wether a Buff is in a certain category or not, using OR filtering.
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <param name="cat">The Category to compare the Buff with</param>
        /// <returns>true if the Buff is in the category, false otherwise.</returns>
        public static bool IsInCategoryOR(Buff b, Categories cat)
        {
            if (b.ID == 0 || cat == Categories.None)
                return false;
            if (cat == Categories.All)
                return true;

            bool ret = false;

            if ((cat & Categories.Buff) != 0)
                ret |= !Main.debuff[b.ID];
            if ((cat & Categories.Debuff) != 0)
                ret |= Main.debuff[b.ID];
            if ((cat & Categories.LightPet) != 0)
                ret |= b.LightPet;
            if ((cat & Categories.VanityPet) != 0)
                ret |= b.VanityPet;
            if ((cat & Categories.WeaponBuff) != 0)
                ret |= (b.Type & BuffType.WeaponBuff) != 0;

            return ret;
        }
        /// <summary>
        /// Checks wether a Buff is in a certain category or not, using XOR filtering.
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <param name="cat">The Category to compare the Buff with</param>
        /// <returns>true if the Buff is in the category, false otherwise.</returns>
        public static bool IsInCategoryXOR(Buff b, Categories cat)
        {
            if (b.ID == 0 || cat == Categories.All)
                return false;
            if (cat == Categories.None)
                return true;

            bool ret = false;
            
            if ((cat & Categories.Buff) != 0)
                ret ^= !Main.debuff[b.ID];
            if ((cat & Categories.Debuff) != 0)
                ret ^= Main.debuff[b.ID];
            if ((cat & Categories.LightPet) != 0)
                ret ^= b.LightPet;
            if ((cat & Categories.VanityPet) != 0)
                ret ^= b.VanityPet;
            if ((cat & Categories.WeaponBuff) != 0)
                ret ^= (b.Type & BuffType.WeaponBuff) != 0;

            return ret;
        }

        /// <summary>
        /// Checks wether a Buff is a result of the given search string
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <returns>true if the Buff matches the current search string, false otherwise.</returns>
        [TargetedPatchingOptOut(MainUI.TPOOReason)]
        public bool IsSearchResult(Buff b)
        {
            if (SearchBox.Text.IsEmpty())
                return true;

            return IsSearchResult(b, SearchBox.Text);
        }
        /// <summary>
        /// Checks wether a Buff is a result of the given search string
        /// </summary>
        /// <param name="b">The Buff to check</param>
        /// <param name="search">The searched text</param>
        /// <returns>true if the Buff matches the search string, false otherwise.</returns>
        public static bool IsSearchResult(Buff b, string search)
        {
            if (search.IsEmpty())
                return true;
            if (b.ID == 0)
                return false;

            return ExcludeSpecialChars(b.Name).ToLower().Contains(search);
        }

        /// <summary>
        /// Wether to include a Buff in the item list or not
        /// </summary>
        /// <param name="b">A Buff to check</param>
        /// <returns>true if the Buff should be included, false otherwise.</returns>
        public bool IncludeInList(Buff b)
        {
            bool ret = IsInCategory(b);

            if (FilterOptions[0].IsChecked)
                return ret && IsSearchResult(b);
            if (FilterOptions[1].IsChecked)
                return ret || IsSearchResult(b);
            if (FilterOptions[2].IsChecked)
                return ret ^ IsSearchResult(b);

            return false;
        }

        /// <summary>
        /// Creates a copy of the Buff
        /// </summary>
        /// <param name="toCopy">The Buff to copy</param>
        /// <returns>The copied Buff</returns>
        public static Buff CopyBuff(Buff toCopy)
        {
            Buff ret = new Buff();

            CopyBuff(toCopy, ref ret);

            return ret;
        }
        /// <summary>
        /// Creates a copy of the Buff
        /// </summary>
        /// <param name="toCopy">The Buff to copy</param>
        /// <param name="copyTo">The Buff to copy all data to</param>
        public static void CopyBuff(Buff toCopy, ref Buff copyTo)
        {
            copyTo = new Buff(toCopy.ID);
        }

        /// <summary>
        /// Updates the CustomUI
        /// </summary>
        public override void Update()
        {
            float oldMin = Minutes.Value;
            float oldSec = Seconds.Value;

            base.Update();

            if (oldMin != Minutes.Value)
                BuffTime.Add(new TimeSpan(0, (int)(Minutes.Value - oldMin), 0));
            if (oldSec != Seconds.Value)
                BuffTime.Add(new TimeSpan(0, 0, (int)(Seconds.Value - oldSec)));
        }
    }
}
