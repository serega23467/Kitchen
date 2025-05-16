using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightnessPlugin
{
    public class BrightnessSingleton : MonoBehaviour
    {
        public static BrightnessSingleton Instance;
        public bool DoubleBright = false;
        public bool IsSetted = false;
        float brValue = 0f;
        UseBrightnessPlugin bPlugin, bPlugin2;

        void Awake()
        {
            CreateSingleton();
        }

        void CreateSingleton()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            bPlugin = GetComponent<UseBrightnessPlugin>();
            if (DoubleBright && bPlugin2 == null)
            {
                bPlugin2 = gameObject.AddComponent<UseBrightnessPlugin>();
            }
        }
        public static BrightnessSingleton GetInstance()
        {
            return Instance;
        }
        public void SetBrightness(float brightness)
        {
            bPlugin.SetBrightness(brightness);
            if (bPlugin2 != null) bPlugin2.SetBrightness(brightness);
            IsSetted = true;
            brValue = brightness;
        }
        public float GetBrightness()
        {
            return brValue;
        }
    }
}