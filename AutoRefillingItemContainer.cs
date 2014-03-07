using System;
using System.Collections.Generic;
using System.Linq;
using TAPI;
using TAPI.SDK.UI;
using TAPI.SDK.UI.Interface;
using TAPI.SDK.UI.Interface.Controls;

namespace PoroCYon.ICM
{
    /// <summary>
    /// An ItemContainer that automatically refills itself and has the maximum stack for the contained Item
    /// </summary>
    public sealed class AutoRefillingItemContainer : ItemContainer
    {
        bool preventSO = false;

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
            if (preventSO)
                return;

            base.ItemChanged(old, @new);

            preventSO = true;

            if (@new == null)
                ContainedItem = old;

            ContainedItem.stack = ContainedItem.maxStack;

            preventSO = false;
        }
        /// <summary>
        /// When ContainedItem.stack is changed
        /// </summary>
        /// <param name="old">The old stack</param>
        /// <param name="new">The new stack</param>
        protected override void StackChanged(int old, int @new)
        {
            if (preventSO)
                return;

            base.StackChanged(old, @new);

            preventSO = true;

            ContainedItem.stack = ContainedItem.maxStack;

            preventSO = false;
        }
    }
}
