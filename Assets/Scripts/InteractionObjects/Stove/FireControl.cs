using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    [SerializeField]
    AudioSource fireSource;
    ParticleSystem flame;
    Dictionary<byte, Vector3> dictFlame = new Dictionary<byte, Vector3>();
    void Start()
    {

        dictFlame.Add(1, new Vector3(0.04f, 0.04f, 0.015f));
        dictFlame.Add(2, new Vector3(0.06f, 0.06f, 0.06f));
        dictFlame.Add(3, new Vector3(0.08f, 0.08f, 0.08f));

        ParticleSystem child = GetComponentInChildren<ParticleSystem>();
        if(child != null )
        {
            flame = child;

            flame.Pause();
        }

        if (fireSource != null) fireSource.volume = 0.9f;
    }
    public void ChangeFire(byte level)
    {
        if (flame != null)
        {
            if (level == 0)
            {
                if (!flame.isPaused)
                {
                    flame.Pause();
                    flame.gameObject.SetActive(false);
                    if (fireSource != null && fireSource.isPlaying)
                    {
                        fireSource.Stop();
                    }
                }
            }
            else
            {            
                if (flame.isPaused || !flame.gameObject.activeSelf)
                {
                    flame.gameObject.SetActive(true);
                    flame.Play();
                    if(fireSource != null && !fireSource.isPlaying)
                    {
                        fireSource.Play();
                    }
                }
                flame.gameObject.transform.localScale = dictFlame[level];
            }
        }
    }
}
