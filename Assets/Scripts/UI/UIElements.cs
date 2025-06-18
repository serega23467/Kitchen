using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToastMe;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    UnityEngine.UI.Image panelMenu;
    Vector3 panelMenuSize;

    ScrollPanel scrollPanel;
    SettingsMenu settingsMenu;
    SliderMenu sliderMenu;
    ConfirmWindow confirmWindow;
    FinishWindow finishWindow;
    Timer timer;
    PanelInfo panelInfo;
    PanelRecipe panelRecipe;
    PanelTutorial panelTutorial;

    static ToastInfo lastMessage;
    private static UIElements instance;

    Stack<IHideble> openedPanels;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        openedPanels = new Stack<IHideble>();
        scrollPanel = GameObject.Find("PanelContent").GetComponent<ScrollPanel>();

        panelRecipe = GameObject.Find("PanelRecipe").GetComponent<PanelRecipe>();

        panelInfo = GameObject.Find("PanelInfo").GetComponent<PanelInfo>(); 

        finishWindow = GameObject.Find("PanelResult").GetComponent<FinishWindow>();

        panelMenu = GameObject.Find("PanelMenu").GetComponent<UnityEngine.UI.Image>();
        panelMenuSize = panelMenu.rectTransform.localScale;

        settingsMenu = GameObject.Find("Settings").GetComponent<SettingsMenu>();

        confirmWindow = GameObject.Find("PanelConfirm").GetComponent<ConfirmWindow>();

        sliderMenu = GameObject.Find("SliderMenu").GetComponent<SliderMenu>();

        panelTutorial = GameObject.Find("PanelTutorial").GetComponent<PanelTutorial>();

        timer = GameObject.Find("Timer").GetComponent<Timer>();
        SettingsInit.UpdateVirtualSecond();
        timer.StartTimer();

    }
    private void Start()
    {
        HideMenu();
    }
    public static UIElements GetInstance()
    {
        return instance;
    }
    public static void ShowToast(string message, string title="")
    {
        ToastInfo t = null;
        if(lastMessage?.Message == message)
        {
            lastMessage.Count++;
            t = ToastMe.Toast.Pop("Notification", message, title+ $" ({lastMessage.Count})");
            t.Count = lastMessage.Count;
            lastMessage.Destroy();
        }
        else
        {
            t = ToastMe.Toast.Pop("Notification", message, title);
            t.Count = 1;
        }
        lastMessage = t;
        t?.OnHide.AddListener(delegate () { if (lastMessage == t) lastMessage = null; });
    }
    public static int GetFontSize(int charCount)
    {
        int minChars = 1;
        int maxChars = 1000;
        int minCharsForBigValues = maxChars;
        int maxCharsForBigValues = 2000;

        int minFont = 20;
        int minFontForBigValues = 10;
        int maxFont = 40;
        int maxFontForBigValues = 30;

        if (charCount < minChars)
        {
            return 0;
        }
        int fontSize = 0;
        if (charCount > maxChars)
        {
            float t = (float)(charCount - minCharsForBigValues) / (maxCharsForBigValues - minCharsForBigValues);
            fontSize = (int)(maxFontForBigValues - t * (maxFontForBigValues - minFontForBigValues));
        }
        else
        {
            float t = (float)(charCount - minChars) / (maxChars - minChars);
            fontSize = (int)(maxFont - t * (maxFont - minFont));
        }

        return fontSize;
    }
    public int GetTimerTime()
    {
        if (timer == null) return -1;
        return timer.TotalSeconds;
    }
    public void ShowPanelResult(LevelInfo info, int playerRate, int totalSeconds, string issues)
    {
        if(finishWindow != null)
        {
            finishWindow.Show(info, playerRate, totalSeconds, issues);
            openedPanels.Push(finishWindow);
        }
    }
    public void HidePanelResult()
    {
        finishWindow.Hide();
    }
    public void ShowObjectContent(IListable list, bool hasPlate = true)
    {
        scrollPanel.Show();
        scrollPanel.RetrieveData(list.Foods.ToList(), hasPlate, list.CanPull);
        openedPanels.Push(scrollPanel);
    }
    public void UpdateObjectContent(IListable list)
    {
        scrollPanel.RetrieveData(list.Foods.ToList(), canPull: list.CanPull);
    }
    public void HideObjectContent()
    {
        scrollPanel.Hide();
    }
    public bool IsContentShowed()
    {
        return scrollPanel.isActiveAndEnabled;
    }    
    public void ShowObjectInfo(string nameText, string descText, string dataText)
    {
        panelInfo.ShowInfo(nameText, descText, dataText);
    }
    public void HideObjectInfo()
    {
        panelInfo.Hide();
    }
    public void ShowMenu()
    {
        Time.timeScale = 0f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        panelMenu.gameObject.SetActive(true);
    }
    public void HideMenu()
    {
        Time.timeScale = 1;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        panelMenu.gameObject.SetActive(false);
    }
    public void ShowSettings()
    {
        settingsMenu.Show();
        settingsMenu.OpenTab("Game");
        settingsMenu.UpdateKeys();
        settingsMenu.UpdateSettings();
        openedPanels.Push(settingsMenu);
    }
    public void HideSettings()
    {
        settingsMenu.Hide();
    }
    public void OpenSliderMenu(Action<List<FoodComponent>, int> onSelect, List<FoodComponent> list)
    {
        if(sliderMenu != null && !sliderMenu.IsActive)
        {
            sliderMenu.OpenSliderMenu(onSelect, list);
            openedPanels.Push(sliderMenu);
        }
    }
    public void OpenPanelConfirm(string text, Action<bool> OnConfirm)
    {
        if (confirmWindow == null) return;
        confirmWindow.Show(text, OnConfirm);
        openedPanels.Push(confirmWindow);

    }
    public void ShowRecipePanel()
    {
        panelRecipe.Show();
    }
    public void HideRecipePanel()
    {
        panelRecipe.Hide();
    }
    public void ShowTutorialPanel()
    {
        panelTutorial.Show();
        openedPanels.Push(panelTutorial);
    }
    public bool TryEscape()
    {
        if(openedPanels.TryPop(out IHideble panel))
        {
            if(!panel.IsActive)
            {
                return TryEscape();
            }
            panel.Hide();
            return false;
        }
        return true;

    }
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.PlayHideOnAwake = true;
        SceneLoader.SwitchScene("MainMenu");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneLoader.PlayHideOnAwake = true;
        SceneLoader.SwitchScene("Gameplay");
    }
}
