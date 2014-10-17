using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions;
using Terraria;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.ICM.Menus;
using PoroCYon.ICM.Pages;

namespace PoroCYon.ICM
{
    /// <summary>
    /// The mod entry point
    /// </summary>
    public sealed class IcmMod : ModBase
    {
        static int editPlayerOffset, editWorldOffset;

        /// <summary>
        /// The path to the ICM_Data.sav file
        /// </summary>
        public readonly static string ICMDataFile = Main.SavePath + "\\ICM_Data.sav";

        /// <summary>
        /// The Mod singleton instance
        /// </summary>
        public static IcmMod Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the Mod class. Called through reflection.
        /// </summary>
        public IcmMod()
            : base()
        {
            Instance = this;
        }

        /// <summary>
        /// Called when the mod is loaded
        /// </summary>
        public override void OnLoad()
        {
            Mct.EnsureMct("Ingame Cheat Menu");
            Mct.Init();

            base.OnLoad();

            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(ICMDataFile, FileMode.Open);
            //    ReadSettings(fs);
            //}
            //catch (IOException)
            //{
            //    fs = new FileStream(ICMDataFile, FileMode.Create);
            //    WriteSettings(fs);
            //}
            //finally
            //{
            //    if (fs != null)
            //        fs.Close();
            //}
        }

        /// <summary>
        /// Called after the game is drawn
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Game</param>
        public override void PostGameDraw(SpriteBatch sb)
        {
            base.PostGameDraw(sb);

            if (Menu.currentPage == "Player Select")
            {
                MenuPage p = Menu.menuPages["Player Select"];

                for (int i = 0; i < 5; i++)
                    p.buttons[i + editPlayerOffset].Disable(i + Menu.skip >= Main.numLoadPlayers);
            }
            if (Menu.currentPage == "World Select")
            {
                MenuPage p = Menu.menuPages["World Select"];

                for (int i = 0; i < 5; i++)
                    p.buttons[i + editWorldOffset].Disable(i + Menu.skip >= Main.numLoadWorlds);
            }
        }

