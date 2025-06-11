using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip mainMenuMusic;
    [SerializeField]
    AudioClip gameplayMusic;
    AudioSource source;
    [SerializeField]
    AudioMixerGroup mixer;
    [SerializeField]
    AudioMixerGroup menuMixer;
    [SerializeField]
    AudioMixerGroup musicMixer;

    [HideInInspector]
    public bool IsSetted = false;
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        source = gameObject.GetComponent<AudioSource>();
    }
    public void ChangeVolume(float volume, string volumeName)
    {
        if(volume < 0.1f)
        {
            mixer.audioMixer.SetFloat(volumeName, Mathf.Lerp(-80, 0, volume));
        }
        else
        {
            mixer.audioMixer.SetFloat(volumeName, Mathf.Lerp(-40, 0, volume));
        }
    }
    public float GetVolume(string volumeName)
    {
        if (mixer.audioMixer.GetFloat(volumeName, out float volume))
            return volume;
        else
            return 0;
    }
    public void PlayMusic(string name)
    {
        switch (name)
        {
            case "menu":
                source.outputAudioMixerGroup = menuMixer;
                source.clip = mainMenuMusic;
                source.volume = 0.1f;
                source.Play();
                break;
            case "gameplay":
                source.Stop();
                source.outputAudioMixerGroup = musicMixer;
                source.clip = gameplayMusic;
                source.volume = 1.0f;
                source.Play();
                break;
        }
    }
    void StopMusic()
    {
        source.Stop();
    }
}
