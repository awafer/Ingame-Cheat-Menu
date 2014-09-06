using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using PoroCYon.MCT;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.MCT.UI.Interface.Controls.Primitives;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM World cheat menu
    /// </summary>
    public sealed class WorldUI : CheatUI
    {
        internal static bool
            Christmas = false,
            Halloween = false;

        /// <summary>
        /// The WorldUI singleton instance
        /// </summary>
        public static WorldUI Instance
        {
            get;
            internal set;
        }

        /// <summary>
        /// Creates a new instance of the WorldUI class
        /// </summary>
        public WorldUI()
            : base(InterfaceType.World)
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

            AddControl(new CheckBox(Main.hardMode, "Hardmode")
            {
                Position = new Vector2(200f, Main.screenHeight - 350f),

                OnUpdate = c => ((Checkable)c).IsChecked = Main.hardMode,

                OnChecked = ca => Main.hardMode = true,
                OnUnchecked = ca => Main.hardMode = false
            });

            AddControl(new CheckBox(Main.bloodMoon, "Blood moon")
            {
                Position = new Vector2(320f, Main.screenHeight - 350f),

                OnUpdate = c => ((Checkable)c).IsChecked = Main.bloodMoon,

                OnChecked = ca => World.StartBloodMoon(),
                OnUnchecked = ca => World.StopBloodMoon()
            });
            AddControl(new CheckBox(Main.eclipse, "Eclipse")
            {
                Position = new Vector2(320f, Main.screenHeight - 300f),

                OnUpdate = c => ((Checkable)c).IsChecked = Main.eclipse,

                OnChecked = ca => World.StartEclipse(),
                OnUnchecked = ca => World.StopEclipse()
            });

            AddControl(new CheckBox(Main.xMas, "Christmas")
            {
                Position = new Vector2(460f, Main.screenHeight - 350f),

                OnUpdate = (c) =>
                {
                    bool @checked = ((Checkable)c).IsChecked;

                    ((Checkable)c).IsChecked = Main.xMas || @checked;

                    World.ForceChristmas = @checked;
                }
            });
            AddControl(new CheckBox(Main.halloween, "Halloween")
            {
                Position = new Vector2(460f, Main.screenHeight - 300f),

                OnUpdate = (c) =>
                {
                    bool @checked = ((Checkable)c).IsChecked;

                    ((Checkable)c).IsChecked = Main.halloween || @checked;

                    World.ForceHalloween = @checked;
                }
            });

            AddControl(new CheckBox(Main.pumpkinMoon, "Pumpkin moon")
            {
                Position = new Vector2(620f, Main.screenHeight - 350f),

                OnUpdate = c => ((Checkable)c).IsChecked = Main.pumpkinMoon,

                OnChecked = ca => World.StartPumpkinMoon(),
                OnUnchecked = ca => World.StopPumpkinMoon()
            });
            AddControl(new CheckBox(Main.snowMoon, "Frost moon")
            {
                Position = new Vector2(620f, Main.screenHeight - 300f),

                OnUpdate = c => ((Checkable)c).IsChecked = Main.snowMoon,

                OnChecked = ca => World.StartFrostMoon(),
                OnUnchecked = ca => World.StopFrostMoon()
            });

            AddControl(new TextButton("Day")
            {
                Position = new Vector2(200f, Main.screenHeight - 250f),

                OnUpdate = c => ((TextButton)c).Text = Main.dayTime ? "Day" : "Night",
                OnClicked = b => ((TextButton)b).Text = (Main.dayTime = !Main.dayTime) ? "Day" : "Night"
            });

            AddControl(new PlusMinusButton((float)Main.time, 10f, "Time")
            {
                Position = new Vector2(270f, Main.screenHeight - 250f),

                Size = new Vector2(120f, Main.fontMouseText.MeasureString("Time").Y),

                ShowValue = false,

                OnUpdate = (c) =>
                {
                    ((PlusMinusButton)c).Value = (float)Main.time;
                    ((PlusMinusButton)c).Text = "Time: " + World.TimeAsTimeSpan;
                },
                OnValueChanged = (pmb, o, n) =>
                {
                    Main.time = n;

                    if (Main.time < 0d)
                    {
                        Main.dayTime = !Main.dayTime;
                        Main.time = Main.dayLength + Main.time; // time is negative here
                    }
                }
            });
            AddControl(new PlusMinusButton(Main.moonPhase, "Moon phase")
            {
                Position = new Vector2(460f, Main.screenHeight - 250f),

                OnUpdate = c => ((PlusMinusButton)c).Value = Main.moonPhase,
                OnValueChanged = (pmb, o, n) => Main.moonPhase = (int)n % 8
            });
            AddControl(new PlusMinusButton(Main.dayRate, 1f, "Time speed")
            {
                Position = new Vector2(650f, Main.screenHeight - 250f),

                OnUpdate = c => ((PlusMinusButton)c).Value = Main.dayRate,
                OnValueChanged = (pmb, o, n) =>
                {
                    if (n < 0f)
                        n = 0f;

                    Main.dayRate = (int)n;
                }
            });

            const string INVASIONS = "ICM:WorldUI.Invasions";

            AddControl(new RadioButton(INVASIONS, World.CurrentInvasion == InvasionType.None, "No invasion")
            {
                Position = new Vector2(200f, Main.screenHeight - 200f),

                OnChecked = ca => World.StopInvasions(),
                OnUpdate = (c) =>
                {
                    if (World.CurrentInvasion == InvasionType.None)
                        ((Checkable)c).IsChecked = true;
                }
            });
            AddControl(new RadioButton(INVASIONS, World.CurrentInvasion == InvasionType.GoblinArmy, "Goblin army")
            {
                Position = new Vector2(340f, Main.screenHeight - 200f),

                OnChecked = ca => World.StartInvasion(InvasionType.GoblinArmy),
                OnUpdate = (c) =>
                {
                    if (World.CurrentInvasion == InvasionType.GoblinArmy)
                        ((Checkable)c).IsChecked = true;
                }
            });
            AddControl(new RadioButton(INVASIONS, World.CurrentInvasion == InvasionType.FrostLegion, "Frost legion")
            {
                Position = new Vector2(480f, Main.screenHeight - 200f),

                OnChecked = ca => World.StartInvasion(InvasionType.FrostLegion),
                OnUpdate = (c) =>
                {
                    if (World.CurrentInvasion == InvasionType.FrostLegion)
                        ((Checkable)c).IsChecked = true;
                }
            });
            AddControl(new RadioButton(INVASIONS, World.CurrentInvasion == InvasionType.Pirates, "Pirates")
            {
                Position = new Vector2(620f, Main.screenHeight - 200f),

                OnChecked = ca => World.StartInvasion(InvasionType.Pirates),
                OnUpdate = (c) =>
                {
                    if (World.CurrentInvasion == InvasionType.Pirates)
                        ((Checkable)c).IsChecked = true;
                }
            });
        }
    }
}
