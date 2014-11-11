using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace PoroCYon.ICM.ModClasses
{
    sealed class MWorld : ModWorld
    {
        public override void Save(BinBuffer bb)
        {
            Main.dayRate = 1;

            base.Save(bb);
        }
    }
}
