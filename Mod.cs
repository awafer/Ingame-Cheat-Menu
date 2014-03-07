using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using TAPI.SDK;
//using TAPI.SDK.GUI;
//using TAPI.PoroCYon.ICM.Menus;
//using TAPI.PoroCYon.ICM.Menus.Sub;

namespace TAPI.PoroCYon.ICM
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
            base.OnLoad();

            Sdk.Init();
        }

        /// <summary>
        /// Called when all mods are loaded
        /// </summary>
        public override void OnAllModsLoaded()
        {
            UI.Init();

            FileStream fs = null;
            try
            {
                fs = new FileStream(ICMDataFile, FileMode.Open);
                ReadSettings(fs);
                fs.Close();
            }
            catch (IOException)
            {
                fs = new FileStream(ICMDataFile, FileMode.Create);
                WriteSettings(fs);
                fs.Close();
            }

            // easy as 4 * Math.Atan(1)

            //SdkUI.AddUI(MainUI.Interface = new MainUI());

            //SdkUI.AddUI(ItemUI.Interface = new ItemUI());
            //SdkUI.AddUI(BuffUI.Interface = new BuffUI());
            //SdkUI.AddUI(PrefixUI.Interface = new PrefixUI());
            //SdkUI.AddUI(NPCUI.Interface = new NPCUI());
            //SdkUI.AddUI(PlayerUI.Interface = new PlayerUI());
            //SdkUI.AddUI(WorldUI.Interface = new WorldUI());

            //SdkUI.AddUI(SettingsUI.Interface = new SettingsUI());

            //SdkUI.AddUI(EditPlayerUI.Interface = new EditPlayerUI());
            //SdkUI.AddUI(EditGlobalNPCUI.Interface = new EditGlobalNPCUI());
            //SdkUI.AddUI(EditItemUI.Interface = new EditItemUI());
            //SdkUI.AddUI(EditNPCUI.Interface = new EditNPCUI());

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
            UI.MenuThemes.Accent = (UI.MenuThemes.ForeColor)s.ReadByte();
            UI.MenuThemes.Theme = (UI.MenuThemes.BGColor)s.ReadByte();
            //SettingsUI.AccentColour = (AccentColour)s.ReadByte();
            //SettingsUI.ThemeColour  = (ThemeColour)s.ReadByte();
        }
        /// <summary>
        /// Reads the ICM settings from a Stream
        /// </summary>
        /// <param name="s">The Stream to read the data from</param>
        public static void WriteSettings(Stream s)
        {
            s.WriteByte((byte)UI.MenuThemes.Accent);
            s.WriteByte((byte)UI.MenuThemes.Theme);
            //s.WriteByte((byte)SettingsUI.AccentColour);
            //s.WriteByte((byte)SettingsUI.ThemeColour );
        }

        public override void PostGameDraw(SpriteBatch sb)
        {
            UI.Update();

            base.PostGameDraw(sb);
        }
    }
}
