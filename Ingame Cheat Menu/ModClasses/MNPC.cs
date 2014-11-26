using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TAPI;

namespace PoroCYon.ICM.ModClasses
{
    [GlobalMod]
    sealed class MNPC : ModNPC
    {
#pragma warning disable 649
        internal static int? SpawnRate    ;
        internal static bool DisableSpawns;
#pragma warning restore 649

        public override void UpdateSpawnRate(Player p)
        {
            base.UpdateSpawnRate(p);

            if (SpawnRate != null)
                NPC.spawnRate = SpawnRate.Value;
        }

        public override List<int> EditSpawnPool(int x, int y, List<int> pool, Player p)
        {
            if (DisableSpawns && pool.Count > 0)
                pool.Clear();

            return pool;
        }
    }
}
