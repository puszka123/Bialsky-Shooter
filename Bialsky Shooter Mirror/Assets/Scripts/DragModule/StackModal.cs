using BialskyShooter.ItemSystem.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BialskyShooter.DragModule
{
    public class StackModal : MonoBehaviour
    {
        public event Action<IItemSlot, IItemSlot, int> OnStackFinished;

        [SerializeField] Slider slider;
        [SerializeField] TMP_Text stackValueText;
        [SerializeField] Image image;
        public IItemSlot Source { get; private set; }
        public IItemSlot Destination { get; private set; }

        public void Init(IItemSlot source, IItemSlot destination)
        {
            Source = source;
            Destination = destination;
            slider.minValue = 0;
            slider.maxValue = source.GetItemInformation().count;
            stackValueText.text = slider.value.ToString();
            image.sprite = Resources.Load<Sprite>(source.GetItemInformation().iconPath);
        }

        public void SliderOnValueChanged(float value)
        {
            stackValueText.text = ((int)value).ToString();
        }

        public void StackOk()
        {
            OnStackFinished?.Invoke(Source, Destination, (int)slider.value);
            Destroy(gameObject);
        }
    }
}