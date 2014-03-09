using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TAPI;
using PoroCYon.MCT.Content;

namespace PoroCYon.ICM
{
    /// <summary>
    /// A class that holds data of a certain buff type
    /// </summary>
    public class Buff
    {
        /// <summary>
        /// The ID of the Buff
        /// </summary>
        public int ID = 0;

        /// <summary>
        /// The type of the buff - buff, debuff or weapon buff.
        /// </summary>
        public BuffType Type = BuffType.Buff;
        /// <summary>
        /// I have no idea what this is
        /// </summary>
        public bool PvP = false;
        /// <summary>
        /// Wether the Buff indicates that the player is having a pet that is meant for vanity purposes or not
        /// </summary>
        public bool VanityPet;
        /// <summary>
        /// Wether the Buff indicates that the player is having a pet that gives off light or not
        /// </summary>
        public bool LightPet;

        /// <summary>
        /// The display name of the Buff
        /// </summary>
        public string Name = "";
        /// <summary>
        /// The tooltip of the Buff
        /// </summary>
        public string Tooltip = "";

        /// <summary>
        /// The modded class of the buff
        /// </summary>
        public ModBuff SubClass
        {
            get
            {
                return Defs.buffs[ID];
            }
        }

        /// <summary>
        /// The icon texture of the buff
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return Main.buffTexture[ID];
            }
        }

        /// <summary>
        /// Creates a new instance of the Buff class
        /// </summary>
        public Buff()
        {

        }
        /// <summary>
        /// Creates a new instance of the Buff class
        /// </summary>
        /// <param name="id">The ID of the buff</param>
        public Buff(int id)
        {
            ID = id;

            Type = 0;
            if (Main.debuff[id])
                Type |= BuffType.Debuff;
            if (Main.meleeBuff[id])
                Type |= BuffType.WeaponBuff;

            PvP = Main.pvpBuff[id];
            VanityPet = Main.vanityPet[id];
            LightPet = Main.lightPet[id];

            Name = Defs.buffNames[id];

            Tooltip = Main.buffTip[id];
        }
        /// <summary>
        /// Creates a new instance of the Buff class
        /// </summary>
        /// <param name="name">The full name of the Buff</param>
        public Buff(string name)
            : this(Defs.buffType[name])
        {

        }
    }
}
