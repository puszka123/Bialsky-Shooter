using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem
{
    public interface IItemSelection
    {
        Guid GetItemId();
        Image GetItemImage();
        void ItemDragged();
    }
}