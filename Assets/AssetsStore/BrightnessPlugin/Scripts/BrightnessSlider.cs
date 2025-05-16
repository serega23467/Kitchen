using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrightnessPlugin
{
    [RequireComponent(typeof(Slider))]
    public class BrightnessSlider : MonoBehaviour
    {
        Slider slider;
        void Start()
        {
            slider = GetComponent<Slider>();
            if (PlayerPrefs.HasKey("Brightness"))
            {
                slider.value = PlayerPrefs.GetFloat("Brightness");
            }

            slider.onValueChanged.AddListener(SetBrightness);
        }

        public void SetBrightness(float brightness)
        {
            BrightnessSingleton.Instance.SetBrightness(brightness);
        }
    }
}