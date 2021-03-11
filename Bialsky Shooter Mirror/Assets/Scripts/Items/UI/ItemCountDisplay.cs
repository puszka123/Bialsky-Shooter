using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BialskyShooter.ItemSystem.UI
{
    public class ItemCountDisplay : MonoBehaviour
    {
        [SerializeField] GameObject countUI;
        [SerializeField] TMP_Text text;
        public int Count { get; private set; }

        public void SetCount(int count)
        {
            Count = count;
            text.text = Count.ToString();
            countUI.SetActive(Count > 0);
        }

        public void Disable()
        {
            Count = 1;
            countUI.SetActive(false);
        }

        public bool IsActive()
        {
            return countUI.activeSelf;
        }
    }
}
