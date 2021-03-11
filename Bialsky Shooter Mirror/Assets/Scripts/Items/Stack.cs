using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [Serializable]
    public struct Stack
    {
        public IItem item;
        public int count;
    }
}
