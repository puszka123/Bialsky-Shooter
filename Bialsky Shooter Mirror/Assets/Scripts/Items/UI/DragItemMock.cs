using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.ItemSystem.UI
{
    public class DragItemMock : MonoBehaviour
    {
        [SerializeField] Image image = null;
        [SerializeField] RectTransform rectTransform = null;
        [SerializeField] Canvas canvas = null;

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetPosition(Vector2 screenPosition)
        {
            var canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
            var screenSize = new Vector2(Screen.width, Screen.height);
            var factor = canvasSize / screenSize;
            rectTransform.anchoredPosition = screenPosition * factor;
        }
    }
}
