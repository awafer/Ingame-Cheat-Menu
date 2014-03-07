using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI;
using TAPI.SDK;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.MenuItems;
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
            Sdk.Init();
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

            SdkUI.AddCustomUI(MainUI.Interface = new MainUI());

            SdkUI.AddCustomUI(ItemUI.Interface = new ItemUI());
            SdkUI.AddCustomUI(BuffUI.Interface = new BuffUI());
            SdkUI.AddCustomUI(PrefixUI.Interface = new PrefixUI());
            SdkUI.AddCustomUI(NPCUI.Interface = new NPCUI());
            SdkUI.AddCustomUI(PlayerUI.Interface = new PlayerUI());
            SdkUI.AddCustomUI(WorldUI.Interface = new WorldUI());

            SdkUI.AddCustomUI(EditPlayerUI.Interface = new EditPlayerUI());
            SdkUI.AddCustomUI(EditGlobalNPCUI.Interface = new EditGlobalNPCUI());
            SdkUI.AddCustomUI(EditItemUI.Interface = new EditItemUI());
            SdkUI.AddCustomUI(EditNPCUI.Interface = new EditNPCUI());

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
