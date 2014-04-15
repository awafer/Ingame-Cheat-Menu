using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
        public BuffType Type
        {
            get
            {
                BuffType ret = BuffType.Buff;

                if (Main.debuff[ID])
                    ret = BuffType.Debuff;
                if (Main.meleeBuff[ID])
                    ret |= BuffType.WeaponBuff;

                return ret;
            }
        }

        /// <summary>
        /// I have no idea what this is
        /// </summary>
        public bool PvP
        {
            get
            {
                return Main.pvpBuff[ID];
            }
        }
        /// <summary>
        /// Wether the Buff indicates that the player is having a pet that is meant for vanity purposes or not
        /// </summary>
        public bool VanityPet
        {
            get
            {
                return Main.vanityPet[ID];
            }
        }
        /// <summary>
        /// Wether the Buff indicates that the player is having a pet that gives off light or not
        /// </summary>
        public bool LightPet
        {
            get
            {
                return Main.lightPet[ID];
            }
        }

        /// <summary>
        /// The name of the Buff
        /// </summary>
        public string Name
        {
            get
            {
                return Defs.buffNames[ID];
            }
        }
        /// <summary>
        /// The display name of the buff
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Main.buffName.Length >= ID ? Main.buffName[ID] : Name;
            }
        }
        /// <summary>
        /// The tooltip of the Buff
        /// </summary>
        public string Tooltip
        {
            get
            {
                return Main.buffTip[ID];
            }
        }

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
