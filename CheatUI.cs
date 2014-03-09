﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;

namespace PoroCYon.ICM
{
    /// <summary>
    /// The base class of all cheat menus
    /// </summary>
    public abstract class CheatUI : CustomUI
    {
        /// <summary>
        /// The type of the cheat UI
        /// </summary>
        public InterfaceType Type = InterfaceType.None;

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

        /// <summary>
        /// Creates a new instace of the CheatUI class
        /// </summary>
        /// <param name="type">The type of the cheat menu</param>
        public CheatUI(InterfaceType type)
            : base()
        {
            Type = type;
        }

        /// <summary>
        /// Gets wether the interface is visible or not
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return base.IsVisible && MainUI.UIType == Type;
            }
        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            base.Init();

            if (LeftArrow == null)
                LeftArrow = Mod.ModInstance.textures["Sprites/Left.png"];
            if (RightArrow == null)
                RightArrow = Mod.ModInstance.textures["Sprites/Right.png"];
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
    /// <summary>
    /// The base class of all cheat menus, provides searching, filter options and an object list
    /// </summary>
    public abstract class CheatUI<T> : CheatUI
    {
        /// <summary>
        /// The length of the cheat object list
        /// </summary>
        public const int LIST_LENGTH = 20;
        /// <summary>
        /// The amount of filter options
        /// </summary>
        public const int FILTER_OPTIONS_LENGTH = 3;

        /// <summary>
        /// The 3 filter options RadioButtons
        /// </summary>
        public RadioButton[] FilterOptions;

        /// <summary>
        /// The button used to decrease Position
        /// </summary>
        public ImageButton LeftButton;
        /// <summary>
        /// The button used to increase Position
        /// </summary>
        public ImageButton RightButton;

        /// <summary>
        /// The thread where all objects are reset with the new filters
        /// </summary>
        protected Thread ResetThread = null;

        /// <summary>
        /// Wether the user changed the search text or not
        /// </summary>
        protected bool ChangedSearchText = false;

        /// <summary>
        /// The position of the cheat object list
        /// </summary>
        public int Position = 0;
        /// <summary>
        /// The TextBox instance used to search cheat objects
        /// </summary>
        public TextBox SearchBox = new TextBox();
        /// <summary>
        /// The current list (which match the search query)
        /// </summary>
        public ReadOnlyCollection<T> Objects
        {
            get
            {
                return objects.AsReadOnly();
            }
        }

        /// <summary>
        /// The current list (which match the search query), writable
        /// </summary>
        protected List<T> objects = new List<T>();

        /// <summary>
        /// Creates a new instace of the CheatUI class
        /// </summary>
        /// <param name="type">The type of the cheat menu</param>
        public CheatUI(InterfaceType type)
            : base(type)
        {
            Type = type;
        }

        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            objects.Clear();
        }

        /// <summary>
        /// Initializes the CustomUI
        /// </summary>
        public override void Init()
        {
            objects = new List<T>();

            FilterOptions = new RadioButton[FILTER_OPTIONS_LENGTH];

            Position = 0;

            base.Init();

            AddControl(LeftButton = new ImageButton(LeftArrow)
            {
                HasBackground = true,
                KeepFiring = true,

                Position = new Vector2(130f, Main.screenHeight - 208),

                OnClicked = (b) => Position = Math.Max(Position - 4, 0)
            });
            AddControl(RightButton = new ImageButton(RightArrow)
            {
                HasBackground = true,
                KeepFiring = true,

                Position = new Vector2(430f, Main.screenHeight - 208),

                OnClicked = (b) =>
                {
                    if (objects.Count > 20)
                        Position = Math.Min(Position + 4, objects.Count - 1);
                }
            });

            AddControl(SearchBox = new TextBox("Search " + typeof(T).Name.ToLower() + "...")
            {
                EnterMode = EnterMode.EnterOrShiftEnter,

                Position = new Vector2(170f, Main.screenHeight - 415f)
            });

            AddControl(FilterOptions[0] = new RadioButton("ICM:FilterType", true, "AND filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 470f),
                Tooltip = "The filter searches for items that have all of the selected remarks."
            });
            AddControl(FilterOptions[1] = new RadioButton("ICM:FilterType", false, "OR filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 420f),
                Tooltip = "The filter searches for items which have one or more of the selected remarks."
            });
            AddControl(FilterOptions[2] = new RadioButton("ICM:FilterType", false, "XOR filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 370f),
                Tooltip = "The filter searches for items which have none of the selected remarks."
            });
        }

        /// <summary>
        /// Updates the CustomUI
        /// </summary>
        public override void Update()
        {
            string oldText = SearchBox.Text;

            base.Update();

            if (oldText != SearchBox.Text && oldText.Length < SearchBox.Text.Length) // wait until 'Search T...' is deleted
            {
                ChangedSearchText = true;
                SearchTextChanged();
            }
        }

        /// <summary>
        /// Called when the text of the search text box is changed
        /// </summary>
        public virtual void SearchTextChanged()
        {

        }
    }
}
