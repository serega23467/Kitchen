using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    SettingsMenu settingsMenu;
    [SerializeField]
    LevelsMenu levelsMenu;
    [SerializeField]
    PanelTutorial tutorial;

    PlayerControls playerControls;
    Stack<IHideble> openedPanels;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void Start()
    {
        SettingsInit.InitVideo();
        SettingsInit.InitAudio();
        AudioManager.Instance.PlayMusic("menu");

        openedPanels = new Stack<IHideble>();
    }
    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Escape.performed += delegate (CallbackContext context) { Escape(); };
    }
    void Escape()
    {
        if (openedPanels.TryPop(out IHideble panel))
        {
            if (!panel.IsActive)
            {
                Escape();
            }
            panel.Hide();
        }
    }
    public void OpenSettings()
    {
        if (settingsMenu != null)
        {
            settingsMenu.gameObject.SetActive(true);
            settingsMenu.OpenTab("Game");
            settingsMenu.UpdateKeys();
            settingsMenu.UpdateSettings();
            openedPanels.Push(settingsMenu);
        }
    }
    public void OpenLevelsMenu()
    {
        if (levelsMenu != null)
        {
            levelsMenu.gameObject.SetActive(true);
            levelsMenu.UpdateLevels();
            openedPanels.Push(levelsMenu);
        }
    }
    public void ShowTutorial()
    {
        if(tutorial!=null)
        {
            tutorial.Show();
            openedPanels.Push(tutorial);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Escape.performed -= delegate (CallbackContext context) { Escape(); };
    }
}
