using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [CreateAssetMenu(fileName = "Health Potion", menuName = "ScriptableObjects/Items/Health Potion")]
    public class HealthPotion : Item, IStackable
    {
        [SerializeField] int count = 1;
        Stack stack;

        protected override void OnEnable()
        {
            base.OnEnable();
            stack = new Stack
            {
                item = this,
                count = count,
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
            if (stackParam.item.GetItem().UniqueName != stack.item.GetItem().UniqueName) return;
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
