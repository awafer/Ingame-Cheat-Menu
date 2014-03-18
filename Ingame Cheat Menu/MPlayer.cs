using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PoroCYon.XnaExtensions;
using TAPI;
using PoroCYon.MCT.Input;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM
{
    sealed class MPlayer : ModPlayer
    {
        internal static bool Invincibility = false, Noclip = false;

        static int oldStack;
        static bool oldMouse;
        static int[] CD = new int[2];

        public MPlayer(ModBase @base, Player p)
            : base(@base, p)
        {

        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player == Main.localPlayer)
            {
                if (!Main.playerInventory)
                    MainUI.UIType = InterfaceType.None;
                for (int i = 0; i < CD.Length; i++)
                    if (CD[i] <= 15)
                        CD[i]++;

                if (Invincibility)
                {
                    Main.localPlayer.immuneAlpha = 0;
                    Main.localPlayer.potionDelay = 0;
                    Main.localPlayer.lavaCD = 3;
                    Main.localPlayer.gravDir = 1f;
                    Main.localPlayer.immuneTime = 60;

                    Main.localPlayer.suffocating = false;
                    Main.localPlayer.slippy = false;
                    Main.localPlayer.slippy2 = false;
                    Main.localPlayer.powerrun = false;
                    Main.localPlayer.poisoned = false;
                    Main.localPlayer.venom = false;
                    Main.localPlayer.onFire = false;
                    Main.localPlayer.burned = false;
                    Main.localPlayer.suffocating = false;
                    Main.localPlayer.onFire2 = false;
                    Main.localPlayer.ichor = false;
                    Main.localPlayer.blackout = false;
                    Main.localPlayer.burned = false;
                    Main.localPlayer.onFrostBurn = false;
                    Main.localPlayer.blind = false;
                    Main.localPlayer.blackout = false;
                    Main.localPlayer.noItems = false;
                    Main.localPlayer.immune = true;
                    Main.localPlayer.lavaImmune = true;
                    Main.localPlayer.noKnockback = true;

                    for (int i = 0; i < 10; i++)
                        if ((Main.localPlayer.buffType[i] == 30 || Main.localPlayer.buffType[i] == 22 || Main.localPlayer.buffType[i] == 36 || Main.localPlayer.buffType[i] == 31
                            || Main.localPlayer.buffType[i] == 35 || Main.localPlayer.buffType[i] == 32) && Main.localPlayer.buffTime[i] > 0)
                            Main.localPlayer.buffTime[i] = Main.localPlayer.buffType[i] = 0;

                    if (Main.localPlayer.statLife < Main.localPlayer.statLifeMax)
                        Main.localPlayer.statLife = Main.localPlayer.statLifeMax;
                    if (Main.localPlayer.statMana < Main.localPlayer.statManaMax2)
                        Main.localPlayer.statMana = Main.localPlayer.statManaMax2;

                    Main.localPlayer.breath = Main.localPlayer.breathMax - 1;
                }
                if (Noclip)
                {
                    Main.localPlayer.fallStart = (int)(Main.localPlayer.position.Y / 16f);
                    Main.localPlayer.gravControl = false;
                }
                if (Invincibility && Noclip)
                {
                    Lighting.AddLight((int)Main.localPlayer.position.X, (int)Main.localPlayer.position.Y, 1.2f, 1.2f, 1.2f);

                    Main.dust[Dust.NewDust(Main.localPlayer.position, Main.localPlayer.width, Main.localPlayer.height, 57, Main.localPlayer.velocity.X * 3, Main.localPlayer.velocity.Y * 3 * Main.localPlayer.direction, 100, new Color(), 1.5f)].noGravity = true;

                    Main.localPlayer.accCompass = Main.localPlayer.accDepthMeter = 1;
                    Main.localPlayer.accWatch = 3;
                    Main.localPlayer.blockRange = 40;
                    Main.localPlayer.AddBuff(9, 60);
                    Main.localPlayer.AddBuff(17, 60);
                    Main.localPlayer.AddBuff(12, 60);
                    Main.localPlayer.pickSpeed = 0.000001f;

                    if (Main.mouseItem.type == 0)
                    {
                        if (Main.localPlayer.inventory[Main.localPlayer.selectedItem].stack < oldStack
                            && Main.localPlayer.selectedItem == Main.localPlayer.oldSelectItem && !oldMouse
                            && oldStack <= Main.localPlayer.inventory[Main.localPlayer.selectedItem].maxStack)
                            Main.localPlayer.inventory[Main.localPlayer.selectedItem].stack = oldStack;
                        oldStack = Main.localPlayer.inventory[Main.localPlayer.selectedItem].stack;
                        oldMouse = false;
                    }
                    else
                    {
                        if (Main.mouseItem.stack < oldStack && oldMouse && oldStack <= Main.mouseItem.maxStack)
                            Main.mouseItem.stack = oldStack;
                        oldStack = Main.mouseItem.stack;
                        oldMouse = true;
                    }

                    //Main.localPlayer.blockRange += 40;
                    Main.localPlayer.tileRangeX = 50;
                    Main.localPlayer.tileRangeY = 30;

                    if (!Constants.KeyboardInputFocused())
                    {
                        if (GInput.Keyboard.IsKeyDown(Keys.V) && CD[0] > 15)
                        {
                            CD[0] = 0;

                            Main.PlaySound(2, (int)Main.localPlayer.position.X, (int)Main.localPlayer.position.Y, 29);

                            for (int num89 = 0; num89 < 50; num89++)
                                Dust.NewDust(Main.localPlayer.position, Main.localPlayer.width, Main.localPlayer.height, 76, (float)Math.Cos(Main.rand.Next(628) / 100d), (float)Math.Cos(Main.rand.Next(628) / 100d), 150, default(Color), 2f);

                            Main.localPlayer.position = GInput.Mouse.WorldPosition + new Vector2(Main.localPlayer.width / 2, Main.localPlayer.height / 2);

                            for (int num91 = 0; num91 < 50; num91++)
                                Main.dust[Dust.NewDust(Main.localPlayer.position, Main.localPlayer.width, Main.localPlayer.height, 64, (float)Math.Cos(Main.rand.Next(628) / 100d) * 10f, (float)Math.Sin(Main.rand.Next(628) / 100d) * 20f, 150, default(Color), 6f)].noGravity = true;

                            Main.PlaySound(2, (int)Main.localPlayer.position.X, (int)Main.localPlayer.position.Y, 29);
                        }
                        if (GInput.Keyboard.IsKeyDown(Keys.J))
                        {
                            Main.localPlayer.position = GInput.Mouse.WorldPosition + new Vector2(Main.localPlayer.width / 2, Main.localPlayer.height / 2);

                            for (int zzz = 0; zzz < 18; zzz++)
                                for (int zzb = 0; zzb < 2; zzb++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(Main.localPlayer.position + new Vector2(-5, -5), (Main.localPlayer.width + 2 * 5), (Main.localPlayer.height + 2 * 5), 66, 0, 0, 100, default(Color), 2.5f)];
                                    d.velocity /= 10f;
                                    d.velocity += ((Main.localPlayer.velocity) * zzz / 100f);
                                    d.noGravity = true;
                                }
                        }
                        if (GInput.Keyboard.IsKeyDown(Keys.K) && CD[0] > 15)
                        {
                            CD[0] = 0;
                            foreach (NPC n in Main.npc)
                                if (n.active && !n.friendly)
                                {
                                    n.NPCLoot();
                                    n.HitEffect(Main.localPlayer.direction, 50);
                                    Main.PlaySound(4, (int)n.position.X, (int)n.position.Y, n.soundKilled);
                                    n.active = false;
                                }
                            foreach (Projectile pr in Main.projectile)
                                if (pr.active && (!pr.friendly || pr.hostile))
                                    pr.Kill();
                        }
                        if (GInput.Keyboard.IsKeyDown(Keys.L) && CD[1] > 5)
                        {
                            CD[1] = 0;
                            Projectile.NewProjectile(GInput.Mouse.WorldPosition.X, GInput.Mouse.WorldPosition.Y, 0, 0, 108, 500, 10f, Main.myPlayer);
                        }
                        if (GInput.Keyboard.IsKeyDown(Keys.P) && CD[1] > 5)
                        {
                            CD[1] = 0;
                            Projectile.NewProjectile(GInput.Mouse.WorldPosition.X, GInput.Mouse.WorldPosition.Y, 0, 0, 10, 0, 0, Main.myPlayer);
                            Projectile.NewProjectile(GInput.Mouse.WorldPosition.X, GInput.Mouse.WorldPosition.Y, 0, 0, 11, 0, 0, Main.myPlayer);
                        }
                    }
                }
            }
        }
    }
}
