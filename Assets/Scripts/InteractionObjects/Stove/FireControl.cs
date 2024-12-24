using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    ParticleSystem flame;
    ParticleSystem flare;
    Dictionary<byte, Vector3> dict = new Dictionary<byte, Vector3>();
    void Start()
    {
        dict.Add(1, new Vector3(0.04f, 0.04f, 0.04f));
        dict.Add(2, new Vector3(0.06f, 0.06f, 0.06f));
        dict.Add(3, new Vector3(0.08f, 0.08f, 0.08f));
        ParticleSystem child = GetComponentInChildren<ParticleSystem>();
        if(child != null )
        {
            ParticleSystem child2 = child.gameObject.GetComponentInChildren<ParticleSystem>();
            if (child2 != null)
            {
                flame = child;
                flare = child2;

                flame.Pause();
                flare.Pause();
            }
        }
    }
    public void ChangeFire(byte level)
    {
        if (flame != null && flare != null)
        {
            if (level == 0)
            {
                if (!flame.isPaused)
                {
                    flame.Pause();
                    flame.gameObject.SetActive(false);
                }
                if (!flare.isPaused)
                {
                    flare.Pause();
                    flare.gameObject.SetActive(false);
                }
            }
            else
            {
                flame.gameObject.transform.localScale = dict[level];
                flare.gameObject.transform.localScale = dict[level];
                
                if (flame.isPaused)
                {
                    flame.gameObject.SetActive(true);
                    flame.Play();

                }
                if (flare.isPaused)
                {
                    flare.gameObject.SetActive(true);
                    flare.Play();
                }
            }
        }
    }
}
