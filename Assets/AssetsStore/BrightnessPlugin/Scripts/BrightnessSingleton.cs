using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightnessPlugin
{
    public class BrightnessSingleton : MonoBehaviour
    {
        public static BrightnessSingleton instance;
        public bool doubleBright = false;
        private UseBrightnessPlugin bPlugin, bPlugin2;

        void Awake()
        {
            CreateSingleton();
        }

        void CreateSingleton()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            bPlugin = GetComponent<UseBrightnessPlugin>();
            if (doubleBright && bPlugin2 == null)
            {
                bPlugin2 = gameObject.AddComponent<UseBrightnessPlugin>();
            }
        }
        public static BrightnessSingleton GetInstance()
        {
            return instance;
        }
        public void SetBrightness(float brightness)
        {
            bPlugin.SetBrightness(brightness);
            if (bPlugin2 != null) bPlugin2.SetBrightness(brightness);
        }
    }
}