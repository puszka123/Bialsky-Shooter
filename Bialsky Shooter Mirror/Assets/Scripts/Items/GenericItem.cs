using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BialskyShooter.ItemSystem
{
    [System.Serializable]
    public class GenericItem<T>
    {
        [SerializeField] T itemSO;
        Guid id;
        public T ItemSO { get { return itemSO; } }
        public Guid Id { get { return id; } }

        public GenericItem()
        {
            id = Guid.NewGuid();
        }
        public GenericItem(T itemSO)
        {
            this.itemSO = itemSO;
            id = Guid.NewGuid();
        }

        public GenericItem(Guid id, T itemSO)
        {
            this.itemSO = itemSO;
            this.id = id;
        }

        public GenericItem(GenericItem<T> item)
        {
            itemSO = item.itemSO;
            id = item.id;
        }
    }
}
