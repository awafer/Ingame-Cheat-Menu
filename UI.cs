using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.XnaExtensions;
using PoroCYon.XnaExtensions.Geometry;
using PoroCYon.XnaExtensions.Input;
using TAPI;
using PoroCYon.MCT;

namespace PoroCYon.ICM
{
    using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
    using Mouse = Microsoft.Xna.Framework.Input.Mouse;

    public enum UIType : int
    {
        NPCVars = -3,
        CharMod = -2,
        None = -1,
        Item = 0,
        Buff = 1,
        Prefix = 2,
        NPC = 3,
        Player = 4,
        World = 5,
        Settings = 6,
        Count = 7
    }

    [GlobalMod]
    public sealed class UI : ModInterface
    {
        public UI(ModBase @base) : base(@base) { }

        #region Child classes
        public static class ItemCheat
        {
            //public enum Category
            //{
            //    None = 0,

            //    Melee = 1,
            //    Ranged = 2,
            //    Magic = 3,

            //    Helmet = 4,
            //    Torso = 5,
            //    Leggings = 6,

            //    Vanity = 7,
            //    Accessories = 8,
            //    Tools = 9,

            //    Ammunition = 10,
            //    Materials = 11,
            //    Consumables = 12,

            //    Tiles = 13,
            //    Walls = 14,
            //    Others = 15,

            //    Count = 16
            //}

            //public static Category filter;

            public static void Update()
            {
                Rectangle r;
                Vector2 v;

                if (pressCD < 8)
                    pressCD++;

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8 && position > -48)
                    {
                        position -= 4;
                        if (position < -48)
                            position = -48;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8 && position + 20 < Main.maxItemTypes)
                    {
                        position += 4;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region item list
                int col = 0, row = 0, added = 0;
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.items.Count)
                        break;
                    if (i < -48)
                        continue;
                    if (i == 0 || !Defs.itemNames.ContainsKey(i) || !Defs.items[Defs.itemNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (new Rectangle(160 + col * Main.inventoryBack11Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack11Texture.Height,
                        Main.inventoryBack11Texture.Width, Main.inventoryBack11Texture.Height).Intersects(mouse) && i != 0)
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            //Main.mouseItem = (Item)Defs.items[Defs.itemNames[i]].Clone();
                            //Main.mouseItem.stack = Defs.items[Defs.itemNames[i]].maxStack;
                            Main.mouseItem.netDefaults(i);
                            Main.mouseItem.stack = Main.mouseItem.maxStack;
                            Main.PlaySound(7);
                        }
                    }
                    row++;
                }
                position -= added;
                #endregion

                #region jump to
                #region by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 364;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 604;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region by ID
                v = new Vector2(240f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("netID").X, (int)Main.fontMouseText.MeasureString("netID").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = -48;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(300f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1+").X, (int)Main.fontMouseText.MeasureString("1+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1;
                        Main.PlaySound(12);
                    }
                }

