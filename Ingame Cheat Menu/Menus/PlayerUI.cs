﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.ModClasses;

namespace PoroCYon.ICM.Menus
{
    /// <summary>
    /// The ICM Player cheat menu
    /// </summary>
    public sealed class PlayerUI : CheatUI
    {
        /// <summary>
        /// The PlayerUI singleton instance
        /// </summary>
        public static PlayerUI Instance
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets wether Invincibility is turned on or off
        /// </summary>
        public static bool Invincibility
        {
            get
            {
                return MPlayer.Invincibility;
            }
            set
            {
                MPlayer.Invincibility = value;
            }
        }
        /// <summary>
        /// Gets or sets wether Noclip is turned on or off
        /// </summary>
        public static bool Noclip
        {
            get
            {
                return MPlayer.Noclip;
            }
            set
            {
                MPlayer.Noclip = value;
            }
        }

        /// <summary>
        /// Creates a new instance of the PlayerUI class
        /// </summary>
        public PlayerUI()
            : base(InterfaceType.Player)
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

            AddControl(new TextButton("Heal!")
            {
                Position = new Vector2(200f, Main.screenHeight - 350f),

                OnClicked = (b) =>
                {
                    int
                        healAmount = Main.localPlayer.statLifeMax2 - Main.localPlayer.statLife,
                        manaAmount = Main.localPlayer.statManaMax2 - Main.localPlayer.statMana;

                    Main.localPlayer.statLife += healAmount;
                    Main.localPlayer.statMana += manaAmount;

                    Main.localPlayer.HealEffect(healAmount);
                    Main.localPlayer.ManaEffect(manaAmount);
                }
            });

            AddControl(new TextButton("Difficulty: "
                + (Main.localPlayer.difficulty == 0 ? "Softcore" : Main.localPlayer.difficulty == 1 ? "Mediumcore" : "Hardcore"))
            {
                Position = new Vector2(200f, Main.screenHeight - 300f),

                OnClicked = (b) =>
                {
                    Main.localPlayer.difficulty++;
                    if (Main.localPlayer.difficulty > 2)
                        Main.localPlayer.difficulty = 0;
                    ((TextButton)b).Text = "Difficulty: "
                        + (Main.localPlayer.difficulty == 0 ? "Softcore" : Main.localPlayer.difficulty == 1 ? "Mediumcore" : "Hardcore");
                }
            });

            AddControl(new PlusMinusButton(Main.localPlayer.statLifeMax, "Max life")
            {
                Position = new Vector2(200f, Main.screenHeight - 250f),

                OnValueChanged = (pmb, o, n) =>
                {
                    Main.localPlayer.statLife = Main.localPlayer.statLifeMax += (int)(n - o);
                },
                OnUpdate = (c) => ((PlusMinusButton)c).Value = Main.localPlayer.statLifeMax
            });
            AddControl(new PlusMinusButton(Main.localPlayer.statManaMax, "Max mana")
            {
                Position = new Vector2(400f, Main.screenHeight - 250f),

                OnValueChanged = (pmb, o, n) =>
                {
                    Main.localPlayer.statMana = Main.localPlayer.statManaMax += (int)(n - o);
                },
                OnUpdate = (c) => ((PlusMinusButton)c).Value = Main.localPlayer.statManaMax
            });

            AddControl(new CheckBox(false, "Invincibility")
            {
                Position = new Vector2(200f, Main.screenHeight - 200f),

                OnChecked = (cb) =>
                {
                    Invincibility = true;
                },
                OnUnchecked = (cb) =>
                {
                    Invincibility = false;
                }
            });
            AddControl(new CheckBox(false, "Noclip")
            {
                Position = new Vector2(400f, Main.screenHeight - 200f),

                OnChecked = (cb) =>
                {
                    Noclip = true;
                },
                OnUnchecked = (cb) =>
                {
                    Noclip = false;
                }
            });
        }
    }
}
