using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Controls;

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

        internal static Item TooltipToDisplay = null;

        /// <summary>
        /// Draws the CustomUI
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control</param>
        public override void Draw(SpriteBatch sb)
        {
            TooltipToDisplay = null;

            base.Draw(sb);

            if (TooltipToDisplay != null)
                MctUI.MouseText(TooltipToDisplay);

            TooltipToDisplay = null;
        }

        /// <summary>
        /// Creates a new instace of the CheatUI class
        /// </summary>
        /// <param name="type">The type of the cheat menu</param>
        protected CheatUI(InterfaceType type)
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

            if (LeftArrow  == null)
                LeftArrow  = IcmMod.Instance.textures["Ingame Cheat Menu/Sprites/Left" ];
            if (RightArrow == null)
                RightArrow = IcmMod.Instance.textures["Ingame Cheat Menu/Sprites/Right"];
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
    /// <typeparam name="T">The object list element type.</typeparam>
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

        int oldCount;

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
        /// The ScrollBar which represents the Position
        /// </summary>
        public ScrollBar ScrollBar;

        /// <summary>
        /// The thread where all objects are reset with the new filters
        /// </summary>
        protected Thread ResetThread = null;

        /// <summary>
        /// Wether the user changed the search text or not
        /// </summary>
        protected bool ChangedSearchText = false;

		bool preventSO = false;
		/// <summary>
		/// Gets or sets whether the object list is being reallocated (on a different <see cref="Thread" />).
		/// </summary>
		protected static bool ReallocatingObjectList;

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
        protected CheatUI(InterfaceType type)
            : base(type)
        {
            Type = type;
        }

        /// <summary>
        /// When the UI is opened
        /// </summary>
        public override void Open()
        {
            CreateContainers();
            ResetObjectList(true);
        }
        /// <summary>
        /// When the UI is closed
        /// </summary>
        public override void Close()
        {
            objects.Clear();

            RemoveContainers();
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
                KeepFiring    = true,

                Position = new Vector2(130f, Main.screenHeight - 145f),

                OnClicked = (b) =>
                {
                    Position = Math.Max(Position - 4, 0);

                    ScrollBar.Value = Position / 4f;
                }
            });
            AddControl(RightButton = new ImageButton(RightArrow)
            {
                HasBackground = true,
                KeepFiring    = true,

                Position = new Vector2(430f, Main.screenHeight - 150f),

                OnClicked = (b) =>
                {
                    if (objects.Count - Position > 20)
                        Position = Math.Min(Position + 4, objects.Count - 1);

                    ScrollBar.Value = Position / 4f;
                }
            });

            AddControl(ScrollBar = new ScrollBar(0f, objects.Count / 4f - 5f)
            {
                Position = new Vector2(160f, Main.screenHeight - 145f),
                Size = new Vector2(Main.inventoryBackTexture.Width * 5f, 16f),
                MouseScrollStep = 0.5f,

                OnValueChanged = (sb, ov, nv) =>
                {
                    if (preventSO || ov == nv)
                        return;

					if (ReallocatingObjectList)
					{
						preventSO = true;

						sb.Value = ov;

						preventSO = false;

						return;
					}

                    Position = (int)(nv * 4f);
                    ResetContainers();

                    Main.PlaySound(12);
                }
            });

            AddControl(SearchBox = new TextBox("Search " + typeof(T).Name.ToLower() + "...")
            {
                EnterMode = EnterMode.EnterOrShiftEnter,

                Position = new Vector2(170f, Main.screenHeight - 415f)
            });

            AddControl(FilterOptions[0] = new RadioButton("ICM:FilterType-" + GetType().FullName, true, "AND filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 470f),
                Tooltip = "The filter searches for items that have all of the selected remarks."
            });
            AddControl(FilterOptions[1] = new RadioButton("ICM:FilterType-" + GetType().FullName, false, "OR filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 420f),
                Tooltip = "The filter searches for items which have one or more of the selected remarks."
            });
            AddControl(FilterOptions[2] = new RadioButton("ICM:FilterType-" + GetType().FullName, false, "XOR filtering")
            {
                Position = new Vector2(20f, Main.screenHeight - 370f),
                Tooltip = "The filter searches for items which have none of the selected remarks."
            });

             LeftButton.OnClicked += b => ResetContainers();
            RightButton.OnClicked += b => ResetContainers();
        }

        /// <summary>
        /// Updates the CustomUI
        /// </summary>
        public override void Update()
        {
            if (oldCount != objects.Count)
            {
                float max = objects.Count / 4f - 5f;

                if (max <= ScrollBar.MinValue || max <= 5f)
                    ScrollBar.Enabled = false;
                else
                {
                    ScrollBar.Enabled = true;

                    if (max < ScrollBar.Value)
                        ScrollBar.Value = max;
                    ScrollBar.MaxValue = max;
                }

                oldCount = objects.Count;
            }

            string oldText = SearchBox.Text;

            base.Update();

            if (oldText != SearchBox.Text &&
                ((oldText.Length < SearchBox.Text.Length && !ChangedSearchText) || ChangedSearchText)) // wait until 'Search {T}...' is deleted
            {
                ChangedSearchText = true;
                SearchTextChanged();
            }
        }

        /// <summary>
        /// Clears the object list and fills it, with the current filters
        /// </summary>
        /// <param name="thisThread">Wether to load it on the current thread or on a new one</param>
        public virtual void ResetObjectList(bool thisThread = false)
        {

        }
        /// <summary>
        /// Resets the object containers content
        /// </summary>
        public virtual void ResetContainers()
        {

        }

        /// <summary>
        /// Creates the object container list
        /// </summary>
        protected virtual void CreateContainers() { }
        /// <summary>
        /// Disposes the object container list
        /// </summary>
        protected virtual void RemoveContainers() { }

        /// <summary>
        /// Called when the text of the search text box is changed
        /// </summary>
        public virtual void SearchTextChanged()
        {
            ResetObjectList();

            Position = 0;
        }
    }
    /// <summary>
    /// The base class of all cheat menus, provides searching, filter options, an object list and mod filtering options.
    /// </summary>
    /// <typeparam name="TCodableEntity">The object list element type.</typeparam>
    public abstract class CheatEntityUI<TCodableEntity> : CheatUI<TCodableEntity>
        where TCodableEntity : CodableEntity
    {
        int currentMFilter = -1;

        /// <summary>
        /// Gets all mod filters.
        /// </summary>
        public ModFilter<TCodableEntity>[] ModFilters;
        /// <summary>
        /// Gets the current mod filter.
        /// </summary>
        public ModFilter<TCodableEntity> CurrentModFilter
        {
            get
            {
                if (currentMFilter == -1)
                    return null;

                return ModFilters[currentMFilter];
            }
            set
            {
                currentMFilter = value == null ? -1 : Array.IndexOf(ModFilters, value);
            }
        }

        /// <summary>
        /// Creates a new instance of the CheatEntityUI class.
        /// </summary>
        /// <param name="type">The interface type of the CheatUI.</param>
        protected CheatEntityUI(InterfaceType type)
            : base(type)
        {

        }
    }
}
