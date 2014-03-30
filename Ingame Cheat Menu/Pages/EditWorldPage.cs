using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ionic.Zip;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Graphics;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.MenuItems;

namespace PoroCYon.ICM.Pages
{
    /// <summary>
    /// The MenuPage used to edit a world
    /// </summary>
    public sealed class EditWorldPage : Page
    {
        internal static string selectedWorld, selectedWorldPath;
        static JsonData worldJson;

        static MenuButtonStringValue name;

        /// <summary>
        /// Creates a new instance of the EditWorldPage class
        /// </summary>
        public EditWorldPage()
            : base()
        {

        }

        internal void LoadData()
        {
            using (ZipFile zf = new ZipFile(selectedWorldPath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    zf["Info.json"].Extract(ms);
                    worldJson = JsonMapper.ToObject(Encoding.UTF8.GetString(ms.ToArray()));

                    ms.Close();
                }
            }
        }

        /// <summary>
        /// Initializes the Page
        /// </summary>
        protected override void Init()
        {
            base.Init();

            MenuAnchor
                aLeft = new MenuAnchor()  
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(-105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aRight = new MenuAnchor() 
                {
                    anchor = new Vector2(0.5f, 0),
                    offset = new Vector2(105, 200),
                    offset_button = new Vector2(0, 50)
                },
                aCentre = new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(0f, 200f),
                    offset_button = new Vector2(0f, 50f)
                };

            int lid = 0, cid = 0, rid = 0;

            anchors.AddRange(new List<MenuAnchor>() { aLeft, aRight, aCentre });

            Main.newWorldName = (string)worldJson["name"];

            buttons.Add(name = new MenuButtonStringValue(new Vector2(0.5f, 0), new Vector2(-150, 200), "World Name", "").SetSize(300, 60)
                .Where(mbsv =>
                {
                    mbsv.displayTextValueEmpty = selectedWorld;

                    mbsv.Update = () =>
                    {
                        mbsv.displayTextValueEmpty = selectedWorld;

                        if (mbsv.whoAmi == Menu.setButton)
                        {
                            string old = Main.newWorldName;
                            Main.newWorldName = Main.GetInputText(Main.newWorldName);

                            if (Main.newWorldName.Length > 20)
                                Main.newWorldName = Main.newWorldName.Substring(0, 20);

                            if (old != Main.newWorldName)
                                Main.PlaySound(12, -1, -1, 1);

                            if (Main.inputTextEnter)
                                Menu.setButton = -1;
                        }

                        mbsv.scaleValue = Math.Min(1f, (mbsv.size.X - 20) / Main.fontMouseText.MeasureString(mbsv.displayTextValue = Main.newWorldName).X);
                    };

                    mbsv.SetAutomaticPosition(aCentre, cid++);
                }));
            cid++;

            // more stuff here later

            buttons.Add(new MenuButton(0, "Save & go back", "World Select").Where(mb =>
            {
                mb.Click += () =>
                {
                    using (ZipFile zf = new ZipFile(selectedWorldPath))
                    {
                        zf.RemoveEntry("Info.json");
                        zf.AddEntry("Info.json", JsonMapper.ToJson(worldJson));

                        zf.Save();
                    }
                };

                mb.SetAutomaticPosition(aCentre, cid++);
            }));
            buttons.Add(new MenuButton(0, "Back", "World Select").Where(mb => mb.SetAutomaticPosition(aCentre, cid++)));
        }

        /// <summary>
        /// Updates the Page
        /// </summary>
        protected override void Update()
        {
            base.Update();

            worldJson["name"] = name.displayTextValue;
        }
    }
}
