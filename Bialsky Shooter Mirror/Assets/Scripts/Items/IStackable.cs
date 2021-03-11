using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    public interface IStackable
    {
        void Push(Stack stack);
        Stack Pop(int count);
        int GetCount();
    }
}
