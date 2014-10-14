using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoroCYon.Extensions.Xna.Graphics;
using Terraria;
using TAPI;
using PoroCYon.MCT.UI;
using PoroCYon.MCT.UI.Interface.Controls;
using PoroCYon.ICM.Menus;

namespace PoroCYon.ICM.Controls
{
    /// <summary>
    /// A mod filter.
    /// </summary>
    /// <typeparam name="TCodableEntity">The type of the elements that are filtered.</typeparam>
    public abstract class ModFilter<TCodableEntity> : ImageButton
        where TCodableEntity : CodableEntity
    {
        /// <summary>
        /// The ModBase.
        /// </summary>
        public ModBase ModBase;

        /// <summary>
        /// Gets the hitbox of the Control.
        /// </summary>
        public override Rectangle Hitbox
        {
            get
            {
                Vector2 pos = Position - (Main.inventoryBackTexture.Size() / 2f - PicAsTexture.Size() / 2f);

                return new Rectangle((int)pos.X, (int)pos.Y, 52, 52);
            }
        }

        /// <summary>
        /// Creates a new instance of the ModFilter class.
        /// </summary>
        /// <param name="@base">The ModBase used to filter CodableEntities.</param>
        public ModFilter(ModBase @base)
            : base(MctUI.WhitePixel)
        {
            ModBase = @base;

            HasBackground = true;

            Tooltip = @base.mod.path.FileNameWithoutExtension;

            SetTexture();
        }

        /// <summary>
        /// Updates the Control.
        /// </summary>
        public override void Update()
        {
            base.Update();

            Colour = GetIsSelected() ? new Color(255, 255, 255, 0) : new Color(127, 127, 127, 0);
        }

		/// <summary>
		/// Gets whether the CodableEntity is in the filtered entities collection or not.
		/// </summary>
		/// <param name="ce">The CodableEntity to check.</param>
		/// <returns>true if the CodableEntity is in the filtered entities; false otherwise.</returns>
		public bool Filter(TCodableEntity ce)
		{
			return Filtered().Contains(ce);
		}

		/// <summary>
		/// Gets all filtered CodableEntities.
		/// </summary>
		/// <returns>All CodableEntities that belong to the ModBase.</returns>
		public abstract IEnumerable<TCodableEntity> Filtered();
		//public IEnumerable<TCodableEntity> Filtered()
		//{
		//    return from ce in GetCollection() where ce.modBase == ModBase select ce;
		//}

		void SetTexture()
        {
            Texture2D tex = null;
            try
            {
                tex = ModBase.OnModCall(Mod.Instance, "GetTexture", typeof(TCodableEntity)) as Texture2D;
            }
            catch { }

            if ((tex = tex ?? GetDefaultImage()) != null)
                Picture = tex;
        }

        /// <summary>
        /// Gets the default image of the filter.
        /// </summary>
        /// <returns>The default image of the filter (when the ModBase does not provide an alternative image).</returns>
        protected virtual Texture2D GetDefaultImage()
        {
            return Main.confuseTexture;
        }
        /// <summary>
        /// Gets the complete collection of CodableEntites.
        /// </summary>
        /// <returns>The complete collection of CodableEntites, even if they don't belong to the ModBase.</returns>
        protected abstract IEnumerable<TCodableEntity> GetCollection();
        /// <summary>
        /// Gets whether the filter is active or not.
        /// </summary>
        /// <returns>Whether the filter is active or not.</returns>
        protected abstract bool GetIsSelected();
    }

    /// <summary>
    /// A mod filter for Items.
    /// </summary>
    public class ItemFilter : ModFilter<Item>
    {
        /// <summary>
        /// Creates a new instance of the ItemFilter class.
        /// </summary>
        /// <param name="@base">The ModBase used to filter CodableEntities.</param>
        public ItemFilter(ModBase @base)
            : base(@base)
        {

        }

        /// <summary>
        /// Clicks the Button.
        /// </summary>
        protected override void Click()
        {
            base.Click();

            ItemUI.Instance.CurrentModFilter = ItemUI.Instance.CurrentModFilter == this ? null : this;

            ItemUI.Instance.Position = 0;

            ItemUI.Instance.ResetObjectList();
        }

