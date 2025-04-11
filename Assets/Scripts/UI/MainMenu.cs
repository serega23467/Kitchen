using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    SettingsMenu settingsMenu;
    private void Start()
    {
        SettingsInit.InitVideo();
    }
    public void Play()
    {
        Scenes.SwitchScene("Gameplay");
    }
    public void OpenSettings()
    {
        if (settingsMenu != null)
        {
            settingsMenu.gameObject.SetActive(true);
            settingsMenu.OpenTab("Game");
            settingsMenu.UpdateKeys();
            settingsMenu.UpdateSettings();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
