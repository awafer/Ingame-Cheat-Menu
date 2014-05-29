using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace PoroCYon.ICM
{
    sealed class MWorld : ModWorld
    {
        public MWorld(ModBase @base)
            : base(@base)
        {

        }

        public override void Save(BinBuffer bb)
        {
            Main.dayRate = 1;

            base.Save(bb);
        }
    }
}
