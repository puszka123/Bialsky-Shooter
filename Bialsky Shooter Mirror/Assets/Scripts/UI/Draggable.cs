using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BialskyShooter.UI
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static event Action<Draggable> clientOnBeginDrag;
        public static event Action<Draggable> clientOnEndDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            clientOnBeginDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            clientOnEndDrag?.Invoke(this);
        }
    }
}
