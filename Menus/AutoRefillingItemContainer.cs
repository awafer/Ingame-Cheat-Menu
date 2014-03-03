using System;
using System.Collections.Generic;
using System.Linq;
using TAPI.SDK.GUI;
using TAPI.SDK.GUI.Controls;

namespace TAPI.PoroCYon.ICM.Menus
{
    /// <summary>
    /// An ItemContainer that automatically refills itself and has the maximum stack for the contained Item
    /// </summary>
    public sealed class AutoRefillingItemContainer : ItemContainer
    {
        /// <summary>
        /// Creates a new instance of the AutoRefillingItemContainer class
        /// </summary>
        public AutoRefillingItemContainer()
            : base(new Item())
        {

        }
        /// <summary>
        /// Creates a new instance of the AutoRefillingItemContainer class with the given Item
        /// </summary>
        /// <param name="i">Sets the ContainedItem field</param>
        public AutoRefillingItemContainer(Item i)
            : base(i)
        {
            ContainedItem.stack = ContainedItem.maxStack;
        }

        /// <summary>
        /// Called when the Item is changed
        /// </summary>
        /// <param name="old">The old item</param>
        /// <param name="new">The new item</param>
        protected override void ItemChanged(Item old, Item @new)
        {
            base.ItemChanged(old, @new);

            if (@new == null)
                ContainedItem = old;

            ContainedItem.stack = ContainedItem.maxStack;
        }
        /// <summary>
        /// When ContainedItem.stack is changed
        /// </summary>
        /// <param name="old">The old stack</param>
        /// <param name="new">The new stack</param>
        protected override void StackChanged(int old, int @new)
        {
            base.StackChanged(old, @new);

            ContainedItem.stack = ContainedItem.maxStack;
        }
    }
}
