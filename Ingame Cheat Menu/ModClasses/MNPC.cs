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
        internal static int? SpawnRate    ;
        internal static bool DisableSpawns;

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