                #region 100+ - 300+
                v = new Vector2(240f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("100+").X, (int)Main.fontMouseText.MeasureString("100+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 10;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(290f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("200+").X, (int)Main.fontMouseText.MeasureString("200+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 200;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(340f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("300+").X, (int)Main.fontMouseText.MeasureString("300+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 300;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region 400+ - 600+
                v = new Vector2(240f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("400+").X, (int)Main.fontMouseText.MeasureString("400+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 400;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(290f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("500+").X, (int)Main.fontMouseText.MeasureString("500+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 500;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(340f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("600+").X, (int)Main.fontMouseText.MeasureString("600+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 600;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region 700+ - 900+
                v = new Vector2(240f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("700+").X, (int)Main.fontMouseText.MeasureString("700+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 700;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(290f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("800+").X, (int)Main.fontMouseText.MeasureString("800+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 800;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(340f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("900+").X, (int)Main.fontMouseText.MeasureString("900+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 900;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region 1000+ - 1200+
                v = new Vector2(240f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1000+").X, (int)Main.fontMouseText.MeasureString("1000+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1000;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(290f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1100+").X, (int)Main.fontMouseText.MeasureString("1100+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1100;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(340f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1200+").X, (int)Main.fontMouseText.MeasureString("1200+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1200;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region 1300+ - 1500+
                v = new Vector2(240f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1300+").X, (int)Main.fontMouseText.MeasureString("1300+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1300;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(290f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1400+").X, (int)Main.fontMouseText.MeasureString("1400+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1400;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(340f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1500+").X, (int)Main.fontMouseText.MeasureString("1500+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1500;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region 1600+ - 1800+
                v = new Vector2(240f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1600+").X, (int)Main.fontMouseText.MeasureString("1600+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1600;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(290f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1700+").X, (int)Main.fontMouseText.MeasureString("1700+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1700;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(340f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1800+").X, (int)Main.fontMouseText.MeasureString("1800+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1800;
                        Main.PlaySound(12);
                    }
                }
                #endregion
                #endregion
                #endregion

                #region search
                if (keyState.IsKeyDown(Key.Return) && oldKeyState.IsKeyUp(Key.Return)) // if just pressed enter
                {
                    searching = !searching;
                    Main.clrInput();
                    if (!searching)
                        search = "";
                }
                if (searching)
                {
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Main.playerInventory = false;
                        return;
                    }
                    Main.editSign = true;
                    search = Main.GetInputText(search).ToLower();
                }
                #endregion

                #region categories
                //col = row = 0;
                //for (int i = 0; i < (int)Category.Count; i++)
                //{
                //    if (col >= 3)
                //    {
                //        row++;
                //        col = 0;
                //    }
                //    v = new Vector2(430f + col * panel.Width, Main.screenHeight - 350f + row * panel.Height);
                //    r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);

                //    if (mouse.Intersects(r))
                //    {
                //        myP.mouseInterface = true;
                //        if (justClicked)
                //            filter = (Category)i;
                //    }

                //    col++;
                //}
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;

                int col = 0, row = 0, mRare = 0, added = 0;
                bool mouse = false;
                string mText = "";
                Item mItem = new Item();

                #region item list
                #region background tiles
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.items.Count)
                        break;
                    if (i < -48)
                        continue;
                    if (i == 0 || !Defs.itemNames.ContainsKey(i) || !Defs.items[Defs.itemNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    sb.Draw(Main.inventoryBack7Texture,
                        new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                                    (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height), alpha(MenuThemes.BgClr, 150));
                    row++;
                }
                position -= added;
                #endregion
                col = row = added = 0;
                #region item textures
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.items.Count)
                        break;
                    if (i < -48)
                        continue;
                    if (i == 0 || !Defs.itemNames.ContainsKey(i) || !Defs.items[Defs.itemNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (i != 0)
                        sb.Draw(Main.itemTexture[Defs.items[Defs.itemNames[i]].type],
                            new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                                        (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height)
                            + (new Vector2(Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height) / 2f
                            - new Vector2(Main.itemTexture[Defs.items[Defs.itemNames[i]].type].Width, Main.itemTexture[Defs.items[Defs.itemNames[i]].type].Height) / 2f), Defs.items[Defs.itemNames[i]].color == Color.Transparent ? Color.White : full(Defs.items[Defs.itemNames[i]].GetColor(Color.White)));
                    if (new Rectangle(160 + col * Main.inventoryBack7Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack7Texture.Height,
                        Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height).Intersects(new Vector2(Main.mouseX, Main.mouseY)) && i != 0)
                    {
                        mouse = true;
                        mText = Defs.items[Defs.itemNames[i]].AffixName();
                        if (Defs.items[Defs.itemNames[i]].maxStack != 1)
                            mText += " (" + Defs.items[Defs.itemNames[i]].maxStack + ")";
                        mRare = Defs.items[Defs.itemNames[i]].rare;
                        mItem = Defs.items[Defs.itemNames[i]];
                    }
                    row++;
                }
                position -= added;
                #endregion
                #endregion

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                DrawOutlinedString("<", v, clr(r, MenuThemes.ForeClr), 2f);

                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                DrawOutlinedString(">", v, clr(r, MenuThemes.ForeClr), 2f);
                #endregion

                #region jump to
                #region by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                DrawOutlinedString("1.0", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                DrawOutlinedString("1.1", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                DrawOutlinedString("1.2", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region by ID
                v = new Vector2(240f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("netID").X, (int)Main.fontMouseText.MeasureString("netID").Y);
                DrawOutlinedString("netID", v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(300f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1+").X, (int)Main.fontMouseText.MeasureString("1+").Y);
                DrawOutlinedString("1+", v, clr(r, MenuThemes.ForeClr));

                #region 100+ - 300+
                v = new Vector2(240f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("100+").X, (int)Main.fontMouseText.MeasureString("100+").Y);
                DrawOutlinedString("100+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(290f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("200+").X, (int)Main.fontMouseText.MeasureString("200+").Y);
                DrawOutlinedString("200+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(340f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("300+").X, (int)Main.fontMouseText.MeasureString("300+").Y);
                DrawOutlinedString("300+", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region 400+ - 600+
                v = new Vector2(240f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("400+").X, (int)Main.fontMouseText.MeasureString("400+").Y);
                DrawOutlinedString("500+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(290f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("500+").X, (int)Main.fontMouseText.MeasureString("500+").Y);
                DrawOutlinedString("500+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(340f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("600+").X, (int)Main.fontMouseText.MeasureString("600+").Y);
                DrawOutlinedString("600+", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region 700+ - 900+
                v = new Vector2(240f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("700+").X, (int)Main.fontMouseText.MeasureString("700+").Y);
                DrawOutlinedString("700+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(290f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("800+").X, (int)Main.fontMouseText.MeasureString("800+").Y);
                DrawOutlinedString("800+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(340f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("900+").X, (int)Main.fontMouseText.MeasureString("900+").Y);
                DrawOutlinedString("900+", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region 1000+ - 1200+
                v = new Vector2(240f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1000+").X, (int)Main.fontMouseText.MeasureString("1000+").Y);
                DrawOutlinedString("1000+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(290f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1100+").X, (int)Main.fontMouseText.MeasureString("1100+").Y);
                DrawOutlinedString("1100+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(340f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1200+").X, (int)Main.fontMouseText.MeasureString("1200+").Y);
                DrawOutlinedString("1200+", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region 1300+ - 1500+
                v = new Vector2(240f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1300+").X, (int)Main.fontMouseText.MeasureString("1300+").Y);
                DrawOutlinedString("1300+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(290f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1400+").X, (int)Main.fontMouseText.MeasureString("1400+").Y);
                DrawOutlinedString("1400+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(340f, Main.screenHeight - 40f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1500+").X, (int)Main.fontMouseText.MeasureString("1500+").Y);
                DrawOutlinedString("1500+", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region 1600+ - 1800+
                v = new Vector2(240f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1600+").X, (int)Main.fontMouseText.MeasureString("1600+").Y);
                DrawOutlinedString("1600+", v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(290f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1700+").X, (int)Main.fontMouseText.MeasureString("1700+").Y);
                DrawOutlinedString("1700+", v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(340f, Main.screenHeight - 20f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1800+").X, (int)Main.fontMouseText.MeasureString("1800+").Y);
                DrawOutlinedString("1800+", v, clr(r, MenuThemes.ForeClr));
                #endregion
                #endregion
                #endregion

                #region search
                if (search != "")
                    DrawOutlinedString(search, new Vector2(200f, Main.screenHeight - 400f), MenuThemes.ForeClr);
                #endregion

                #region categories
                //col = row = 0;
                //for (int i = 0; i < (int)Category.Count; i++)
                //{
                //    if (col >= 3)
                //    {
                //        row++;
                //        col = 0;
                //    }
                //    v = new Vector2(430f + col * panel.Width, Main.screenHeight - 350f + row * panel.Height);
                //    r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                //    sb.Draw(panel, new Vector2(430f + col * panel.Width, Main.screenHeight - 350f + row * panel.Height), filter == (Category)i ? MenuThemes.ForeClr : clr(r, MenuThemes.ForeClr));

                //    col++;
                //}
                //col = row = 0;
                //for (int i = 0; i < (int)Category.Count; i++)
                //{
                //    if (col >= 3)
                //    {
                //        row++;
                //        col = 0;
                //    }
                //    v = new Vector2(430f + col * panel.Width, Main.screenHeight - 350f + row * panel.Height);
                //    r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                //    Texture2D tex = Main.blackTileTexture;
                //    #region select tex
                //    switch ((Category)i)
                //    {
                //        case Category.Accessories:
                //            tex = Main.itemTexture[216];
                //            break;
                //        case Category.Ammunition:
                //            tex = Main.itemTexture[40];
                //            break;
                //        case Category.Consumables:
                //            tex = Main.itemTexture[28];
                //            break;
                //        case Category.Helmet:
                //            tex = Main.itemTexture[559];
                //            break;
                //        case Category.Leggings:
                //            tex = Main.itemTexture[552];
                //            break;
                //        case Category.Magic:
                //            tex = Main.itemTexture[149];
                //            break;
                //        case Category.Materials:
                //            tex = Main.itemTexture[9];
                //            break;
                //        case Category.Melee:
                //            tex = Main.itemTexture[4];
                //            break;
                //        case Category.None:
                //            col++;
                //            continue;
                //        case Category.Others:
                //            tex = Main.itemTexture[215];
                //            break;
                //        case Category.Ranged:
                //            tex = Main.itemTexture[39];
                //            break;
                //        case Category.Tiles:
                //            tex = Main.itemTexture[3];
                //            break;
                //        case Category.Tools:
                //            tex = Main.itemTexture[1];
                //            break;
                //        case Category.Torso:
                //            tex = Main.itemTexture[551];
                //            break;
                //        case Category.Vanity:
                //            tex = Main.itemTexture[867];
                //            break;
                //        case Category.Walls:
                //            tex = Main.itemTexture[26];
                //            break;
                //    }
                //    #endregion
                //    sb.Draw(tex, v + (new Vector2(panel.Width, panel.Height) / 2f - new Vector2(tex.Width, tex.Height) / 2f), filter == (Category)i ? Color.White : clr(r, Color.White));

                //    if (UI.mouse.Intersects(r))
                //    {
                //        mouse = true;
                //        mText = ((Category)i).ToString();
                //        mItem = null;
                //        mRare = 0;
                //    }

                //    col++;
                //}
                #endregion

                if (mouse)
                {
                    if (mItem != null)
                        Main.toolTip = mItem;
                    mainInstance.MouseText(mText, mRare);
                    Main.toolTip = new Item();
                }
            }
            public static void Init()
            {
                position = -48;
                searching = false;
                search = "";
                Main.clrInput();
                //filter = Category.None;
            }
            public static void Close()
            {
                position = 1;
                searching = false;
                search = "";
                Main.clrInput();
                //filter = Category.None;
            }

            //public static bool IsInCategory(Item item, Category cat)
            //{
            //    switch (cat)
            //    {
            //        case Category.None:
            //            return true;
            //        case Category.Accessories:
            //            return item.accessory;
            //        case Category.Ammunition:
            //            return item.ammo > 0;
            //        case Category.Consumables:
            //            return item.potion || (item.consumable && item.buffType > 0) || item.healMana > 0 || item.healLife > 0;
            //        case Category.Helmet:
            //            return item.headSlot > -1;
            //        case Category.Leggings:
            //            return item.legSlot > -1;
            //        case Category.Magic:
            //            return item.magic;
            //        case Category.Materials:
            //            return item.material;
            //        case Category.Melee:
            //            return item.melee;
            //        case Category.Ranged:
            //            return item.ranged;
            //        case Category.Tiles:
            //            return item.createTile > -1;
            //        case Category.Tools:
            //            return item.pick > 0 || item.axe > 0 || item.hammer > 0;
            //        case Category.Torso:
            //            return item.bodySlot > -1;
            //        case Category.Vanity:
            //            return item.vanity;
            //        case Category.Walls:
            //            return item.createWall > -1;
            //        case Category.Others:
            //            for (int i = 1; i < 15; i++)
            //                if (IsInCategory(item, (Category)i))
            //                    return false;
            //            return true;
            //        default:
            //            throw new ArgumentOutOfRangeException("cat", "Invalid argument: " + cat);
            //    }
            //}
        }
        public static class PrefixCheat
        {
            public static Item instance = new Item();
            public readonly static Item Empty = new Item() { type = 1, name = "Empty", prefix = Prefix.None };

            public static void Update()
            {
                Rectangle r;
                Vector2 v;
                string s;

                int col = 0, row = 0, added = 0;

                if (pressCD < 8)
                    pressCD++;

                #region item container
                v = new Vector2(500f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, Main.inventoryBack10Texture.Width, Main.inventoryBack10Texture.Height);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Item toMouse = (Item)instance.Clone();
                        instance = Main.mouseItem;
                        Main.mouseItem = toMouse;
                        Main.PlaySound(7);
                        position = 1;
                    }
                }
                #endregion

                #region random button
                s = "[RANDOM]";
                v = new Vector2(200f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked && instance.type != 0)
                    {
                        int st = instance.stack;
                        instance.netDefaults(instance.netID);
                        instance.Prefix(-2);
                        instance.stack = st;
                        Main.showItemText = true;
                        ItemText.NewText(instance, instance.stack);
                        Color c = new Color();
                        int mtc = Main.mouseTextColor;
                        float cl = mtc / 255f;
                        switch (instance.rare)
                        {
                            case -1:
                                c = new Color((int)(130f * cl), (int)(130f * cl), (int)(130f * cl), mtc);
                                break;
                            case 1:
                                c = new Color((int)(150f * cl), (int)(150f * cl), (int)(255f * cl), mtc);
                                break;
                            case 2:
                                c = new Color((int)(150f * cl), (int)(255f * cl), (int)(150f * cl), mtc);
                                break;
                            case 3:
                                c = new Color((int)(255f * cl), (int)(200f * cl), (int)(150f * cl), mtc);
                                break;
                            case 4:
                                c = new Color((int)(255f * cl), (int)(150f * cl), (int)(150f * cl), mtc);
                                break;
                            case 5:
                                c = new Color((int)(255f * cl), (int)(150f * cl), (int)(255f * cl), mtc);
                                break;
                            case 6:
                                c = new Color((int)(210f * cl), (int)(160f * cl), (int)(255f * cl), mtc);
                                break;
                            case 7:
                                c = new Color((int)(150f * cl), (int)(255f * cl), (int)(10f * cl), mtc);
                                break;
                            case 8:
                                c = new Color((int)(255f * cl), (int)(255f * cl), (int)(10f * cl), mtc);
                                break;
                            default:
                                c = new Color((int)(5f * cl), (int)(200f * cl), (int)(255f * cl), mtc);
                                break;
                        }
                        CombatText.NewText(myP.Hitbox, c, instance.AffixName(), false, true);
                        Main.PlaySound(2, -1, -1, 37);
                    }
                }
                #endregion

                #region remove prefix button
                s = "[REMOVE PREFIX]";
                v = new Vector2(300f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked && instance.type != 0)
                    {
                        int st = instance.stack;
                        instance.netDefaults(instance.netID);
                        instance.Prefix(0);
                        instance.stack = st;
                        Main.showItemText = true;
                        ItemText.NewText(instance, instance.stack);
                        Color c = new Color();
                        int mtc = Main.mouseTextColor;
                        float cl = mtc / 255f;
                        switch (instance.rare)
                        {
                            case -1:
                                c = new Color((int)(130f * cl), (int)(130f * cl), (int)(130f * cl), mtc);
                                break;
                            case 1:
                                c = new Color((int)(150f * cl), (int)(150f * cl), (int)(255f * cl), mtc);
                                break;
                            case 2:
                                c = new Color((int)(150f * cl), (int)(255f * cl), (int)(150f * cl), mtc);
                                break;
                            case 3:
                                c = new Color((int)(255f * cl), (int)(200f * cl), (int)(150f * cl), mtc);
                                break;
                            case 4:
                                c = new Color((int)(255f * cl), (int)(150f * cl), (int)(150f * cl), mtc);
                                break;
                            case 5:
                                c = new Color((int)(255f * cl), (int)(150f * cl), (int)(255f * cl), mtc);
                                break;
                            case 6:
                                c = new Color((int)(210f * cl), (int)(160f * cl), (int)(255f * cl), mtc);
                                break;
                            case 7:
                                c = new Color((int)(150f * cl), (int)(255f * cl), (int)(10f * cl), mtc);
                                break;
                            case 8:
                                c = new Color((int)(255f * cl), (int)(255f * cl), (int)(10f * cl), mtc);
                                break;
                            default:
                                c = new Color((int)(5f * cl), (int)(200f * cl), (int)(255f * cl), mtc);
                                break;
                        }
                        CombatText.NewText(myP.Hitbox, c, instance.AffixName(), false, true);
                        Main.PlaySound(2, -1, -1, 37);
                    }
                }
                #endregion

                #region prefix list
                Item inst = (Item)instance.Clone();
                if (inst.type == 0)
                    inst = Empty;
                for (int i = position; i < position + 12; i++)
                {
                    if (i >= Defs.prefixes.Count)
                        break;
                    if (i < 1)
                        continue;
                    //inst.prefix = Defs.prefixes[Defs.prefixNames[i]];
                    //s = inst.AffixName().Split(' ')[0];
                    if (!Defs.prefixNames[i].ToLower().Contains(search) || i == 0)
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    v = new Vector2(160f + col * Main.inventoryBack11Texture.Width * 2f, (Main.screenHeight - 350f) + row * Main.inventoryBack11Texture.Height)
                      + new Vector2(Main.inventoryBack11Texture.Width * 2f, Main.inventoryBack11Texture.Height) / 2f - Main.fontMouseText.MeasureString(s) / 2f;
                    r = new Rectangle((int)v.X, (int)v.Y, Main.inventoryBack11Texture.Width * 2, Main.inventoryBack11Texture.Height);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked && instance.type != 0)
                        {
                            int st = instance.stack;
                            instance.netDefaults(instance.netID);
                            instance.Prefix(i);
                            instance.stack = st;
                            Main.showItemText = true;
                            ItemText.NewText(instance, instance.stack);
                            Color c = new Color();
                            float mtc = Main.mouseTextColor, cl = (float)mtc / 255f;
                            switch (instance.rare)
                            {
                                case -1:
                                    c = new Color((int)(130f * cl), (int)(130f * cl), (int)(130f * cl), (int)mtc);
                                    break;
                                case 1:
                                    c = new Color((int)(150f * cl), (int)(150f * cl), (int)(255f * cl), (int)mtc);
                                    break;
                                case 2:
                                    c = new Color((int)(150f * cl), (int)(255f * cl), (int)(150f * cl), (int)mtc);
                                    break;
                                case 3:
                                    c = new Color((int)(255f * cl), (int)(200f * cl), (int)(150f * cl), (int)mtc);
                                    break;
                                case 4:
                                    c = new Color((int)(255f * cl), (int)(150f * cl), (int)(150f * cl), (int)mtc);
                                    break;
                                case 5:
                                    c = new Color((int)(255f * cl), (int)(150f * cl), (int)(255f * cl), (int)mtc);
                                    break;
                                case 6:
                                    c = new Color((int)(210f * cl), (int)(160f * cl), (int)(255f * cl), (int)mtc);
                                    break;
                                case 7:
                                    c = new Color((int)(150f * cl), (int)(255f * cl), (int)(10f * cl), (int)mtc);
                                    break;
                                case 8:
                                    c = new Color((int)(255f * cl), (int)(255f * cl), (int)(10f * cl), (int)mtc);
                                    break;
                                default:
                                    c = new Color((int)(5f * cl), (int)(200f * cl), (int)(255f * cl), (int)mtc);
                                    break;
                            }
                            CombatText.NewText(myP.Hitbox, c, instance.AffixName(), false, true);
                            Main.PlaySound(2, -1, -1, 37);
                        }
                    }
                    row++;
                }
                position -= added;
                #endregion

                #region go up/down
                s = "<";
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X * 2, (int)Main.fontMouseText.MeasureString(s).Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8 && position > 1)
                    {
                        position -= 4;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                s = ">";
                v = new Vector2(450f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X * 2, (int)Main.fontMouseText.MeasureString(s).Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8 && position + 12 <= 84)
                    {
                        position += 4;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region search
                if (keyState.IsKeyDown(Key.Return) && oldKeyState.IsKeyUp(Key.Return)) // if just pressed enter
                {
                    searching = !searching;
                    Main.clrInput();
                    if (!searching)
                        search = "";
                }
                if (searching)
                {
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Main.playerInventory = false;
                        return;
                    }
                    Main.editSign = true;
                    search = Main.GetInputText(search).ToLower();
                }
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;
                string s;

                int col = 0, row = 0, mRare = 0, added = 0;
                bool mouse = false;
                string mText = "";
                Item mItem = null;

                #region item container
                v = new Vector2(500f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height);
                sb.Draw(Main.inventoryBack7Texture, v, alpha(MenuThemes.BgClr, 150));
                if (instance.type != 0)
                {
                    v += new Vector2(Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height) / 2f
                        - new Vector2(Main.itemTexture[instance.type].Width, Main.itemTexture[instance.type].Height) / 2f;
                    sb.Draw(Main.itemTexture[instance.type], v, new Color(255, 255, 255, 150));
                    if (r.Intersects(UI.mouse))
                    {
                        mouse = true;
                        mItem = (Item)instance.Clone();
                        mText = instance.AffixName();
                        if (instance.stack > 1)
                            mText += " (" + instance.stack + ")";
                        mRare = instance.rare;
                    }
                }
                #endregion

                #region random button
                s = "[RANDOM]";
                v = new Vector2(200f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region remove prefix button
                s = "[REMOVE PREFIX]";
                v = new Vector2(300f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region prefix list
                Item inst = (Item)instance.Clone();
                if (inst.type == 0)
                    inst = Empty;
                #region background tiles
                for (int i = position; i < position + 12; i++)
                {
                    if (i >= Defs.prefixes.Count)
                        break;
                    if (i < 1)
                        continue;
                    inst.prefix = Defs.prefixes[Defs.prefixNames[i]];
                    s = inst.AffixName().Split(' ')[0];
                    if (!s.ToLower().Contains(search) || i == 0)
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    sb.Draw(Main.inventoryBack7Texture, new Vector2(160f + col * Main.inventoryBack7Texture.Width * 2f, (Main.screenHeight - 350f) + row
                        * Main.inventoryBack7Texture.Height), null, alpha(MenuThemes.ForeClr, 150), 0f, Vector2.Zero, new Vector2(2f, 1f), SpriteEffects.None, 0f);
                    row++;
                }
                position -= added;
                #endregion
                col = row = added = 0;
                inst = (Item)instance.Clone();
                if (inst.type == 0)
                    inst = Empty;
                #region prefixes
                for (int i = position; i < position + 12; i++)
                {
                    if (i >= 84)
                        break;
                    if (i < 1)
                        continue;
                    inst.prefix = Defs.prefixes[Defs.prefixNames[i]];
                    s = inst.AffixName().Split(' ')[0];
                    if (!s.ToLower().Contains(search) || i == 0)
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    v = new Vector2(160f + col * Main.inventoryBack7Texture.Width * 2f, (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height);
                    r = new Rectangle((int)v.X, (int)v.Y, Main.inventoryBack7Texture.Width * 2, Main.inventoryBack7Texture.Height);
                    DrawOutlinedString(s, v + new Vector2(Main.inventoryBack7Texture.Width * 2f, Main.inventoryBack7Texture.Height)
                        / 2f - Main.fontMouseText.MeasureString(s) / 2f, clr(r, MenuThemes.ForeClr));
                    if (r.Intersects(UI.mouse))
                    {
                        mouse = true;
                        mText = s;
                        mRare = 0;
                        mItem = (Item)instance.Clone();
                        mItem.prefix = Defs.prefixes[Defs.prefixNames[i]];
                    }
                    row++;
                }
                position -= added;
                #endregion
                #endregion

                #region go up/down
                s = "<";
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X * 2, (int)Main.fontMouseText.MeasureString(s).Y * 2);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 2f);

                s = ">";
                v = new Vector2(450f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X * 2, (int)Main.fontMouseText.MeasureString(s).Y * 2);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 2f);
                #endregion

                #region search
                if (search != "")
                    DrawOutlinedString(search, new Vector2(200f, Main.screenHeight - 130f), MenuThemes.ForeClr);
                #endregion

                if (mouse)
                {
                    if (mItem != null)
                        Main.toolTip = mItem;
                    mainInstance.MouseText(mText, mRare);
                }
            }
            public static void Init()
            {
                position = 1;
                searching = false;
                search = "";
                Main.clrInput();
            }
            public static void Close()
            {
                position = 1;
                searching = false;
                search = "";
                Main.clrInput();
            }
        }
        public static class NPCCheat
        {
            public static class GVars
            {
                public static void Update()
                {
                    string s = "";
                    Vector2 v;
                    Rectangle r;

                    #region ints
                    s = "/\\";
                    v = new Vector2(215f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.invasionType > 0 && Main.invasionType <= 3 && pressCD >= 5)
                        {
                            Main.invasionSize++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    s = "\\/";
                    v = new Vector2(215f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.invasionType > 0 && Main.invasionType <= 3 && pressCD >= 5)
                        {
                            if (Main.invasionSize > 0)
                            {
                                Main.invasionSize--;
                                if (Main.invasionSize == 0)
                                {
                                    NPC.downedGoblins |= Main.invasionType == 1;
                                    NPC.downedFrost |= Main.invasionType == 2;
                                    NPC.downedPirates |= Main.invasionType == 3;
                                    World.StopInvasion();
                                }
                            }
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }


                    s = "/\\";
                    v = new Vector2(365f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.pumpkinMoon && pressCD >= 5)
                        {
                            if (NPC.waveCount < 15)
                                NPC.waveCount++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    s = "\\/";
                    v = new Vector2(365f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.pumpkinMoon && pressCD >= 5)
                        {
                            if (NPC.waveCount > 0)
                                NPC.waveCount--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }


                    s = "/\\";
                    v = new Vector2(485f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.pumpkinMoon && pressCD >= 5)
                        {
                            NPC.waveKills++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    s = "\\/";
                    v = new Vector2(485f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && Main.pumpkinMoon && pressCD >= 5)
                        {
                            if (NPC.waveKills > 0)
                                NPC.waveKills--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    #endregion

                    #region invasions
                    s = "Defeated goblins: " + (NPC.downedGoblins ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 290f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedGoblins = !NPC.downedGoblins;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Frost Legion: " + (NPC.downedFrost ? "Yes" : "No");
                    v = new Vector2(290f, Main.screenHeight - 290f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedFrost = !NPC.downedFrost;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated pirates: " + (NPC.downedPirates ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 270f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedPirates = !NPC.downedPirates;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated clown: " + (NPC.downedClown ? "Yes" : "No");
                    v = new Vector2(290f, Main.screenHeight - 270f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedClown = !NPC.downedClown;
                            Main.PlaySound(12);
                        }
                    }
                    #endregion

                    #region bosses
                    s = "Defeated " + (WorldGen.crimson ? "Brain" : "Eye") + " of Ctulhu: " + (NPC.downedBoss1 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedBoss1 = !NPC.downedBoss1;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Eater of Worlds: " + (NPC.downedBoss2 ? "Yes" : "No");
                    v = new Vector2(330f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedBoss2 = !NPC.downedBoss2;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Skeletron: " + (NPC.downedBoss3 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedBoss3 = !NPC.downedBoss3;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Queen Bee: " + (NPC.downedQueenBee ? "Yes" : "No");
                    v = new Vector2(330f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedQueenBee = !NPC.downedQueenBee;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    s = "Defeated The Destroyer" + (NPC.downedMechBoss1 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedMechBoss1 = !NPC.downedMechBoss1;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated The Twins: " + (NPC.downedMechBoss2 ? "Yes" : "No");
                    v = new Vector2(350f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedMechBoss2 = !NPC.downedMechBoss2;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Skeletron Prime: " + (NPC.downedMechBoss3 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedMechBoss3 = !NPC.downedMechBoss3;
                            Main.PlaySound(12);
                        }
                    }

                    s = "Defeated Plantera: " + (NPC.downedPlantBoss ? "Yes" : "No");
                    v = new Vector2(350f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedPlantBoss = !NPC.downedPlantBoss;
                            Main.PlaySound(12);
                        }
                    }


                    s = "Defeated The Golem: " + (NPC.downedGolemBoss ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (mouse.Intersects(r))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            NPC.downedGolemBoss = !NPC.downedGolemBoss;
                            Main.PlaySound(12);
                        }
                    }
                    #endregion
                }
                public static void Draw()
                {
                    string s = "";
                    Vector2 v;
                    Rectangle r;

                    #region ints
                    s = "Invasion Size: " + (Main.invasionType != 0 ? Main.invasionSize : 0);
                    v = new Vector2(100f, Main.screenHeight - 350f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                    s = "Wave number: " + (Main.pumpkinMoon ? NPC.waveCount : 0);
                    v = new Vector2(250f, Main.screenHeight - 350f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                    s = "Wave kills: " + (Main.pumpkinMoon ? NPC.waveKills : 0);
                    v = new Vector2(400f, Main.screenHeight - 350f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                    // ---

                    s = "/\\";
                    v = new Vector2(215f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    s = "\\/";
                    v = new Vector2(215f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));


                    s = "/\\";
                    v = new Vector2(365f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    s = "\\/";
                    v = new Vector2(365f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));


                    s = "/\\";
                    v = new Vector2(485f, Main.screenHeight - 365f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    s = "\\/";
                    v = new Vector2(485f, Main.screenHeight - 330f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    #endregion

                    #region invasions
                    s = "Defeated goblins: " + (NPC.downedGoblins ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 290f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Frost Legion: " + (NPC.downedFrost ? "Yes" : "No");
                    v = new Vector2(290f, Main.screenHeight - 290f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated pirates: " + (NPC.downedPirates ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 270f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated clown: " + (NPC.downedClown ? "Yes" : "No");
                    v = new Vector2(290f, Main.screenHeight - 270f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    #endregion

                    #region bosses
                    s = "Defeated " + (WorldGen.crimson ? "Brain" : "Eye") + " of Ctulhu: " + (NPC.downedBoss1 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Eater of Worlds: " + (NPC.downedBoss2 ? "Yes" : "No");
                    v = new Vector2(330f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Skeletron: " + (NPC.downedBoss3 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Queen Bee: " + (NPC.downedQueenBee ? "Yes" : "No");
                    v = new Vector2(330f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    // ---

                    s = "Defeated The Destroyer" + (NPC.downedMechBoss1 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated The Twins: " + (NPC.downedMechBoss2 ? "Yes" : "No");
                    v = new Vector2(350f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Skeletron Prime: " + (NPC.downedMechBoss3 ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                    s = "Defeated Plantera: " + (NPC.downedPlantBoss ? "Yes" : "No");
                    v = new Vector2(350f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));


                    s = "Defeated The Golem: " + (NPC.downedGolemBoss ? "Yes" : "No");
                    v = new Vector2(100f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                    #endregion
                }
                public static void Init()
                {

                }
                public static void Close()
                {

                }
            }

            public static int netID = 0, amount = 0;
            public static bool dragging = false, canSpawnNPCs = true;

            public static void Update()
            {
                Rectangle r;
                Vector2 v;
                string s = "";

                int col = 0, row = 0, added = 0;

                if (pressCD < 8)
                    pressCD++;
                if (fromPress < 20)
                    fromPress++;



                s = "Global variables";
                v = new Vector2(100f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Close();
                        state = UIType.NPCVars;
                        GVars.Init();
                        Main.PlaySound(12);
                    }
                }

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && pressCD >= 8 && position > -48)
                    {
                        position -= 4;
                        if (position < -48)
                            position = -48;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && pressCD >= 8 && position + 20 < Main.maxNPCTypes)
                    {
                        position += 4;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region NPC list
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.npcs.Count)
                        break;
                    if (i < -65)
                        continue;
                    if (i == 0 || !Defs.npcs[Defs.npcNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (i != 0)
                        if (new Rectangle(160 + col * Main.inventoryBack11Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack11Texture.Height,
                            Main.inventoryBack11Texture.Width, Main.inventoryBack11Texture.Height).Intersects(mouse) && i != 0)
                        {
                            myP.mouseInterface = true;
                            if (mouseState.LeftButton == ButtonState.Pressed)
                            {
                                if (fromPress >= 10)
                                {
                                    if (netID == i)
                                        amount++;
                                    else
                                        amount = 1;
                                    fromPress = 0;
                                    dragging = true;
                                    netID = i;
                                    Main.PlaySound(12);
                                }
                            }
                        }
                    row++;
                }
                position -= added;
                #endregion

                #region jump to
                #region by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 74;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 147;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region by ID
                v = new Vector2(240f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("netID").X, (int)Main.fontMouseText.MeasureString("netID").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = -48;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(240f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1+").X, (int)Main.fontMouseText.MeasureString("1+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1;
                        Main.PlaySound(12);
                    }
                }

                #region 100+ - 300+
                v = new Vector2(240f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("100+").X, (int)Main.fontMouseText.MeasureString("100+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        position = 10;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(240f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("200+").X, (int)Main.fontMouseText.MeasureString("200+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        position = 200;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(240f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("300+").X, (int)Main.fontMouseText.MeasureString("300+").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        position = 300;
                        Main.PlaySound(12);
                    }
                }
                #endregion
                #endregion
                #endregion

                #region search
                if (keyState.IsKeyDown(Key.Return) && oldKeyState.IsKeyUp(Key.Return)) // if just pressed enter
                {
                    searching = !searching;
                    Main.clrInput();
                    if (!searching)
                        search = "";
                }
                if (searching)
                {
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Main.playerInventory = false;
                        return;
                    }
                    Main.editSign = true;
                    search = Main.GetInputText(search).ToLower();
                }
                #endregion

                #region if dragging
                if (dragging && netID != 0)
                {
                    if (!myP.mouseInterface)
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                Vector2 p = mouse + Main.screenPosition;
                                p += new Vector2(
                                    (float)Math.Cos(Main.rand.Next(628) / 100d) * Main.rand.Next(amount * 10),
                                    (float)Math.Sin(Main.rand.Next(628) / 100d) * Main.rand.Next(amount * 10));
                                Main.npc[NPC.NewNPC((int)p.X, (int)p.Y, 1)].netDefaults(netID);
                            }
                            Main.PlaySound(12);
                            dragging = false;
                            netID = amount = 0;
                        }
                    }
                }
                #endregion

                #region disable/enable spawns
                s = (canSpawnNPCs ? "Disable" : "Enable") + " Spawns";
                v = new Vector2(260f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        canSpawnNPCs = !canSpawnNPCs;
                        Main.PlaySound(12);
                    }
                }
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;
                string s = "";

                int col = 0, row = 0, added = 0;
                bool mouse = false;
                string mText = "";

                s = "Global variables";
                v = new Vector2(100f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                #region NPC list
                #region background tiles
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.npcs.Count)
                        break;
                    if (i < -65)
                        continue;
                    if (i == 0 || !Defs.npcs[Defs.npcNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    sb.Draw(Main.inventoryBack7Texture,
                        new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                                    (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height), alpha(MenuThemes.BgClr, 150));
                    row++;
                }
                position -= added;
                #endregion
                col = row = added = 0;
                #region npcs
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.npcs.Count)
                        break;
                    if (i < -65)
                        continue;
                    if (i == 0 || !Defs.npcs[Defs.npcNames[i]].name.ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (i != 0)
                        sb.Draw(Main.npcTexture[Defs.npcs[Defs.npcNames[i]].type],
                            new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                                        (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height)
                            + (new Vector2(Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height) / 2f
                            - new Vector2(Main.npcTexture[Defs.npcs[Defs.npcNames[i]].type].Width, Main.npcTexture[Defs.npcs[Defs.npcNames[i]].type].Height / Main.npcFrameCount[Defs.npcs[Defs.npcNames[i]].type]) / 2f),
                            new Rectangle(0, 0, Main.npcTexture[Defs.npcs[Defs.npcNames[i]].type].Width, Main.npcTexture[Defs.npcs[Defs.npcNames[i]].type].Height / Main.npcFrameCount[Defs.npcs[Defs.npcNames[i]].type]),
                            Defs.npcs[Defs.npcNames[i]].color == Color.Transparent ? Color.White : Defs.npcs[Defs.npcNames[i]].GetColor(Color.White));

                    if (new Rectangle(160 + col * Main.inventoryBack7Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack7Texture.Height,
                        Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height).Intersects(UI.mouse) && i != 0)
                    {
                        mouse = true;
                        mText = Defs.npcs[Defs.npcNames[i]].name;
                        if (Defs.npcs[Defs.npcNames[i]].lifeMax != 1)
                            mText += " (" + Defs.npcs[Defs.npcNames[i]].lifeMax + " max life)";
                    }
                    row++;
                }
                position -= added;
                #endregion
                #endregion

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                DrawOutlinedString("<", v, clr(r, MenuThemes.ForeClr), 2f);

                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                DrawOutlinedString(">", v, clr(r, MenuThemes.ForeClr), 2f);
                #endregion

                #region jump to
                #region by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                DrawOutlinedString("1.0", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                DrawOutlinedString("1.1", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                DrawOutlinedString("1.2", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region by ID
                v = new Vector2(240f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("netID").X, (int)Main.fontMouseText.MeasureString("netID").Y);
                DrawOutlinedString("netID", v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(240f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1+").X, (int)Main.fontMouseText.MeasureString("1+").Y);
                DrawOutlinedString("1+", v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(240f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("100+").X, (int)Main.fontMouseText.MeasureString("100+").Y);
                DrawOutlinedString("100+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(240f, Main.screenHeight - 80f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("200+").X, (int)Main.fontMouseText.MeasureString("200+").Y);
                DrawOutlinedString("200+", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(240f, Main.screenHeight - 60f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("300+").X, (int)Main.fontMouseText.MeasureString("300+").Y);
                DrawOutlinedString("300+", v, clr(r, MenuThemes.ForeClr));
                #endregion
                #endregion

                #region search
                if (search != "")
                    DrawOutlinedString(search, new Vector2(200f, Main.screenHeight - 400f), MenuThemes.ForeClr);
                #endregion

                #region disable/enable spawns
                s = (canSpawnNPCs ? "Disable" : "Enable") + " Spawns";
                v = new Vector2(260f, Main.screenHeight - 400f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region amount number
                if (amount > 0 && dragging)
                    DrawOutlinedString("Amount: " + amount, new Vector2(430f, Main.screenHeight - 350f), MenuThemes.ForeClr);
                #endregion

                if (mouse)
                    mainInstance.MouseText(mText);
            }
            public static void Init()
            {
                position = -65;
                dragging = false;
                netID = amount = fromPress = 0;
                searching = false;
                search = "";
                Main.clrInput();
            }
            public static void Close()
            {
                position = 1;
                dragging = false;
                netID = amount = fromPress = 0;
                searching = false;
                search = "";
                Main.clrInput();
            }
        }
        public static class BuffCheat
        {
            public static int ticks;

            public static void Update()
            {
                Rectangle r;
                Vector2 v;
                string s;

                int col = 0, row = 0, added = 0;

                if (pressCD < 8)
                    pressCD++;

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && pressCD >= 8 && position > -48)
                    {
                        position -= 4;
                        if (position < 1)
                            position = 1;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && pressCD >= 8 && position + 20 < Main.maxBuffs)
                    {
                        position += 4;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region buff list
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.buffs.Count)
                        break;
                    if (i < 1)
                        continue;
                    if (!Defs.buffNames[i].ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (i != 0)
                        if (new Rectangle(160 + col * Main.inventoryBack11Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack11Texture.Height,
                            Main.inventoryBack11Texture.Width, Main.inventoryBack11Texture.Height).Intersects(mouse) && i != 0)
                        {
                            myP.mouseInterface = true;
                            if (justClicked)
                            {
                                myP.AddBuff(i, ticks);
                                Main.PlaySound(12);
                            }
                        }
                    row++;
                }
                position -= added;
                #endregion

                #region jump to - by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 1;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 22;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        searching = false;
                        search = "";
                        Main.clrInput();
                        position = 41;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region set length
                s = "/\\";
                v = new Vector2(200f, Main.screenHeight - 425f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                    {
                        ticks += 3600;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(230f, Main.screenHeight - 425f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                    {
                        ticks += 60;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                s = "\\/";
                v = new Vector2(200f, Main.screenHeight - 395f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                    {
                        ticks -= 3600;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                v = new Vector2(230f, Main.screenHeight - 395f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                    {
                        ticks -= 60;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region search
                if (keyState.IsKeyDown(Key.Return) && oldKeyState.IsKeyUp(Key.Return)) // if just pressed enter
                {
                    searching = !searching;
                    Main.clrInput();
                    if (!searching)
                        search = "";
                }
                if (searching)
                {
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Main.playerInventory = false;
                        return;
                    }
                    Main.editSign = true;
                    search = Main.GetInputText(search).ToLower();
                }
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;
                string s;

                int col = 0, row = 0, added = 0;
                bool mouse = false;
                string mText = "";

                #region buff list
                #region background tiles
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.buffs.Count)
                        break;
                    if (i < 1)
                        continue;
                    if (!Defs.buffNames[i].ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    sb.Draw(Main.inventoryBack7Texture, new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                            (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height), alpha(MenuThemes.BgClr, 150));
                    row++;
                }
                position -= added;
                #endregion
                col = row = added = 0;
                #region buffs
                for (int i = position; i < position + 20; i++)
                {
                    if (i >= Defs.buffs.Count)
                        break;
                    if (i < 1)
                        continue;
                    if (row >= 4)
                    {
                        col++;
                        row = 0;
                    }
                    if (!Defs.buffNames[i].ToLower().Contains(search))
                    {
                        added++;
                        position++;
                        continue;
                    }
                    if (i != 0)
                        sb.Draw(Main.buffTexture[i],
                            new Vector2(160f + col * Main.inventoryBack7Texture.Width,
                                        (Main.screenHeight - 350f) + row * Main.inventoryBack7Texture.Height)
                            + (new Vector2(Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height) / 2f
                            - new Vector2(Main.buffTexture[i].Width, Main.buffTexture[i].Height) / 2f), null, Color.White);
                    if (new Rectangle(160 + col * Main.inventoryBack7Texture.Width, (Main.screenHeight - 350) + row * Main.inventoryBack7Texture.Height,
                        Main.inventoryBack7Texture.Width, Main.inventoryBack7Texture.Height).Intersects(new Vector2(Main.mouseX, Main.mouseY)) && i != 0)
                    {
                        mouse = true;
                        mText = Defs.buffNames[i] + "\n" + (i < Main.buffTip.Length ? Main.buffTip[i] : "No data");
                    }
                    row++;
                }
                position -= added;
                #endregion
                #endregion

                #region go up/down
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("<").X * 2, (int)Main.fontMouseText.MeasureString("<").Y * 2);
                DrawOutlinedString("<", v, clr(r, MenuThemes.ForeClr), 2f);

                v = new Vector2(380f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(">").X * 2, (int)Main.fontMouseText.MeasureString(">").Y * 2);
                DrawOutlinedString(">", v, clr(r, MenuThemes.ForeClr), 2f);
                #endregion

                #region jump to - by version
                v = new Vector2(200f, Main.screenHeight - 140f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.0").X, (int)Main.fontMouseText.MeasureString("1.0").Y);
                DrawOutlinedString("1.0", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.1").X, (int)Main.fontMouseText.MeasureString("1.1").Y);
                DrawOutlinedString("1.1", v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(200f, Main.screenHeight - 100f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString("1.2").X, (int)Main.fontMouseText.MeasureString("1.2").Y);
                DrawOutlinedString("1.2", v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region set length
                s = (ticks / 3600) + "m";
                v = new Vector2(200f, Main.screenHeight - 410f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = ((ticks % 3600) / 60) + "s";
                v = new Vector2(230f, Main.screenHeight - 410f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "/\\";
                v = new Vector2(200f, Main.screenHeight - 425f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(230f, Main.screenHeight - 425f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "\\/";
                v = new Vector2(200f, Main.screenHeight - 395f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                v = new Vector2(230f, Main.screenHeight - 395f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region search
                if (search != "")
                    DrawOutlinedString(search, new Vector2(250f, Main.screenHeight - 130f), MenuThemes.ForeClr);
                #endregion

                if (mouse)
                    mainInstance.MouseText(mText);
            }
            public static void Init()
            {
                position = 1;
                ticks = 3600;
                searching = false;
                search = "";
                Main.clrInput();
            }
            public static void Close()
            {
                position = 1;
                ticks = 0;
                searching = false;
                search = "";
                Main.clrInput();
            }
        }
        public static class PlayerCheat
        {
            public static class CharMod
            {
                public static void Update()
                {
                    string s;
                    Rectangle r;
                    Vector2 v;

                    if (pressCD < 8)
                        pressCD++;

                    s = "Gender: " + (myP.male ? "Male" : "Female");
                    v = new Vector2(420f, Main.screenHeight - 320f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            myP.male = !myP.male;
                            Main.PlaySound(myP.male ? 1 : 20);
                        }
                    }

                    #region increase
                    s = "/\\";
                    v = new Vector2(200f, Main.screenHeight - 340f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            myP.hair++;
                            if (myP.hair >= Main.maxHair)
                                myP.hair = 0;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // hair
                    v = new Vector2(160f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.R < 255)
                                myP.hairColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.G < 255)
                                myP.hairColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.B < 255)
                                myP.hairColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // skin
                    v = new Vector2(420f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.R < 255)
                                myP.skinColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.G < 255)
                                myP.skinColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.B < 255)
                                myP.skinColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    // shirt
                    v = new Vector2(160f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.R < 255)
                                myP.shirtColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.G < 255)
                                myP.shirtColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.B < 255)
                                myP.shirtColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // undershirt
                    v = new Vector2(420f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.R < 255)
                                myP.underShirtColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.G < 255)
                                myP.underShirtColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.B < 255)
                                myP.underShirtColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    // pants
                    v = new Vector2(160f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.R < 255)
                                myP.pantsColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.G < 255)
                                myP.pantsColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.B < 255)
                                myP.pantsColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // shoes
                    v = new Vector2(420f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.R < 255)
                                myP.shoeColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.G < 255)
                                myP.shoeColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.B < 255)
                                myP.shoeColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // eye
                    v = new Vector2(160f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.R < 255)
                                myP.eyeColor.R++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.G < 255)
                                myP.eyeColor.G++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.B < 255)
                                myP.eyeColor.B++;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    #endregion

                    #region decrease
                    s = "\\/";
                    v = new Vector2(200f, Main.screenHeight - 300f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            myP.hair--;
                            if (myP.hair < 0)
                                myP.hair = Main.maxHair - 1;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    // hair
                    v = new Vector2(160f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.R > 0)
                                myP.hairColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.G > 0)
                                myP.hairColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.hairColor.B > 0)
                                myP.hairColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // skin
                    v = new Vector2(420f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.R > 0)
                                myP.skinColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.G > 0)
                                myP.skinColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.skinColor.B > 0)
                                myP.skinColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    // shirt
                    v = new Vector2(160f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.R > 0)
                                myP.shirtColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.G > 0)
                                myP.shirtColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shirtColor.B > 0)
                                myP.shirtColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // undershirt
                    v = new Vector2(420f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.R > 0)
                                myP.underShirtColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.G > 0)
                                myP.underShirtColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.underShirtColor.B > 0)
                                myP.underShirtColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // ---

                    // pants
                    v = new Vector2(160f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.R > 0)
                                myP.pantsColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.G > 0)
                                myP.pantsColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.pantsColor.B > 0)
                                myP.pantsColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // shoes
                    v = new Vector2(420f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.R > 0)
                                myP.shoeColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(440f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.G > 0)
                                myP.shoeColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(460f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.shoeColor.B > 0)
                                myP.shoeColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }

                    // eye
                    v = new Vector2(160f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.R > 0)
                                myP.eyeColor.R--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(180f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.G > 0)
                                myP.eyeColor.G--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    v = new Vector2(200f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    if (r.Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 8)
                        {
                            if (myP.eyeColor.B > 0)
                                myP.eyeColor.B--;
                            pressCD = 0;
                            Main.PlaySound(12);
                        }
                    }
                    #endregion
                }
                public static void Draw()
                {
                    string s;
                    Rectangle r;
                    Vector2 v;

                    #region description
                    s = "Hair style: ";
                    v = new Vector2(160f, Main.screenHeight - 320f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.hairColor);
                    v += new Vector2(185f, 0f);
                    sb.Draw(Main.playerHairTexture[myP.hair], v, new Rectangle(0, 6, Main.playerHairTexture[myP.hair].Width, 30), myP.hairColor,
                        0f, Vector2.Zero, 1f, myP.direction == 1f ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

                    s = "Gender: " + (myP.male ? "Male" : "Female");
                    v = new Vector2(420f, Main.screenHeight - 320f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, myP.male ? Color.Blue : Color.Red));


                    s = "Hair Color: " + myP.hairColor.ToHexString();
                    v = new Vector2(160f, Main.screenHeight - 260f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.hairColor);
                    v += new Vector2(190f, 0f);
                    sb.Draw(panel, v, myP.hairColor);

                    s = "Skin Color: " + myP.skinColor.ToHexString();
                    v = new Vector2(420f, Main.screenHeight - 260f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.skinColor);
                    v += new Vector2(240f, 0f);
                    sb.Draw(panel, v, myP.skinColor);


                    s = "Shirt Color: " + myP.shirtColor.ToHexString();
                    v = new Vector2(160f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.shirtColor);
                    v += new Vector2(190f, 0f);
                    sb.Draw(panel, v, myP.shirtColor);

                    s = "Undershirt Color: " + myP.underShirtColor.ToHexString();
                    v = new Vector2(420f, Main.screenHeight - 200f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.underShirtColor);
                    v += new Vector2(240f, 0f);
                    sb.Draw(panel, v, myP.underShirtColor);


                    s = "Pants Color: " + myP.pantsColor.ToHexString();
                    v = new Vector2(160f, Main.screenHeight - 140f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.pantsColor);
                    v += new Vector2(190f, 0f);
                    sb.Draw(panel, v, myP.pantsColor);

                    s = "Shoe Color: " + myP.shoeColor.ToHexString();
                    v = new Vector2(420f, Main.screenHeight - 140f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.shoeColor);
                    v += new Vector2(240f, 0f);
                    sb.Draw(panel, v, myP.shoeColor);

                    s = "Eye Color: " + myP.eyeColor.ToHexString();
                    v = new Vector2(160f, Main.screenHeight - 80f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, myP.eyeColor);
                    v += new Vector2(240f, 0f);
                    sb.Draw(panel, v, myP.eyeColor);
                    #endregion

                    #region increase
                    s = "/\\";

                    v = new Vector2(200f, Main.screenHeight - 340f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, myP.hairColor));

                    // hair
                    v = new Vector2(160f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // skin
                    v = new Vector2(420f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 280f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // ---

                    // shirt
                    v = new Vector2(160f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // undershirt
                    v = new Vector2(420f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 220f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // ---

                    // pants
                    v = new Vector2(160f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // shoes
                    v = new Vector2(420f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 160f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // eye
                    v = new Vector2(160f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 100f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));
                    #endregion

                    #region decrease
                    s = "\\/";

                    v = new Vector2(200f, Main.screenHeight - 300f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, myP.hairColor));

                    // ---

                    // hair
                    v = new Vector2(160f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // skin
                    v = new Vector2(420f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 240f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // ---

                    // shirt
                    v = new Vector2(160f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // undershirt
                    v = new Vector2(420f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 180f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // ---

                    // pants
                    v = new Vector2(160f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // shoes
                    v = new Vector2(420f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(440f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(460f, Main.screenHeight - 120f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));

                    // eye
                    v = new Vector2(160f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Red));
                    v = new Vector2(180f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Green));
                    v = new Vector2(200f, Main.screenHeight - 60f);
                    r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                    DrawOutlinedString(s, v, clr(r, Color.Blue));
                    #endregion
                }
                public static void Init()
                {

                }
                public static void Close()
                {

                }
            }

            public const int Dustiness = 18, Radius = 5;
            public const float Speediness = 0.3f;

            public static bool invincible = false, noClip = false, oldMouse = false, freeze = false;

            public static Vector2 newV = Vector2.Zero, oldP = Vector2.Zero;
            public static int[] CD = new int[2] { 0, 0 };
            public static int oldStack = 0, addMinions = 0; // limit is set in Player.cs

            public static void Update()
            {
                Rectangle r;
                Vector2 v;
                string s;

                if (pressCD < 5)
                    pressCD++;

                #region actions/bools
                s = "Heal!";
                v = new Vector2(160f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        int
                            l = myP.statLifeMax - myP.statLife,
                            m = myP.statManaMax2 - myP.statMana;

                        myP.statLife += l;
                        myP.statMana += m;

                        myP.HealEffect(l);
                        myP.ManaEffect(m);

                        for (int i = 0; i < myP.buffTime.Length; i++)
                            if (Main.debuff[myP.buffType[i]] && myP.buffTime[i] > 0)
                                myP.buffTime[i] = myP.buffType[i] = 0;

                        Main.PlaySound(12);
                    }
                }

                s = "Invincibility: " + (invincible ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        invincible = !invincible;
                        Main.PlaySound(12);
                    }
                }
                s = "Noclip: " + (noClip ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        noClip = !noClip;
                        Main.PlaySound(12);
                    }
                }
                s = "Freeze NPCs: " + (freeze ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        freeze = !freeze;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                s = "Difficulty: " + (myP.difficulty == 0 ? "Softcore" : (myP.difficulty == 1 ? "Mediumcore" : "Hardcore"));
                v = new Vector2(160f, Main.screenHeight - 230f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        myP.difficulty++;
                        if (myP.difficulty > 2)
                            myP.difficulty = 0;
                        Main.PlaySound(12);
                    }
                }

                #region increase
                s = "+";

                v = new Vector2(300f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        myP.statLife = ++myP.statLifeMax;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(300f, Main.screenHeight - 260f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        myP.statMana = ++myP.statManaMax;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(300f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        addMinions++;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region decrease
                s = "-";

                v = new Vector2(320f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed && myP.statLifeMax > 1)
                    {
                        myP.statLife = --myP.statLifeMax;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(320f, Main.screenHeight - 260f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed && myP.statManaMax > 0)
                    {
                        myP.statMana = --myP.statManaMax;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(320f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (pressCD >= 5 && mouseState.LeftButton == ButtonState.Pressed && addMinions > 0)
                    {
                        addMinions--;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region character customization
                s = "Customize " + myP.name;
                v = new Vector2(160f, Main.screenHeight - 180f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)(Main.fontMouseText.MeasureString(s).X * 1.5f), (int)(Main.fontMouseText.MeasureString(s).Y * 1.5f));
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Close();
                        state = UIType.CharMod;
                        CharMod.Init();
                        Main.PlaySound(12);
                    }
                }
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;
                string s;
                bool gText = false;

                #region actions/bools
                s = "Heal!";
                v = new Vector2(160f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Invincibility: " + (invincible ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                if (r.Intersects(mouse))
                    gText = true;
                s = "Noclip: " + (noClip ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                if (r.Intersects(mouse))
                    gText = true;
                s = "Freeze: " + (freeze ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 330f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region description
                s = "Max life: " + myP.statLifeMax;
                v = new Vector2(160f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "Max mana: " + myP.statManaMax2;
                v = new Vector2(160f, Main.screenHeight - 260f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "Difficulty: " + (myP.difficulty == 0 ? "Softcore" : (myP.difficulty == 1 ? "Mediumcore" : "Hardcore"));
                v = new Vector2(160f, Main.screenHeight - 230f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "Max minions: " + (myP.maxMinions + addMinions);
                v = new Vector2(160f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));
                #endregion

                #region increase
                s = "+";

                v = new Vector2(300f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(300f, Main.screenHeight - 260f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(300f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region decrease
                s = "-";

                v = new Vector2(320f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(320f, Main.screenHeight - 260f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                v = new Vector2(320f, Main.screenHeight - 200f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region character customization
                s = "Customize " + myP.name;
                v = new Vector2(160f, Main.screenHeight - 180f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)(Main.fontMouseText.MeasureString(s).X * 1.5f), (int)(Main.fontMouseText.MeasureString(s).Y * 1.5f));
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);
                #endregion

                if (gText)
                    mainInstance.MouseText("If you have both noclip and invincibility enabled:\n    Press V to teleport to the cursor,"
                        + "\n    Press J to fly very fast towards the cursor\n    Press K to kill every evil NPC/Projectile which is active"
                        + "\n    Press L to explode everything around your cursor\n    Press P to purify everything around your cursor");
            }
            public static void Init()
            {

            }
            public static void Close()
            {

            }

            public static void GMUpdate()
            {
                Vector2
                    PC = myP.position + new Vector2(myP.width / 2f, myP.height / 2f),
                    MO = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                for (int i = 0; i < CD.Length; i++)
                    if (CD[i] <= 15)
                        CD[i]++;
                #region invincible
                if (invincible)
                {
                    myP.immuneAlpha = 0;
                    myP.potionDelay = 0;
                    myP.lavaCD = 3;
                    myP.gravDir = 1f;
                    myP.immuneTime = 60;

                    myP.suffocating = false;
                    myP.slippy = false;
                    myP.slippy2 = false;
                    myP.powerrun = false;
                    myP.poisoned = false;
                    myP.venom = false;
                    myP.onFire = false;
                    myP.burned = false;
                    myP.suffocating = false;
                    myP.onFire2 = false;
                    myP.ichor = false;
                    myP.blackout = false;
                    myP.burned = false;
                    myP.onFrostBurn = false;
                    myP.blind = false;
                    myP.blackout = false;
                    myP.noItems = false;
                    myP.immune = true;
                    myP.lavaImmune = true;
                    myP.noKnockback = true;
                    for (int i = 0; i < 10; i++)
                        if ((myP.buffType[i] == 30 || myP.buffType[i] == 22 || myP.buffType[i] == 36 || myP.buffType[i] == 31
                            || myP.buffType[i] == 35 || myP.buffType[i] == 32) && myP.buffTime[i] > 0)
                            myP.buffTime[i] = myP.buffType[i] = 0;
                    if (myP.statLife < myP.statLifeMax)
                        myP.statLife = myP.statLifeMax;
                    if (myP.statMana < myP.statManaMax2)
                        myP.statMana = myP.statManaMax2;
                    myP.breath = myP.breathMax - 1;
                }
                #endregion
                #region noClip
                if (noClip)
                {
                    myP.fallStart = (int)(myP.position.Y / 16f);
                    myP.gravControl = false;

                    // rest is in postplayer

                    //myP.position -= myP.oldVelocity;
                    //myP.position -= myP.velocity;
                    //myP.velocity = new Vector2(0, -0.0000002f);
                    //float abc = 75f;
                    //if (myP.controlDown)
                    //    newV.Y += abc;
                    //if (myP.controlUp)
                    //    newV.Y -= abc;
                    //if (myP.controlLeft)
                    //    newV.X -= abc;
                    //if (myP.controlRight)
                    //    newV.X += abc;
                    //if (myP.position - newV / 50f != myP.oldPosition)
                    //{
                    //    myP.position -= myP.position - myP.oldPosition;
                    //    myP.position += newV / 25f;
                    //}
                    //else
                    //    myP.position += newV / 50f;
                    //newV *= 0.75f;
                }
                #endregion
                #region both
                if (invincible && noClip)
                {
                    #region passive effects
                    // infinite ammo is in Player.cs
                    // freezing stuff is in Main.cs

                    Lighting.AddLight((int)myP.position.X, (int)myP.position.Y, 1.2f, 1.2f, 1.2f);
                    if (!freeze)
                        Main.dust[Dust.NewDust(myP.position, myP.width, myP.height, 57, myP.velocity.X * 3, myP.velocity.Y * 3 * myP.direction, 100, new Color(), 1.5f)].noGravity = true;
                    myP.accCompass = myP.accDepthMeter = 1;
                    myP.accWatch = 3;
                    myP.blockRange = 40;
                    myP.AddBuff(9, 60);
                    myP.AddBuff(17, 60);
                    myP.AddBuff(12, 60);
                    myP.pickSpeed = 0.000001f;
                    if (Main.mouseItem.type == 0)
                    {
                        if (myP.inventory[myP.selectedItem].stack < oldStack && myP.selectedItem == myP.oldSelectItem && !oldMouse && oldStack <= myP.inventory[myP.selectedItem].maxStack)
                            myP.inventory[myP.selectedItem].stack = oldStack;
                        oldStack = myP.inventory[myP.selectedItem].stack;
                        oldMouse = false;
                    }
                    else
                    {
                        if (Main.mouseItem.stack < oldStack && oldMouse && oldStack <= Main.mouseItem.maxStack)
                            Main.mouseItem.stack = oldStack;
                        oldStack = Main.mouseItem.stack;
                        oldMouse = true;
                    }
                    //myP.blockRange += 40;
                    myP.tileRangeX = 50;
                    myP.tileRangeY = 30;

                    #endregion
                    if (!searching && !Main.editSign && !Main.chatMode)
                    {
                        #region V
                        if (keyState.IsKeyDown(Key.V) && CD[0] > 15)
                        {
                            CD[0] = 0;
                            Main.PlaySound(2, (int)myP.position.X, (int)myP.position.Y, 29);
                            for (int num89 = 0; num89 < 50; num89++)
                                Dust.NewDust(myP.position, myP.width, myP.height, 76, (float)Math.Cos(Main.rand.Next(628) / 100d), (float)Math.Cos(Main.rand.Next(628) / 100d), 150, default(Color), 2f);
                            myP.position = MO + new Vector2(myP.width / 2, myP.height / 2);
                            //Main.NewText("Teleported to Cursor: " + myP.position.X + ", " + myP.position.Y, 175, 75, 255);
                            for (int num91 = 0; num91 < 50; num91++)
                                Main.dust[Dust.NewDust(myP.position, myP.width, myP.height, 64, (float)Math.Cos(Main.rand.Next(628) / 100d) * 10f, (float)Math.Sin(Main.rand.Next(628) / 100d) * 20f, 150, default(Color), 6f)].noGravity = true;
                            Main.PlaySound(2, (int)myP.position.X, (int)myP.position.Y, 29);
                        }
                        #endregion
                        #region J
                        if (keyState.IsKeyDown(Key.J))
                        {
                            myP.position = MO + new Vector2(myP.width / 2, myP.height / 2);
                            for (int zzz = 0; zzz < Dustiness; zzz++)
                                for (int zzb = 0; zzb < 2; zzb++)
                                {
                                    Dust D = Main.dust[Dust.NewDust(myP.position + new Vector2(-Radius, -Radius), (myP.width + 2 * Radius), (myP.height + 2 * Radius), 66, 0, 0, 100, default(Color), 2.5f)];
                                    D.velocity /= 10f;
                                    D.velocity += ((myP.velocity) * zzz / 100f);
                                    D.noGravity = true;
                                }
                        }
                        #endregion
                        #region K
                        if (keyState.IsKeyDown(Key.K) && CD[0] > 15)
                        {
                            CD[0] = 0;
                            foreach (NPC N in Main.npc)
                            {
                                if (N.active && !N.friendly)
                                {
                                    N.NPCLoot();
                                    N.HitEffect(myP.direction, 50);
                                    Main.PlaySound(4, (int)N.position.X, (int)N.position.Y, N.soundKilled);
                                    //Main.NewText("Killed NPC: " + N.name, 175, 75, 255);
                                    N.active = false;
                                }
                            }
                            foreach (Projectile Pr in Main.projectile)
                                if (Pr.active && (!Pr.friendly || Pr.hostile))
                                {
                                    Pr.Kill();
                                    //Main.NewText("Stopped Projectile: " + Pr.name, 175, 75, 255);
                                }
                            //foreach (Player eP in Main.player)
                            //{
                            //    if (eP.active && eP.hostile && eP.whoAmi != myP.whoAmi)
                            //    {
                            //        if (eP.team != myP.team)
                            //            eP.KillMe(9001, 1, true, " tried to impale God (aka " + myP.name + ") ...");
                            //        else if (eP.team == myP.team)
                            //        {
                            //            int
                            //                healAmt = eP.statLifeMax == 0 ? eP.statLifeMax - eP.statLife : eP.statLifeMax - eP.statLife,
                            //                manaAmt = eP.statManaMax2 == 0 ? eP.statManaMax - eP.statMana : eP.statManaMax2 - eP.statMana;
                            //            eP.statLife += healAmt;
                            //            eP.HealEffect(healAmt);
                            //            eP.statMana += manaAmt;
                            //            eP.ManaEffect(manaAmt);
                            //            eP.immuneTime = 60;
                            //            //Main.NewText("Healed Player: " + eP.name + ": " + healAmt + ", " + manaAmt, 175, 75, 255);
                            //        }
                            //    }
                            //}
                        }
                        #endregion
                        #region L
                        if (keyState.IsKeyDown(Key.L) && CD[1] > 5)
                        {
                            CD[1] = 0;
                            Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 108, 500, 10f, Main.myPlayer);
                        }
                        #endregion
                        #region P
                        if (keyState.IsKeyDown(Key.P) && CD[1] > 5)
                        {
                            CD[1] = 0;
                            Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 10, 0, 0, Main.myPlayer);
                            Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 11, 0, 0, Main.myPlayer);
                        }
                        #endregion
                        #region Y
                        if (keyState.IsKeyDown(Key.Y) && CD[0] > 15)
                        {
                            freeze = !freeze;
                            CD[0] = 0;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            public static void GMPostPlayer()
            {
                // called after player updating
                Vector2
                    PC = myP.position + new Vector2(myP.width / 2f, myP.height / 2f),
                    MO = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                #region invincible
                if (invincible)
                {
                    myP.suffocating = false;
                    myP.slippy = false;
                    myP.slippy2 = false;
                    myP.powerrun = false;
                    myP.poisoned = false;
                    myP.venom = false;
                    myP.onFire = false;
                    myP.burned = false;
                    myP.suffocating = false;
                    myP.onFire2 = false;
                    myP.ichor = false;
                    myP.blackout = false;
                    myP.burned = false;
                    myP.onFrostBurn = false;
                    myP.blind = false;
                    myP.blackout = false;
                    myP.noItems = false;
                    myP.immune = true;
                    myP.lavaImmune = true;
                    myP.noKnockback = true;
                    for (int i = 0; i < 10; i++)
                        if ((myP.buffType[i] == 30 || myP.buffType[i] == 22 || myP.buffType[i] == 36 || myP.buffType[i] == 31
                            || myP.buffType[i] == 35 || myP.buffType[i] == 32 || myP.buffType[i] == 44 || myP.buffType[i] == 46
                             || myP.buffType[i] == 47 || myP.buffType[i] == 67 || myP.buffType[i] == 68 || myP.buffType[i] == 69
                             || myP.buffType[i] == 70 || myP.buffType[i] == 80) && myP.buffTime[i] > 0)
                            myP.buffTime[i] = myP.buffType[i] = 0;
                    if (myP.statLife < myP.statLifeMax)
                        myP.statLife = myP.statLifeMax;
                    if (myP.statMana < myP.statManaMax2)
                        myP.statMana = myP.statManaMax2;
                    myP.breath = myP.breathMax;

                    myP.immuneAlpha = 0;
                    myP.potionDelay = 0;
                    myP.lavaCD = 3;
                    myP.gravDir = 1f;
                    myP.immuneTime = 60;
                }
                #endregion
                #region noClip
                if (noClip)
                {
                    myP.fallStart = (int)(myP.position.Y / 16f);
                    myP.gravControl = false;
                    myP.position -= myP.oldVelocity;
                    myP.position -= myP.velocity;
                    myP.velocity = new Vector2(0, -0.0000002f);
                    float abc = 75f;
                    if (myP.controlDown)
                        newV.Y += abc;
                    if (myP.controlUp)
                        newV.Y -= abc;
                    if (myP.controlLeft)
                        newV.X -= abc;
                    if (myP.controlRight)
                        newV.X += abc;
                    if (myP.position - newV / 50f != myP.oldPosition)
                    {
                        myP.position -= myP.position - myP.oldPosition;
                        myP.position += newV / 25f;
                    }
                    else
                        myP.position += newV / 50f;
                    newV *= 0.75f;
                }
                #endregion
                #region both
                if (invincible && noClip)
                {
                    #region passive effects
                    // infinite ammo is in Player.cs
                    // freezing stuff is in Main.cs

                    Lighting.AddLight((int)myP.position.X, (int)myP.position.Y, 1.2f, 1.2f, 1.2f);
                    if (!freeze)
                        Main.dust[Dust.NewDust(myP.position, myP.width, myP.height, 57, myP.velocity.X * 3, myP.velocity.Y * 3 * myP.direction, 100, new Color(), 1.5f)].noGravity = true;
                    myP.accCompass = myP.accDepthMeter = 1;
                    myP.accWatch = 3;
                    myP.blockRange = 40;
                    //myP.AddBuff(9, 60);
                    //myP.AddBuff(17, 60);
                    //myP.AddBuff(12, 60);
                    myP.pickSpeed = 0.000001f;
                    //if (Main.mouseItem.type == 0)
                    //{
                    //    if (myP.inventory[myP.selectedItem].stack < oldStack && myP.selectedItem == myP.oldSelectItem && !oldMouse && oldStack <= myP.inventory[myP.selectedItem].maxStack)
                    //        myP.inventory[myP.selectedItem].stack = oldStack;
                    //    oldStack = myP.inventory[myP.selectedItem].stack;
                    //    oldMouse = false;
                    //}
                    //else
                    //{
                    //    if (Main.mouseItem.stack < oldStack && oldMouse && oldStack <= Main.mouseItem.maxStack)
                    //        Main.mouseItem.stack = oldStack;
                    //    oldStack = Main.mouseItem.stack;
                    //    oldMouse = true;
                    //}
                    //myP.blockRange += 40;
                    //Player.tileRangeX = 50;
                    //Player.tileRangeY = 30;

                    #endregion
                    #region failed no lighting
                    //Main.eclipseLight = 0f;
                    //Lighting.brightness = 50f;
                    #endregion
                    #region V
                    //if (keyState.IsKeyDown(Key.V) && CD[0] > 15 && !searching)
                    //{
                    //    CD[0] = 0;
                    //    Main.PlaySound(2, (int)myP.position.X, (int)myP.position.Y, 29);
                    //    for (int num89 = 0; num89 < 50; num89++)
                    //        Dust.NewDust(myP.position, myP.width, myP.height, 76, (float)Math.Cos(Main.rand.Next(628) / 100d), (float)Math.Cos(Main.rand.Next(628) / 100d), 150, default(Color), 2f);
                    //    myP.position = MO + new Vector2(myP.width / 2, myP.height / 2);
                    //    //Main.NewText("Teleported to Cursor: " + myP.position.X + ", " + myP.position.Y, 175, 75, 255);
                    //    for (int num91 = 0; num91 < 50; num91++)
                    //        Main.dust[Dust.NewDust(myP.position, myP.width, myP.height, 64, (float)Math.Cos(Main.rand.Next(628) / 100d) * 10f, (float)Math.Sin(Main.rand.Next(628) / 100d) * 20f, 150, default(Color), 6f)].noGravity = true;
                    //    Main.PlaySound(2, (int)myP.position.X, (int)myP.position.Y, 29);
                    //}
                    #endregion
                    #region J
                    //if (keyState.IsKeyDown(Key.J) && !searching)
                    //{
                    //    myP.position = MO + new Vector2(myP.width / 2, myP.height / 2);
                    //    for (int zzz = 0; zzz < Dustiness; zzz++)
                    //        for (int zzb = 0; zzb < 2; zzb++)
                    //        {
                    //            Dust D = Main.dust[Dust.NewDust(myP.position + new Vector2(-Radius, -Radius), (myP.width + 2 * Radius), (myP.height + 2 * Radius), 66, 0, 0, 100, default(Color), 2.5f)];
                    //            D.velocity /= 10f;
                    //            D.velocity += ((myP.velocity) * zzz / 100f);
                    //            D.noGravity = true;
                    //        }
                    //}
                    #endregion
                    #region K
                    //if (keyState.IsKeyDown(Key.K) && CD[0] > 15 && !searching)
                    //{
                    //    CD[0] = 0;
                    //    foreach (NPC N in Main.npc)
                    //    {
                    //        if (N.active && !N.friendly)
                    //        {
                    //            N.NPCLoot();
                    //            N.HitEffect(myP.direction, 50);
                    //            Main.PlaySound(4, (int)N.position.X, (int)N.position.Y, N.soundKilled);
                    //            //Main.NewText("Killed NPC: " + N.name, 175, 75, 255);
                    //            N.active = false;
                    //        }
                    //    }
                    //    foreach (Projectile Pr in Main.projectile)
                    //        if (Pr.active && (!Pr.friendly || Pr.hostile))
                    //        {
                    //            Pr.Kill();
                    //            //Main.NewText("Stopped Projectile: " + Pr.name, 175, 75, 255);
                    //        }
                    //    //foreach (Player eP in Main.player)
                    //    //{
                    //    //    if (eP.active && eP.hostile && eP.whoAmi != myP.whoAmi)
                    //    //    {
                    //    //        if (eP.team != myP.team)
                    //    //            eP.KillMe(9001, 1, true, " tried to impale God (aka " + myP.name + ") ...");
                    //    //        else if (eP.team == myP.team)
                    //    //        {
                    //    //            int
                    //    //                healAmt = eP.statLifeMax == 0 ? eP.statLifeMax - eP.statLife : eP.statLifeMax - eP.statLife,
                    //    //                manaAmt = eP.statManaMax2 == 0 ? eP.statManaMax - eP.statMana : eP.statManaMax2 - eP.statMana;
                    //    //            eP.statLife += healAmt;
                    //    //            eP.HealEffect(healAmt);
                    //    //            eP.statMana += manaAmt;
                    //    //            eP.ManaEffect(manaAmt);
                    //    //            eP.immuneTime = 60;
                    //    //            //Main.NewText("Healed Player: " + eP.name + ": " + healAmt + ", " + manaAmt, 175, 75, 255);
                    //    //        }
                    //    //    }
                    //    //}
                    //}
                    #endregion
                    #region L
                    //if (keyState.IsKeyDown(Key.L) && CD[1] > 5 && !searching)
                    //{
                    //    CD[1] = 0;
                    //    Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 108, 500, 10f, Main.myPlayer);
                    //}
                    #endregion
                    #region P
                    //if (keyState.IsKeyDown(Key.P) && CD[1] > 5 && !searching)
                    //{
                    //    CD[1] = 0;
                    //    Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 10, 0, 0, Main.myPlayer);
                    //    Projectile.NewProjectile(MO.X, MO.Y, 0, 0, 11, 0, 0, Main.myPlayer);
                    //}
                    #endregion
                    #region Y
                    //if (keyState.IsKeyDown(Key.Y) && CD[0] > 15 && !searching)
                    //{
                    //    freeze = !freeze;
                    //    CD[0] = 0;
                    //}
                    #endregion
                }
                #endregion
            }
        }
        public static class WorldCheat
        {
            public static bool hMod = false, xMod = false, inited = false;

            public static void Update()
            {
                Rectangle r;
                Vector2 v;
                string s;

                if (pressCD < 10)
                    pressCD++;

                #region bools (hm, blood moon, etc)
                s = "Hardmode: " + (Main.hardMode ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Main.hardMode = !Main.hardMode;
                        Main.PlaySound(12);
                    }
                }

                s = "Blood moon: " + (Main.bloodMoon ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Main.bloodMoon = !Main.bloodMoon;
                        if (Main.bloodMoon)
                            Main.dayTime = false;
                        Main.PlaySound(12);
                    }
                }

                s = Main.dayTime ? "Day" : "Night";
                v = new Vector2(160f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Main.dayTime = !Main.dayTime;
                        Main.PlaySound(12);
                    }
                }

                s = "Eclipse: " + (Main.eclipse ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        Main.eclipse = !Main.eclipse;
                        if (Main.eclipse)
                            Main.dayTime = true;
                        Main.PlaySound(12);
                    }
                }

                s = "Raining: " + (Main.raining ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        if (!Main.raining)
                            Main.StartRain();
                        else
                            Main.StopRain();
                        Main.PlaySound(12);
                    }
                }

                s = "Drop a meteor!";
                v = new Vector2(300f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        WorldGen.dropMeteor();
                        //if (!WorldGen.dropMeteor())
                        //    Main.NewText("Could not drop a meteor...", 200, 50, 255);
                        Main.PlaySound(12);
                    }
                }

                s = "Christmas: " + (Main.xMas ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        xMod = !xMod;
                        // checkXMas is edited
                        Main.checkXMas();
                        Main.PlaySound(12);
                    }
                }

                s = "Halloween: " + (Main.halloween ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        hMod = !hMod;
                        Main.checkHalloween();
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region increase
                s = "+";

                v = new Vector2(300f, Main.screenHeight - 250f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 5)
                    {
                        Main.time += 200;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(300f, Main.screenHeight - 220f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 10)
                    {
                        Main.moonPhase += 1;
                        if (Main.moonPhase >= 8)
                            Main.moonPhase = 0;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(300f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 10)
                    {
                        Main.dayRate += 1;
                        if (Main.dayRate == 0)
                            Main.dayRate++;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region decrease
                s = "-";

                v = new Vector2(320f, Main.screenHeight - 250f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 5)
                    {
                        Main.time -= 200;
                        if (Main.time < 0)
                        {
                            Main.time = 0;
                            Main.dayTime = !Main.dayTime;
                        }
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(320f, Main.screenHeight - 220f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 10)
                    {
                        Main.moonPhase -= 1;
                        if (Main.moonPhase < 0)
                            Main.moonPhase = 7;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }

                v = new Vector2(320f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (mouseState.LeftButton == ButtonState.Pressed && pressCD >= 10)
                    {
                        Main.dayRate -= 1;
                        if (Main.dayRate == 0)
                            Main.dayRate--;
                        pressCD = 0;
                        Main.PlaySound(12);
                    }
                }
                #endregion

                #region invasion stuff
                s = "Goblin Army: " + (Main.invasionType == 1 ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        if (Main.invasionType == 1)
                            World.StopInvasion();
                        else
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(1);
                        }
                        Main.PlaySound(12);
                    }
                }
                s = "Frost Legion: " + (Main.invasionType == 2 ? "On" : "Off");
                v = new Vector2(320f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        if (Main.invasionType == 2)
                            World.StopInvasion();
                        else
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(2);
                        }
                        Main.PlaySound(12);
                    }
                }

                s = "Pirate invasion: " + (Main.invasionType == 3 ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        if (Main.invasionType == 3)
                            World.StopInvasion();
                        else
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(3);
                        }
                        Main.PlaySound(12);
                    }
                }
                s = "Pumpkin Moon: " + (Main.pumpkinMoon ? "On" : "Off");
                v = new Vector2(320f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                if (r.Intersects(mouse))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                    {
                        if (Main.pumpkinMoon)
                        {
                            Main.pumpkinMoon = false;
                            NPC.waveKills = NPC.waveCount = 0;
                        }
                        else
                        {
                            Main.dayTime = false;
                            Main.startPumpkinMoon();
                        }
                        Main.PlaySound(12);
                    }
                }
                #endregion
            }
            public static void Draw()
            {
                Rectangle r;
                Vector2 v;
                string s;

                #region bools (hm, blood moon, etc)
                s = "Hardmode: " + (Main.hardMode ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Blood moon: " + (Main.bloodMoon ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = Main.dayTime ? "Day" : "Night";
                v = new Vector2(160f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Eclipse: " + (Main.eclipse ? "On" : "Off");
                v = new Vector2(300f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Raining: " + (Main.raining ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Drop a meteor!";
                v = new Vector2(300f, Main.screenHeight - 290f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Christmas: " + (Main.xMas || xMod ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 350f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Halloween: " + (Main.halloween || hMod ? "On" : "Off");
                v = new Vector2(440f, Main.screenHeight - 320f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion

                #region descriptions
                s = "Time: " + (int)(Main.time);
                v = new Vector2(160f, Main.screenHeight - 250f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "Moon phase: " + Main.moonPhase;
                v = new Vector2(160f, Main.screenHeight - 220f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));

                s = "Time speed: " + Main.dayRate;
                v = new Vector2(160f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, Color.Lerp(MenuThemes.ForeClr, MenuThemes.BgClr, 0.333333f));
                #endregion

                #region increase
                s = "+";

                v = new Vector2(300f, Main.screenHeight - 250f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);

                v = new Vector2(300f, Main.screenHeight - 220f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);

                v = new Vector2(300f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);
                #endregion

                #region decrease
                s = "-";

                v = new Vector2(320f, Main.screenHeight - 250f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);

                v = new Vector2(320f, Main.screenHeight - 220f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);

                v = new Vector2(320f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr), 1.5f);
                #endregion

                #region invasion stuff
                s = "Goblin Army: " + (Main.invasionType == 1 ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                s = "Frost Legion: " + (Main.invasionType == 2 ? "On" : "Off");
                v = new Vector2(320f, Main.screenHeight - 150f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));

                s = "Pirate invasion: " + (Main.invasionType == 3 ? "On" : "Off");
                v = new Vector2(160f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                s = "Pumpkin Moon: " + (Main.pumpkinMoon ? "On" : "Off");
                v = new Vector2(320f, Main.screenHeight - 120f);
                r = new Rectangle((int)v.X, (int)v.Y, (int)Main.fontMouseText.MeasureString(s).X, (int)Main.fontMouseText.MeasureString(s).Y);
                DrawOutlinedString(s, v, clr(r, MenuThemes.ForeClr));
                #endregion
            }
            public static void Init()
            {
                if (!inited)
                {
                    Main.checkXMas();
                    Main.checkHalloween();
                    hMod = Main.halloween;
                    xMod = Main.xMas;
                }
            }
            public static void Close()
            {

            }
        }
        public static class MenuThemes
        {
            public enum BGColor : byte
            {
                Black = 0,
                White = 1
            }
            public enum ForeColor : byte
            {
                Blue = 0,
                Green = 1,
                Orange = 2
            }

            public static BGColor Theme = BGColor.Black;
            public static ForeColor Accent = ForeColor.Green;

            public static Color BgClr
            {
                get
                {
                    return Theme == BGColor.Black ? new Color(30, 30, 30) // pretty black but not black-black
                        : Color.White;
                }
            }
            public static Color ForeClr
            {
                get
                {
                    switch (Accent)
                    {
                        case ForeColor.Blue:
                            return new Color(32, 160, 224); // cobalt blue-ish
                        case ForeColor.Green:
                            return Color.LimeGreen; // neon green-ish
                        case ForeColor.Orange:
                            return Color.OrangeRed; // neon orange-ish
                    }
                    throw new ArgumentException("Wrong Accent value: " + Accent, "Accent");
                }
            }

            public static void Update()
            {
                Vector2 v;
                Rectangle r;

                #region background color
                v = new Vector2(300f, Main.screenHeight - 270f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                if (mouse.Intersects(r))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                        Theme = BGColor.Black;
                }

                v = new Vector2(326f, Main.screenHeight - 270f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                if (mouse.Intersects(r))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                        Theme = BGColor.White;
                }
                #endregion

                #region accent color
                v = new Vector2(300f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                if (mouse.Intersects(r))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                        Accent = ForeColor.Blue;
                }

                v = new Vector2(326f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                if (mouse.Intersects(r))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                        Accent = ForeColor.Green;
                }

                v = new Vector2(352f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                if (mouse.Intersects(r))
                {
                    myP.mouseInterface = true;
                    if (justClicked)
                        Accent = ForeColor.Orange;
                }
                #endregion
            }
            public static void Draw()
            {
                string s;
                Vector2 v;
                Rectangle r;

                #region background color
                s = "Background color: ";
                v = new Vector2(300f, Main.screenHeight - 300f);
                DrawOutlinedString(s, v, Color.Lerp(ForeClr, BgClr, 0.333333f));

                v = new Vector2(300f, Main.screenHeight - 270f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                sb.Draw(panel, v, clr(r, new Color(50, 50, 50)));
                v = new Vector2(326f, Main.screenHeight - 270f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                sb.Draw(panel, v, clr(r, Color.White));
                #endregion

                #region accent color
                s = "Accent color: ";
                v = new Vector2(300f, Main.screenHeight - 220f);
                DrawOutlinedString(s, v, Color.Lerp(ForeClr, BgClr, 0.333333f));

                v = new Vector2(300f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                sb.Draw(panel, v, clr(r, new Color(32, 160, 224)));

                v = new Vector2(326f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                sb.Draw(panel, v, clr(r, Color.LimeGreen));

                v = new Vector2(352f, Main.screenHeight - 190f);
                r = new Rectangle((int)v.X, (int)v.Y, panel.Width, panel.Height);
                sb.Draw(panel, v, clr(r, Color.OrangeRed));
                #endregion
            }
            public static void Init()
            {

            }
            public static void Close()
            {

            }
        }
        #endregion

        #region Fields
        public static int position = 1, pressCD = 0, fromPress = 0;
        public static KeyHandler keyState, oldKeyState;
        public static MouseState mouseState, oldMouseState;
        public static UIType state;
        public static Texture2D panel, item, npc;
        public static string search;
        public static bool searching, oldInv = false;

        public static Main mainInstance
        {
            get
            {
                return Constants.mainInstance;
            }
        }
        #endregion

        #region Properties
        public static SpriteBatch sb
        {
            get
            {
                return Constants.mainInstance.spriteBatch;
            }
        }
        public static Player myP
        {
            get
            {
                return Main.player[Main.myPlayer];
            }
            set
            {
                Main.player[Main.myPlayer] = value;
            }
        }
        public static Vector2 mouse
        {
            get
            {
                return new Vector2(Main.mouseX, Main.mouseY);
            }
        }
        public static bool justClicked
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
            }
        }
        #endregion

        #region Methods
        public static void Init()
        {
            if (Main.dedServ || Main.netMode != 0)
                return;

            state = UIType.None;

            panel = new Texture2D(mainInstance.GraphicsDevice, 24, 24);
            Color[] texels = new Color[24 * 24];
            for (int i = 0; i < 24 * 24; i++)
                texels[i] = Color.White;
            panel.SetData(texels);

            item = mainInstance.Content.Load<Texture2D>("Images\\Item_1");
            npc = mainInstance.Content.Load<Texture2D>("Images\\NPC_1");

            //for (int i = 0; i < (int)ItemCheat.Category.Count; i++)
            //    itemDefs.Add((ItemCheat.Category)i, new Dictionary<int, Item>());

            #region save data ids to file (commented)
            //using (FileStream fs = new FileStream("Terraria Data IDs.txt", FileMode.Create))
            //{
            //    using (StreamWriter w = new StreamWriter(fs))
            //    {
            //        w.WriteLine("\nItem IDs\n");
            //        I = new Item();
            //        for (int i = -48; i < Main.maxItemTypes; i++)
            //        {
            //            I.netDefaults(i);
            //            w.WriteLine(i + "|" + I.name);
            //        }

            //        w.WriteLine("\n\nNPC IDs\n");
            //        n = new NPC();
            //        for (int i = -65; i < Main.maxNPCTypes; i++)
            //        {
            //            n.netDefaults(i);
            //            w.WriteLine(i + "|" + n.name);
            //        }

            //        w.WriteLine("\n\nProjectile IDs\n");
            //        Projectile p = new Projectile();
            //        for (int i = 0; i < Main.maxProjectileTypes; i++)
            //        {
            //            p.SetDefaults(i);
            //            w.WriteLine(i + "|" + p.name);
            //        }

            //        w.WriteLine("\n\nBuff IDs\n");
            //        for (int i = 0; i < Main.maxBuffs; i++)
            //            w.WriteLine(i + "|" + Main.buffName[i]);

            //        w.WriteLine("\n\nTile IDs\n");
            //        for (int i = 0; i < Main.maxTileSets; i++)
            //            w.WriteLine(i + "|" + Main.tileName[i]);

            //        w.WriteLine();

            //        w.Close();
            //    }

            //    fs.Close();
            //}
            #endregion
        }
        public static void Update()
        {
            oldKeyState = keyState;
            keyState = KeyHandler.GetState();
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (Main.dedServ || Main.netMode != 0 || Main.gameMenu || !Main.hasFocus)
                return;

            if (PlayerCheat.noClip || PlayerCheat.invincible)
                PlayerCheat.GMUpdate();
            else
            {
                myP.tileRangeX = 4;
                myP.tileRangeY = 5;
            }

            if (!Main.playerInventory && searching)
            {
                searching = false;
                Main.clrInput();
                search = "";
            }

            if (!searching)
            {
                if (pressCD < 15)
                    pressCD++;
                if (pressCD <= 15 && keyState.IsKeyDown(Key.N) && oldKeyState.IsKeyUp(Key.N))
                {
                    PlayerCheat.noClip = !PlayerCheat.noClip;
                    pressCD = 0;
                }
                if (pressCD <= 15 && keyState.IsKeyDown(Key.I) && oldKeyState.IsKeyUp(Key.I))
                {
                    PlayerCheat.invincible = !PlayerCheat.invincible;
                    pressCD = 0;
                }
            }

            if (Main.hideUI)
                goto END; // to lazy to do something else

            if (Main.playerInventory)
            {
                if (Main.craftGuide || Main.recBigList || Main.reforge)
                    state = UIType.None;
                switch (state)
                {
                    case UIType.None:
                        for (int i = 0; i < (int)UIType.Count; i++)
                            if (hovered((UIType)i))
                            {
                                myP.mouseInterface = true;
                                if (justClicked)
                                {
                                    state = (UIType)i;
                                    #region call Init
                                    switch (state)
                                    {
                                        case UIType.Item:
                                            ItemCheat.Init();
                                            break;
                                        case UIType.Buff:
                                            BuffCheat.Init();
                                            break;
                                        case UIType.Prefix:
                                            PrefixCheat.Init();
                                            break;
                                        case UIType.NPC:
                                            NPCCheat.Init();
                                            break;
                                        case UIType.Player:
                                            PlayerCheat.Init();
                                            break;
                                        case UIType.World:
                                            WorldCheat.Init();
                                            break;
                                        case UIType.Settings:
                                            MenuThemes.Init();
                                            break;
                                    }
                                    #endregion
                                    Main.PlaySound(12);
                                }
                            }
                        break;
                    #region call Update
                    case UIType.Buff:
                        BuffCheat.Update();
                        break;
                    case UIType.Item:
                        ItemCheat.Update();
                        break;
                    case UIType.NPC:
                        NPCCheat.Update();
                        break;
                    case UIType.Player:
                        PlayerCheat.Update();
                        break;
                    case UIType.Prefix:
                        PrefixCheat.Update();
                        break;
                    case UIType.World:
                        WorldCheat.Update();
                        break;
                    case UIType.CharMod:
                        PlayerCheat.CharMod.Update();
                        break;
                    case UIType.Settings:
                        MenuThemes.Update();
                        break;
                    case UIType.NPCVars:
                        NPCCheat.GVars.Update();
                        break;
                    #endregion
                }
                if (state != UIType.None)
                    if (new Rectangle(580, Main.screenHeight - 340, Main.cdTexture.Width, Main.cdTexture.Height).Intersects(mouse))
                    {
                        myP.mouseInterface = true;
                        if (justClicked)
                        {
                            #region call Close
                            switch (state)
                            {
                                case UIType.Item:
                                    ItemCheat.Close();
                                    break;
                                case UIType.Buff:
                                    BuffCheat.Close();
                                    break;
                                case UIType.Prefix:
                                    PrefixCheat.Close();
                                    break;
                                case UIType.NPC:
                                    NPCCheat.Close();
                                    break;
                                case UIType.Player:
                                    PlayerCheat.Close();
                                    break;
                                case UIType.World:
                                    WorldCheat.Close();
                                    break;
                                case UIType.CharMod:
                                    PlayerCheat.CharMod.Close();
                                    break;
                                case UIType.Settings:
                                    MenuThemes.Close();
                                    break;
                                case UIType.NPCVars:
                                    NPCCheat.GVars.Close();
                                    break;
                            }
                            #endregion
                            state = UIType.None;
                            Main.PlaySound(12);
                        }
                    }
            }
            else
            {
                if (oldInv)
                    #region call Close
                    switch (state)
                    {
                        case UIType.Item:
                            ItemCheat.Close();
                            break;
                        case UIType.Buff:
                            BuffCheat.Close();
                            break;
                        case UIType.Prefix:
                            PrefixCheat.Close();
                            break;
                        case UIType.NPC:
                            NPCCheat.Close();
                            break;
                        case UIType.Player:
                            PlayerCheat.Close();
                            break;
                        case UIType.World:
                            WorldCheat.Close();
                            break;
                        case UIType.CharMod:
                            PlayerCheat.CharMod.Close();
                            break;
                        case UIType.Settings:
                            MenuThemes.Close();
                            break;
                        case UIType.NPCVars:
                            NPCCheat.GVars.Close();
                            break;
                    }
                    #endregion

                state = UIType.None;
            }

        END:
            oldInv = Main.playerInventory;
        }
        public static void Draw()
        {
            if (Main.dedServ || Main.netMode != 0 || Main.gameMenu || Main.hideUI || !Main.hasFocus)
                return;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            if (Main.playerInventory)
            {
                if (state != UIType.None)
                    sb.Draw(Main.cdTexture, new Vector2(580f, Main.screenHeight - 340f), clr(
                        new Rectangle(580, Main.screenHeight - 340, Main.cdTexture.Width, Main.cdTexture.Height), Color.White));
                switch (state)
                {
                    case UIType.None:
                        for (int i = 0; i < (int)UIType.Count; i++)
                            sb.Draw(panel, panelHB((UIType)i).Position(), clr(panelHB((UIType)i), MenuThemes.ForeClr));
                        for (int i = 0; i < (int)UIType.Count; i++)
                            #region draw icons
                            switch ((UIType)i)
                            {
                                case UIType.Item:
                                    if (item == null)
                                        break;
                                    sb.Draw(item, panelHB((UIType)i).Position(), null,
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.NPC:
                                    if (npc == null)
                                        break;
                                    sb.Draw(npc, panelHB((UIType)i).Position() + new Vector2(0f, 2f), new Rectangle(0, 0, npc.Width, 24),
                                        clr(panelHB((UIType)i), new Color(0, 80, 255, 200)), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.Player:
                                    sb.Draw(Main.playerHeadTexture, panelHB((UIType)i).Position() + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerHeadTexture.Width, 30),
                                        clr(panelHB((UIType)i), myP.skinColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    sb.Draw(Main.playerEyeWhitesTexture, panelHB((UIType)i).Position() + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyeWhitesTexture.Width, 30),
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    sb.Draw(Main.playerEyesTexture, panelHB((UIType)i).Position() + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerEyesTexture.Width, 30),
                                        clr(panelHB((UIType)i), myP.eyeColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    sb.Draw(Main.playerHairTexture[myP.hair], panelHB((UIType)i).Position() + new Vector2(-4, 2), new Rectangle(0, 6, Main.playerHairTexture[myP.hair].Width, 30),
                                        clr(panelHB((UIType)i), myP.hairColor), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.Buff:
                                    sb.Draw(Main.buffTexture[1], panelHB((UIType)i).Position(), null,
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.Prefix:
                                    sb.Draw(Main.npcHeadTexture[9], panelHB((UIType)i).Position() + new Vector2(1f, 2f), null,
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.World:
                                    sb.Draw(Main.dayTime ? (Main.eclipse ? Main.sun3Texture : Main.sunTexture) : (Main.pumpkinMoon ? Main.pumpkinMoonTexture : Main.moonTexture[0]),
                                        panelHB((UIType)i).Position() + (Main.dayTime ? new Vector2(-17f) : Vector2.Zero),
                                        Main.dayTime ? null : new Rectangle?(new Rectangle(0, Main.moonPhase * Main.moonTexture[0].Width, Main.moonTexture[0].Width, Main.moonTexture[0].Width)),
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                                    break;
                                case UIType.Settings:
                                    sb.Draw(Main.itemTexture[1344], panelHB((UIType)i).Position() + new Vector2(1f), null,
                                        clr(panelHB((UIType)i), Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                                    break;
                            }
                            #endregion

                        #region call Draw
                        break;
                    case UIType.Buff:
                        BuffCheat.Draw();
                        break;
                    case UIType.Item:
                        ItemCheat.Draw();
                        break;
                    case UIType.NPC:
                        NPCCheat.Draw();
                        break;
                    case UIType.Player:
                        PlayerCheat.Draw();
                        break;
                    case UIType.Prefix:
                        PrefixCheat.Draw();
                        break;
                    case UIType.World:
                        WorldCheat.Draw();
                        break;
                    case UIType.CharMod:
                        PlayerCheat.CharMod.Draw();
                        break;
                    case UIType.Settings:
                        MenuThemes.Draw();
                        break;
                    case UIType.NPCVars:
                        NPCCheat.GVars.Draw();
                        break;
                        #endregion
                }
            }
            sb.End();
            sb.Begin();
        }

        public static void DrawOutlinedString(string s, Vector2 v, Color c, float scale = 1f)
        {
            Color C = alpha(Color.Black, c.A);

            sb.DrawString(Main.fontMouseText, s, v + new Vector2(1f), C, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.DrawString(Main.fontMouseText, s, v + new Vector2(-1f), C, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.DrawString(Main.fontMouseText, s, v + new Vector2(1f, -1f), C, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sb.DrawString(Main.fontMouseText, s, v + new Vector2(-1f, 1f), C, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            sb.DrawString(Main.fontMouseText, s, v, c, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
        public static Rectangle panelHB(UIType kind)
        {
            return new Rectangle(100 + 24 * (int)kind, Main.screenHeight - 100, 24, 24);
        }
        public static bool hovered(UIType kind)
        {
            return panelHB(kind).Intersects(mouse);
        }
        public static Color clr(Rectangle hb, Color inp)
        {
            return hb.Intersects(mouse) ? inp : Color.Lerp(inp, Color.Black, 0.5f);
        }
        public static Color full(Color clr)
        {
            return new Color(clr.R, clr.G, clr.B, 255);
        }
        public static int ticks(TimeSpan ts)
        {
            return (int)(ts.TotalSeconds * 60d); // if i leave out the 60, there's a chance of an outofmemoryexception
        }
        public static Color alpha(Color clr, int alpha)
        {
            clr.A = (byte)alpha;
            return clr;
        }
        #endregion

        public override bool KeyboardInputFocused()
        {
            return searching;
        }
        public override void PostDrawInventory(SpriteBatch sb)
        {
            base.PostDrawInventory(sb);

            Draw();
        }
    }
}
