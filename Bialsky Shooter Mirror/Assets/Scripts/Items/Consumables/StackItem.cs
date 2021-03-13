using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public class StackItem : Item, IStackable
    {
        Stack stack;
        public Stack Stack { get { return stack; } }

        public StackItem(ItemSettings itemSettings, int count = 1) : base(itemSettings)
        {
            stack = new Stack
            {
                count = count,
                item = this
            };
        }

        public Stack Pop(int count)
        {
            if (stack.count < count) return default;
            stack.count -= count;
            return new Stack
            {
                item = stack.item,
                count = count,
            };
        }

        public void Push(Stack stackParam)
        {
            if (stackParam.item.GetItemSettings().UniqueName != stack.item.GetItemSettings().UniqueName) return;
            stack = new Stack
            {
                item = stack.item,
                count = stack.count + stackParam.count,
            };
        }

        public int GetCount()
        {
            return stack.count;
        }
    }
}
