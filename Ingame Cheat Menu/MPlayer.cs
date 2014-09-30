using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using TAPI;
using PoroCYon.MCT.Input;

namespace PoroCYon.ICM
{
    sealed class MPlayer : ModPlayer
    {
        internal static bool Invincibility = false, Noclip = false;

        static int oldStack;
        static bool oldMouse;
        static int[] CD = new int[2];
        static Vector2 oVel = Vector2.Zero; // overriddenVelocity

        static bool oldInv;

        public MPlayer()
            : base()
        {

        }

        public override void MidUpdate()
        {
            base.MidUpdate();

            if (player == Main.localPlayer)
            {
                if (!Main.playerInventory && oldInv)
                    MainUI.ChangeToUI(InterfaceType.None);

                oldInv = Main.playerInventory;

                for (int i = 0; i < CD.Length; i++)
                    if (CD[i] <= 15)
                        CD[i]++;

                #region invincibility
                if (Invincibility)
                {
                    player.immuneAlpha = 0;
                    player.potionDelay = 0;
                    player.lavaCD = 3;
                    player.gravDir = 1f;
                    player.immuneTime = 60;

                    player.suffocating = false;
                    player.slippy = false;
                    player.slippy2 = false;
                    player.powerrun = false;
                    player.poisoned = false;
                    player.venom = false;
                    player.onFire = false;
                    player.burned = false;
                    player.suffocating = false;
                    player.onFire2 = false;
                    player.ichor = false;
                    player.blackout = false;
                    player.burned = false;
                    player.onFrostBurn = false;
                    player.blind = false;
                    player.blackout = false;
                    player.noItems = false;
                    player.immune = true;
                    player.lavaImmune = true;
                    player.noKnockback = true;

                    for (int i = 0; i < 10; i++)
                        if ((player.buffType[i] == 30 || player.buffType[i] == 22 || player.buffType[i] == 36 || player.buffType[i] == 31
                            || player.buffType[i] == 35 || player.buffType[i] == 32) && player.buffTime[i] > 0)
                            player.buffTime[i] = player.buffType[i] = 0;

                    if (player.statLife < player.statLifeMax)
                        player.statLife = player.statLifeMax;
                    if (player.statMana < player.statManaMax2)
                        player.statMana = player.statManaMax2;

                    player.breath = player.breathMax - 1;
                }
                #endregion
                #region noclip
                if (Noclip)
                {
                    player.fallStart = (int)(player.position.Y / 16f);
                    player.gravControl = false;
                }
                #endregion
                #region both
                if (Invincibility && Noclip)
                {
                    Lighting.AddLight((int)player.position.X, (int)player.position.Y, 1.2f, 1.2f, 1.2f);

                    Main.dust[Dust.NewDust(player.position, player.width, player.height, 57, player.velocity.X * 3, player.velocity.Y * 3 * player.direction, 100, new Color(), 1.5f)].noGravity = true;

                    player.accCompass = player.accDepthMeter = 1;
                    player.accWatch = 3;
                    player.blockRange = 40;
                    player.AddBuff(9, 60);
                    player.AddBuff(17, 60);
                    player.AddBuff(12, 60);
                    player.pickSpeed = 0.000001f;

                    if (Main.mouseItem.type == 0)
                    {
                        if (player.inventory[player.selectedItem].stack < oldStack
                            && player.selectedItem == player.oldSelectItem && !oldMouse
                            && oldStack <= player.inventory[player.selectedItem].maxStack)
                            player.inventory[player.selectedItem].stack = oldStack;
                        oldStack = player.inventory[player.selectedItem].stack;
                        oldMouse = false;
                    }
                    else
                    {
                        if (Main.mouseItem.stack < oldStack && oldMouse && oldStack <= Main.mouseItem.maxStack)
                            Main.mouseItem.stack = oldStack;
                        oldStack = Main.mouseItem.stack;
                        oldMouse = true;
                    }

                    //player.blockRange += 40;
                    player.tileRangeX = 50;
                    player.tileRangeY = 30;

                    if (!API.KeyboardInputFocused())
                    {
                        if (GInput.Keyboard.IsKeyDown(Keys.V) && CD[0] > 15)
                        {
                            CD[0] = 0;

                            Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 29);

                            for (int num89 = 0; num89 < 50; num89++)
                                Dust.NewDust(player.position, player.width, player.height, 76, (float)Math.Cos(Main.rand.Next(628) / 100d), (float)Math.Cos(Main.rand.Next(628) / 100d), 150, default(Color), 2f);

                            player.position = GInput.Mouse.WorldPosition + new Vector2(player.width / 2, player.height / 2);

                            for (int num91 = 0; num91 < 50; num91++)
                                Main.dust[Dust.NewDust(player.position, player.width, player.height, 64, (float)Math.Cos(Main.rand.Next(628) / 100d) * 10f, (float)Math.Sin(Main.rand.Next(628) / 100d) * 20f, 150, default(Color), 6f)].noGravity = true;

                            Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 29);
                        }
                        if (GInput.Keyboard.IsKeyDown(Keys.J))
                        {
                            player.position = GInput.Mouse.WorldPosition + new Vector2(player.width / 2, player.height / 2);

                            for (int zzz = 0; zzz < 18; zzz++)
                                for (int zzb = 0; zzb < 2; zzb++)
                                {
                                    Dust d = Main.dust[Dust.NewDust(player.position + new Vector2(-5, -5), (player.width + 2 * 5), (player.height + 2 * 5), 66, 0, 0, 100, default(Color), 2.5f)];
                                    d.velocity /= 10f;
                                    d.velocity += ((player.velocity) * zzz / 100f);
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
                                    n.HitEffect(player.direction, 50);
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
                #endregion
            }
        }

        public override void PostUpdate()
        {
            base.PostUpdate();

            if (player == Main.localPlayer)
            {
                #region invincibility
                if (Invincibility)
                {
                    player.suffocating = false;
                    player.slippy = false;
                    player.slippy2 = false;
                    player.powerrun = false;
                    player.poisoned = false;
                    player.venom = false;
                    player.onFire = false;
                    player.burned = false;
                    player.suffocating = false;
                    player.onFire2 = false;
                    player.ichor = false;
                    player.blackout = false;
                    player.burned = false;
                    player.onFrostBurn = false;
                    player.blind = false;
                    player.blackout = false;
                    player.noItems = false;
                    player.immune = true;
                    player.lavaImmune = true;
                    player.noKnockback = true;
                    for (int i = 0; i < 10; i++)
                        if ((player.buffType[i] == 30 || player.buffType[i] == 22 || player.buffType[i] == 36 || player.buffType[i] == 31
                            || player.buffType[i] == 35 || player.buffType[i] == 32 || player.buffType[i] == 44 || player.buffType[i] == 46
                             || player.buffType[i] == 47 || player.buffType[i] == 67 || player.buffType[i] == 68 || player.buffType[i] == 69
                             || player.buffType[i] == 70 || player.buffType[i] == 80) && player.buffTime[i] > 0)
                            player.buffTime[i] = player.buffType[i] = 0;
                    if (player.statLife < player.statLifeMax)
                        player.statLife = player.statLifeMax;
                    if (player.statMana < player.statManaMax2)
                        player.statMana = player.statManaMax2;
                    player.breath = player.breathMax;

                    player.immuneAlpha = 0;
                    player.potionDelay = 0;
                    player.lavaCD = 3;
                    player.gravDir = 1f;
                    player.immuneTime = 60;
                }
                #endregion
                #region noclip
                if (Noclip)
                {
                    player.fallStart = (int)(player.position.Y / 16f);
                    player.gravControl = false;
                    player.position -= player.oldVelocity;
                    player.position -= player.velocity;
                    player.velocity = new Vector2(0, -0.0000002f);
                    float abc = 75f;
                    if (player.controlDown)
                        oVel.Y += abc;
                    if (player.controlUp)
                        oVel.Y -= abc;
                    if (player.controlLeft)
                        oVel.X -= abc;
                    if (player.controlRight)
                        oVel.X += abc;
                    if (player.position - oVel / 50f != player.oldPosition)
                    {
                        player.position -= player.position - player.oldPosition;
                        player.position += oVel / 25f;
                    }
                    else
                        player.position += oVel / 50f;
                    oVel *= 0.75f;
                }
                #endregion
                #region both
                if (Invincibility && Noclip)
                {
                    Lighting.AddLight((int)player.position.X, (int)player.position.Y, 1.2f, 1.2f, 1.2f);
                        Main.dust[Dust.NewDust(player.position, player.width, player.height, 57, player.velocity.X * 3, player.velocity.Y * 3 * player.direction, 100, new Color(), 1.5f)].noGravity = true;
                    player.accCompass = player.accDepthMeter = 1;
                    player.accWatch = 3;
                    player.blockRange = 40;
                    player.pickSpeed = 0.000001f;
                }
                #endregion
            }
        }
    }
}
