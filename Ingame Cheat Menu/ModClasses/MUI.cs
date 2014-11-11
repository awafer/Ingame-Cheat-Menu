using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TAPI;

namespace PoroCYon.ICM.ModClasses
{
    sealed class MUI : ModInterface
    {
        public override bool PreDrawCrafting(SpriteBatch sb)
        {
            return MainUI.UIType == InterfaceType.None && base.PreDrawCrafting(sb);
        }
    }
}