        /// <summary>
        /// Called when all mods are loaded
        /// </summary>
        public override void OnAllModsLoaded()
        {
            // easy as 4 * Math.Atan(1)

            MctUI.AddCustomUI(MainUI.Interface = new MainUI());

            MctUI.AddCustomUI(ItemUI.Instance = new ItemUI());
            MctUI.AddCustomUI(BuffUI.Instance = new BuffUI());
            MctUI.AddCustomUI(PrefixUI.Instance = new PrefixUI());
            MctUI.AddCustomUI(NpcUI.Instance = new NpcUI());
            MctUI.AddCustomUI(PlayerUI.Instance = new PlayerUI());
            MctUI.AddCustomUI(WorldUI.Instance = new WorldUI());

            //MctUI.AddCustomUI(EditGlobalNPCUI.Interface = new EditGlobalNPCUI());
            //MctUI.AddCustomUI(EditItemUI.Interface = new EditItemUI());


            // a bit less easy...
            Menu.menuPages.Add("ICM:Edit World", new EditWorldPage());

            MenuAnchor aCustom = new MenuAnchor()
            {
                anchor = new Vector2(0.5f, 0f),
                offset = new Vector2(315f, 200f),
                offset_button = new Vector2(0f, 50f)
            };

            //Menu.menuPages.Add("ICM:Settings", new SettingsPage());

            //Menu.menuPages["Options"].anchors.Add(aOptions);
            //Menu.menuPages["Options"].buttons.Add(new MenuButton(1, "ICM Settings", "ICM:Settings").With(mb => mb.SetAutomaticPosition(aCustom, 0)));

            #region edit player code
            editPlayerOffset = Menu.menuPages["Player Select"].buttons.Count;

            for (int i = 0; i < 5; i++)
            {
                int index = i;
                Menu.menuPages["Player Select"].buttons.Add(new MenuButton(new Vector2(0.5f, 0), new Vector2(100, 200 + i * 50), "Edit Player", "")
                .Where(w =>
                {
                    w.Update = () =>
                    {
                        if (index + Menu.skip < Main.numLoadPlayers)
                        {
                            w.displayText = "Edit " + Main.loadPlayer[index + Menu.skip].name;

                            w.scale = Math.Min(1f, (w.size.X - 20) / Main.fontMouseText.MeasureString(w.displayText).X);
                        }
                        //else
                        //{
                        //    // disabling done in PostGameDraw
                        //}
                    };
                    w.Click = () =>
                    {
                        int pid = index + Menu.skip;

                        if (pid >= Main.numLoadPlayers)
                        {
                            Main.loadPlayer[Main.numLoadPlayers] = new Player();
                            Main.loadPlayer[Main.numLoadPlayers].inventory[0].SetDefaults("Copper Shortsword");
                            Main.loadPlayer[Main.numLoadPlayers].inventory[0].Prefix(-1);
                            Main.loadPlayer[Main.numLoadPlayers].inventory[1].SetDefaults("Copper Pickaxe");
                            Main.loadPlayer[Main.numLoadPlayers].inventory[1].Prefix(-1);
                            Main.loadPlayer[Main.numLoadPlayers].inventory[2].SetDefaults("Copper Axe");
                            Main.loadPlayer[Main.numLoadPlayers].inventory[2].Prefix(-1);
                            Menu.MoveTo("Create Player");
                            return;
                        }

                        string name = (string)Main.loadPlayer[pid].name.Clone(); // backup name because it's reset in {Create Player}.OnEntry

                        int oldLoadPlayers = Main.numLoadPlayers;
                        Main.numLoadPlayers = pid;

                        string path = Main.PlayerPath + "\\" + Main.loadPlayer[pid].name + ".plr";

                        MenuPage create = Menu.menuPages["Create Player"];

                        // sometimes, hacking is NOT easy

                        if (File.Exists(path))
                        {
                            File.WriteAllBytes(path + ".ipb", File.ReadAllBytes(path)); // ICM Player Backup
                            File.Delete(path);
                        }
                        if (File.Exists(path + ".bak"))
                        {
                            File.WriteAllBytes(path + ".bak.ipb", File.ReadAllBytes(path + ".bak"));
                            File.Delete(path + ".bak");
                        }

                        Action
                            entry        = null,
                            entryCleanup = null;

                        entryCleanup = () =>
                        {
                            Main.numLoadPlayers = oldLoadPlayers;
                            Menu.menuPages["Player Select"].OnEntry -= entryCleanup;

                            if (!File.Exists(path) && File.Exists(path + ".ipb"))
                            {
                                File.WriteAllBytes(path, File.ReadAllBytes(path + ".ipb"));
                                File.Delete(path + ".ipb");
                            }

                            if (!File.Exists(path + ".bak") && File.Exists(path + ".bak.ipb"))
                            {
                                File.WriteAllBytes(path, File.ReadAllBytes(path + ".bak.ipb"));
                                File.Delete(path + ".bak.ipb");
                            }
                        };
                        entry        = () =>
                        {
                            if (create.buttons[17].displayText == "Save Player")
                                create.buttons[17].displayText = "Create Player";
                            else
                                create.buttons[17].displayText = "Save Player";

                            create.OnEntry -= entry;
                            Menu.menuPages["Player Select"].OnEntry += entryCleanup;
                        };
                        create.OnEntry += entry;

                        Menu.MoveTo("Create Player");

                        Main.loadPlayer[pid].name = name;
                    };
                }));
            }
            #endregion

            #region edit world code
            editWorldOffset = Menu.menuPages["World Select"].buttons.Count;

            for (int i = 0; i < 5; i++)
            {
                int index = i;
                Menu.menuPages["World Select"].buttons.Add(new MenuButton(new Vector2(0.5f, 0), new Vector2(100, 200 + i * 50), "Edit World", "")
                .Where(w =>
                {
                    w.Update = () =>
                    {
                        if (index + Menu.skip < Main.numLoadPlayers)
                        {
                            w.displayText = "Edit " + Main.loadWorld[index + Menu.skip];

                            w.scale = Math.Min(1f, (w.size.X - 20) / Main.fontMouseText.MeasureString(w.displayText).X);
                        }
                    };
                    w.Click = () =>
                    {
                        int wid = index + Menu.skip;

                        EditWorldPage.selectedWorld     = Main.loadWorld    [wid];
                        EditWorldPage.selectedWorldPath = Main.loadWorldPath[wid];

                        ((EditWorldPage)Menu.menuPages["ICM:Edit World"]).LoadData();

                        Menu.MoveTo("ICM:Edit World"); // editing is done there
                    };
                }));
            }
            #endregion

            base.OnAllModsLoaded();
        }

        /// <summary>
        /// Called when the mod is unloaded
        /// </summary>
        public override void OnUnload()
        {
            base.OnUnload();

            //FileStream fs = new FileStream(Main.SavePath + "\\ICM_Data.sav", FileMode.Create);
            //WriteSettings(fs);
            //fs.Close();

            Instance = null;
        }

        /// <summary>
        /// Writes the ICM settings to a Stream
        /// </summary>
        /// <param name="s">The Stream to write the data to</param>
        [Obsolete("This code is not used.")]
        public static void ReadSettings(Stream s)
        {
            SettingsPage.AccentColour = (AccentColour)s.ReadByte();
            SettingsPage.ThemeColour = (ThemeColour)s.ReadByte();
        }
        /// <summary>
        /// Reads the ICM settings from a Stream
        /// </summary>
        /// <param name="s">The Stream to read the data from</param>
        [Obsolete("This code is not used.")]
        public static void WriteSettings(Stream s)
        {
            s.WriteByte((byte)SettingsPage.AccentColour);
            s.WriteByte((byte)SettingsPage.ThemeColour);
        }
    }
}
