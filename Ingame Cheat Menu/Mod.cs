using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.MCT.UI.MenuItems;
using PoroCYon.ICM.Menus;
using PoroCYon.ICM.Menus.Sub;

namespace PoroCYon.ICM
{
    /// <summary>
    /// The mod entry point
    /// </summary>
    public sealed class Mod : ModBase
    {
        /// <summary>
        /// The path to the ICM_Data.sav file
        /// </summary>
        public readonly static string ICMDataFile = Main.SavePath + "\\ICM_Data.sav";

        /// <summary>
        /// The Mod singleton instance
        /// </summary>
        public static Mod ModInstance;

        /// <summary>
        /// Creates a new instance of the Mod class. Called through reflection.
        /// </summary>
        public Mod()
            : base()
        {
            ModInstance = this;
        }

        /// <summary>
        /// Called when the mod is loaded
        /// </summary>
        public override void OnLoad()
        {
            Mct.Init();
            base.OnLoad();

            FileStream fs = null;
            try
            {
                fs = new FileStream(ICMDataFile, FileMode.Open);
                ReadSettings(fs);
            }
            catch (IOException)
            {
                fs = new FileStream(ICMDataFile, FileMode.Create);
                WriteSettings(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// Called when all mods are loaded
        /// </summary>
        public override void OnAllModsLoaded()
        {
            // easy as 4 * Math.Atan(1)

            MctUI.AddCustomUI(MainUI.Interface = new MainUI());

            MctUI.AddCustomUI(ItemUI.Interface = new ItemUI());
            MctUI.AddCustomUI(BuffUI.Interface = new BuffUI());
            MctUI.AddCustomUI(PrefixUI.Interface = new PrefixUI());
            MctUI.AddCustomUI(NPCUI.Interface = new NPCUI());
            MctUI.AddCustomUI(PlayerUI.Interface = new PlayerUI());
            MctUI.AddCustomUI(WorldUI.Interface = new WorldUI());

            MctUI.AddCustomUI(EditPlayerUI.Interface = new EditPlayerUI());
            MctUI.AddCustomUI(EditGlobalNPCUI.Interface = new EditGlobalNPCUI());
            MctUI.AddCustomUI(EditItemUI.Interface = new EditItemUI());
            MctUI.AddCustomUI(EditNPCUI.Interface = new EditNPCUI());

            // a bit less easier...
            Menu.menuPages.Add("ICM:Settings", new SettingsPage());

            MenuAnchor aOptions = new MenuAnchor()
            {
                anchor = new Vector2(0.5f, 0f),
                offset = new Vector2(315f, 200f),
                offset_button = new Vector2(0f, 50f)
            };

            Menu.menuPages["Options"].anchors.Add(aOptions);
            //Menu.menuPages["Options"].buttons.Add(new MenuButton(0, "ICM Settings", "ICM:Settings").With(mb => mb.SetAutomaticPosition(aOptions, 0)));

            base.OnAllModsLoaded();
        }

        /// <summary>
        /// Called when the mod is unloaded
        /// </summary>
        public override void OnUnload()
        {
            base.OnUnload();

            FileStream fs = new FileStream(Main.SavePath + "\\ICM_Data.sav", FileMode.Create);
            WriteSettings(fs);
            fs.Close();

            ModInstance = null;
        }

        /// <summary>
        /// Writes the ICM settings to a Stream
        /// </summary>
        /// <param name="s">The Stream to write the data to</param>
        public static void ReadSettings(Stream s)
        {
            SettingsPage.AccentColour = (AccentColour)s.ReadByte();
            SettingsPage.ThemeColour  = (ThemeColour) s.ReadByte();
        }
        /// <summary>
        /// Reads the ICM settings from a Stream
        /// </summary>
        /// <param name="s">The Stream to read the data from</param>
        public static void WriteSettings(Stream s)
        {
            s.WriteByte((byte)SettingsPage.AccentColour);
            s.WriteByte((byte)SettingsPage.ThemeColour );
        }
    }
}
