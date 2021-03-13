using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.UI
{
    public class ItemImage : MonoBehaviour
    {
        [SerializeField] Image image;

        public Image Image { get { return image; } }
    }
}