		/// <summary>
		/// Gets all filtered CodableEntities.
		/// </summary>
		/// <returns>All CodableEntities that belong to the ModBase.</returns>
		public override IEnumerable<Item> Filtered()
		{
			return from ce in GetCollection() where ce.modEntities.Any(mi => mi.modBase == ModBase) select ce;
		}

		/// <summary>
		/// Gets the default image of the filter.
		/// </summary>
		/// <returns>The default image of the filter (when the ModBase does not provide an alternative image).</returns>
		protected override Texture2D GetDefaultImage()
        {
            if (ModBase == null) // vanilla
                return Main.itemTexture[1];

			var i = ItemDef.byType.FirstOrDefault();
			var v = i.Key == 0 ? null : i.Value.modEntities.FirstOrDefault(mi => mi.modBase == ModBase);
            return v != null ? i.Value.GetTexture() : base.GetDefaultImage();
        }
        /// <summary>
        /// Gets the complete collection of CodableEntites.
        /// </summary>
        /// <returns>The complete collection of CodableEntites, even if they don't belong to the ModBase.</returns>
        protected override IEnumerable<Item> GetCollection()
        {
            return ItemDef.byType.Values;
        }
        /// <summary>
        /// Gets whether the filter is active or not.
        /// </summary>
        /// <returns>Whether the filter is active or not.</returns>
        protected override bool GetIsSelected()
        {
            return ItemUI.Instance.CurrentModFilter == this;
        }
    }
    /// <summary>
    /// A mod filter for NPCs.
    /// </summary>
    public class NpcFilter : ModFilter<NPC>
    {
        Rectangle oneFrame
        {
            get
            {
                Main.LoadNPC(1);
                return new Rectangle(0, 0, Main.npcTexture[1].Width, Main.npcTexture[1].Height / Main.npcFrameCount[1]);
            }
        }

        /// <summary>
        /// Creates a new instance of the NpcFilter class.
        /// </summary>
        /// <param name="@base">The ModBase used to filter CodableEntities.</param>
        public NpcFilter(ModBase @base)
            : base(@base)
        {

        }

        /// <summary>
        /// Clicks the Button.
        /// </summary>
        protected override void Click()
        {
            base.Click();

            NpcUI.Instance.CurrentModFilter = NpcUI.Instance.CurrentModFilter == this ? null : this;

            NpcUI.Instance.Position = 0;

            NpcUI.Instance.ResetObjectList();
		}

		/// <summary>
		/// Gets all filtered CodableEntities.
		/// </summary>
		/// <returns>All CodableEntities that belong to the ModBase.</returns>
		public override IEnumerable<NPC> Filtered()
		{
			return from ce in GetCollection() where ce.modEntities.Any(mn => mn.modBase == ModBase) select ce;
		}

		/// <summary>
		/// Gets the default image of the filter.
		/// </summary>
		/// <returns>The default image of the filter (when the ModBase does not provide an alternative image).</returns>
		protected override Texture2D GetDefaultImage()
        {
            if (ModBase == null) // vanilla
            {
                Main.LoadNPC(1);
                return Main.npcTexture[1];
            }

			var n = NPCDef.byType.FirstOrDefault();
			var v = n.Key == 0 ? null : n.Value.modEntities.FirstOrDefault(mi => mi.modBase == ModBase);
			
			if (v != null)
			{
				Main.LoadNPC(n.Key);
				return Main.npcTexture[n.Key];
			}

            return base.GetDefaultImage();
        }
        /// <summary>
        /// Gets the complete collection of CodableEntites.
        /// </summary>
        /// <returns>The complete collection of CodableEntites, even if they don't belong to the ModBase.</returns>
        protected override IEnumerable<NPC> GetCollection()
        {
            return NPCDef.byType.Values;
        }
        /// <summary>
        /// Gets whether the filter is active or not.
        /// </summary>
        /// <returns>Whether the filter is active or not.</returns>
        protected override bool GetIsSelected()
        {
            return NpcUI.Instance.CurrentModFilter == this;
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the Control.</param>
        public override void Draw(SpriteBatch sb)
        {
            DrawBackground(sb);

            sb.Draw(Picture, Position + Hitbox.Size() / 2f - oneFrame.Size() / 2f,
                oneFrame, Colour, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
        }
    }
}
