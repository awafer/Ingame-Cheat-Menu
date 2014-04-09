using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ionic.Zip;
using LitJson;
using PoroCYon.XnaExtensions;
using TAPI;
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
        static MenuButton[] oreButtons = new MenuButton[3];

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
            buttons.Clear();
            anchors.Clear();

            base.Init();

            MenuAnchor
                aLeft = new MenuAnchor()  
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(-210f, 200f),
                    offset_button = new Vector2(0f, 50f)
                },
                aRight = new MenuAnchor() 
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(210f, 200f),
                    offset_button = new Vector2(0f, 50f)
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
            lid++; lid++;
            rid++; rid++;

            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Eye of Cthulhu"], "Defeated Eye of Ctulhu")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Eye of Cthulhu"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Eye of Cthulhu"] = false;
                    };

                    cb.SetAutomaticPosition(aLeft, lid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Eater of Worlds"], "Defeated Eater of Worlds")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Eater of Worlds"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Eater of Worlds"] = false;
                    };

                    cb.SetAutomaticPosition(aLeft, lid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Skeletron"], "Defeated Skeletron")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Skeletron"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Skeletron"] = false;
                    };

                    cb.SetAutomaticPosition(aLeft, lid++);
                }));

            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Queen Bee"], "Defeated Queen Bee")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Queen Bee"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Queen Bee"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["The Destroyer"], "Defeated The Destroyer")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["The Destroyer"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["The Destroyer"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Skeletron"], "Defeated The Twins")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["The Twins"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["The Twins"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));

            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Skeletron Prime"], "Defeated Skeletron Prime")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Skeletron Prime"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Skeletron Prime"] = false;
                    };

                    cb.SetAutomaticPosition(aRight, rid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Plantera"], "Defeated Plantera")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Plantera"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Plantera"] = false;
                    };

                    cb.SetAutomaticPosition(aRight, rid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["bosses"]["Golem"], "Defeated Golem")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Golem"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["bosses"]["Golem"] = false;
                    };

                    cb.SetAutomaticPosition(aRight, rid++);
                }));

            buttons.Add(new CheckBox((bool)worldJson["crimson"], "Crimson")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["crimson"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["crimson"] = false;
                    };

                    cb.SetAutomaticPosition(aLeft, lid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["corruption"], "Corruption")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["corruption"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["corruption"] = false;
                    };

                    cb.SetAutomaticPosition(aRight, rid++);
                }));

            buttons.Add(new CheckBox((bool)worldJson["progress"]["npcs"]["Goblin Tinkerer"], "Saved Goblin Tinkerer")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Goblin Tinkerer"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Goblin Tinkerer"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["npcs"]["Wizard"], "Saved Wizard")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Wizard"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Wizard"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));
            buttons.Add(new CheckBox((bool)worldJson["progress"]["npcs"]["Mechanic"], "Saved Mechanic")
                .Where(cb =>
                {
                    cb.OnChecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Mechanic"] = true;
                    };
                    cb.OnUnchecked += delegate
                    {
                        worldJson["progress"]["npcs"]["Mechanic"] = false;
                    };

                    cb.SetAutomaticPosition(aCentre, cid++);
                }));

            buttons.Add(new MenuButtonPlusMinus(0, "Altars smashed", "", "", () => { })
                .Where(mbpm =>
                {
                    mbpm.displayText = "Altars smashed: " + (int)worldJson["progress"]["altarCounter"];

                    mbpm.Click = () =>
                    {
                        if (mbpm.currentSelected == 1)
                            worldJson["progress"]["altarCounter"] = (int)worldJson["progress"]["altarCounter"] - 1;
                        else if (mbpm.currentSelected >= 1)
                            worldJson["progress"]["altarCounter"] = (int)worldJson["progress"]["altarCounter"] + 1;

                        if ((int)worldJson["progress"]["altarCounter"] < 0)
                            worldJson["progress"]["altarCounter"] = 0;

                        mbpm.displayText = "Altars smashed: " + (int)worldJson["progress"]["altarCounter"];
                    };

                    mbpm.ClickHold = () =>
                    {
                        if (mbpm.currentSelected < 1)
                            mbpm.framesHeld = 0;
                        else if (mbpm.framesHeld >= 20 && mbpm.framesHeld % 2 == 0)
                            mbpm.Click();
                    };

                    mbpm.Update = () =>
                    {
                        if (mbpm.currentSelected < 0)
                            return;

                        int
                            scroll = (Main.mouseState.ScrollWheelValue - Main.oldMouseState.ScrollWheelValue) / 120,
                            original = mbpm.currentSelected;

                        while (scroll != 0)
                        {
                            if (scroll < 0)
                                mbpm.currentSelected = 1;
                            else
                                mbpm.currentSelected = 2;

                            scroll -= Math.Sign(scroll);

                            mbpm.Click();
                        }

                        mbpm.currentSelected = original;
                    };

                    mbpm.SetAutomaticPosition(aLeft, lid++);
                    mbpm.SetSize(new Vector2(250f, 50f));
                    mbpm.customPos = mbpm.position - new Vector2(50f, 5f);
                }));
            buttons.Add(new MenuButtonPlusMinus(0, "Shadow orbs smashed", "", "", () => { })
                .Where(mbpm =>
                {
                    mbpm.displayText = "Shadow orbs smashed: " + (int)worldJson["progress"]["shadowOrb"]["counter"];

                    mbpm.Click = () =>
                    {
                        if (mbpm.currentSelected == 1)
                            worldJson["progress"]["shadowOrb"]["counter"] = (int)worldJson["progress"]["shadowOrb"]["counter"] - 1;
                        else if (mbpm.currentSelected >= 1)
                            worldJson["progress"]["shadowOrb"]["counter"] = (int)worldJson["progress"]["shadowOrb"]["counter"] + 1;

                        if ((int)worldJson["progress"]["shadowOrb"]["counter"] < 0)
                            worldJson["progress"]["shadowOrb"]["counter"] = 0;

                        mbpm.displayText = "Shadow orbs smashed: " + (int)worldJson["progress"]["shadowOrb"]["counter"];
                    };

                    mbpm.ClickHold = () =>
                    {
                        if (mbpm.currentSelected < 1)
                            mbpm.framesHeld = 0;
                        else if (mbpm.framesHeld >= 20 && mbpm.framesHeld % 2 == 0)
                            mbpm.Click();
                    };

                    mbpm.Update = () =>
                    {
                        if (mbpm.currentSelected < 0)
                            return;

                        int
                            scroll = (Main.mouseState.ScrollWheelValue - Main.oldMouseState.ScrollWheelValue) / 120,
                            original = mbpm.currentSelected;

                        while (scroll != 0)
                        {
                            if (scroll < 0)
                                mbpm.currentSelected = 1;
                            else
                                mbpm.currentSelected = 2;

                            scroll -= Math.Sign(scroll);

                            mbpm.Click();
                        }

                        mbpm.currentSelected = original;
                    };

                    mbpm.SetAutomaticPosition(aRight, rid++);
                    mbpm.SetSize(new Vector2(300f, 50f));
                    mbpm.customPos = mbpm.position - new Vector2(0f, 5f);
                }));

            if ((int)worldJson["hardmodeOres"][0] == -1)
                worldJson["hardmodeOres"][0] = 107;
            if ((int)worldJson["hardmodeOres"][1] == -1)
                worldJson["hardmodeOres"][1] = 187;
            if ((int)worldJson["hardmodeOres"][2] == -1)
                worldJson["hardmodeOres"][2] = 111;

            buttons.Add(oreButtons[0] = new MenuButton(0, "Cobalt", "", "", () =>
            {
                if ((int)worldJson["hardmodeOres"][0] == 107)
                {
                    worldJson["hardmodeOres"][0] = 221;
                    oreButtons[0].displayText = "Palladium";
                }
                else
                {
                    worldJson["hardmodeOres"][0] = 107;
                    oreButtons[0].displayText = "Cobalt";
                }
            }).Where(mb =>
            {
                if ((int)worldJson["hardmodeOres"][0] == 221)
                    mb.displayText = "Palladium";

                mb.SetAutomaticPosition(aLeft, lid++);
            }));
            buttons.Add(oreButtons[1] = new MenuButton(0, "Mithril", "", "", () =>
            {
                if ((int)worldJson["hardmodeOres"][1] == 108)
                {
                    worldJson["hardmodeOres"][1] = 222;
                    oreButtons[1].displayText = "Orchialcum";
                }
                else
                {
                    worldJson["hardmodeOres"][1] = 108;
                    oreButtons[1].displayText = "Mithril";
                }
            }).Where(mb =>
            {
                if ((int)worldJson["hardmodeOres"][1] == 222)
                    mb.displayText = "Orchialcum";

                mb.SetAutomaticPosition(aCentre, cid++);
            }));
            buttons.Add(oreButtons[2] = new MenuButton(0, "Adamantite", "", "", () =>
            {
                if ((int)worldJson["hardmodeOres"][2] == 111)
                {
                    worldJson["hardmodeOres"][2] = 223;
                    oreButtons[2].displayText = "Titanium";
                }
                else
                {
                    worldJson["hardmodeOres"][2] = 111;
                    oreButtons[2].displayText = "Adamantite";
                }
            }).Where(mb =>
            {
                if ((int)worldJson["hardmodeOres"][2] == 223)
                    mb.displayText = "Titanium";

                mb.SetAutomaticPosition(aRight, rid++);
            }));

            lid++;
            cid++;
            rid++;

            buttons.Add(new MenuButton(0, "Save & go back", "World Select").Where(mb =>
            {
                mb.Click += () =>
                {
                    using (ZipFile zf = new ZipFile(selectedWorldPath))
                    {
                        zf.RemoveEntry("Info.json");
                        zf.AddEntry("Info.json", JsonMapper.ToJson(worldJson));

                        zf.Save();

                        Main.LoadWorlds();
                    }
                };

                mb.SetAutomaticPosition(new MenuAnchor()
                {
                    anchor = new Vector2(0.5f, 0f),
                    offset = new Vector2(-105f, 200f),
                    offset_button = new Vector2(0f, 50f)
                }, lid++);
            }));
            buttons.Add(new MenuButton(0, "Go back without saving", "World Select").Where(mb => mb.SetAutomaticPosition(new MenuAnchor()
            {
                anchor = new Vector2(0.5f, 0f),
                offset = new Vector2(105f, 200f),
                offset_button = new Vector2(0f, 50f)
            }, rid++)));
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
