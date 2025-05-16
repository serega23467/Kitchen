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
    [SerializeField]
    LevelsMenu levelsMenu;
    private void Start()
    {
        SettingsInit.InitVideo();
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
    public void OpenLevelsMenu()
    {
        if (levelsMenu != null)
        {
            levelsMenu.gameObject.SetActive(true);
            levelsMenu.UpdateLevels();
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
