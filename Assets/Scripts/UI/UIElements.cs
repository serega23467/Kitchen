using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToastMe;
using UnityEngine;

public class UIElements : MonoBehaviour
{
    GameObject canvas;
    UnityEngine.UI.Image panelMenu;
    UnityEngine.UI.Image panelSettings;
    UnityEngine.UI.Image panelConfirm;

    Vector3 scrollViewSize;
    Vector3 panelMenuSize;
    Vector3 panelSettingsSize;
    Vector3 panelConfirmSize;

    RectTransform scrollView;
    ScrollPanel scrollPanel;
    SettingsMenu settingsMenu;
    SliderMenu sliderMenu;
    ConfirmWindow confirmWindow;
    FinishWindow finishWindow;
    Timer timer;
    PanelInfo panelInfo;

    static ToastInfo lastMessage;
    private static UIElements instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        instance.canvas = GameObject.Find("Canvas");
        instance.scrollPanel = instance.canvas.GetComponent<ScrollPanel>();

        instance.scrollView = GameObject.Find("Scroll View").GetComponent<RectTransform>();
        instance.scrollViewSize = instance.scrollView.localScale;

        //instance.panelRecipe = GameObject.Find("PanelRecipe").GetComponent<UnityEngine.UI.Image>();
        //instance.panelRecipeSize = instance.panelRecipe.rectTransform.localScale;

        instance.panelInfo = GameObject.Find("PanelInfo").GetComponent<PanelInfo>(); 

        instance.finishWindow = GameObject.Find("PanelResult").GetComponent<FinishWindow>();

        instance.panelMenu = GameObject.Find("PanelMenu").GetComponent<UnityEngine.UI.Image>();
        instance.panelMenuSize = instance.panelMenu.rectTransform.localScale;

        instance.panelSettings = GameObject.Find("Settings").GetComponent<UnityEngine.UI.Image>();
        instance.panelSettingsSize = instance.panelSettings.rectTransform.localScale;
        instance.settingsMenu = instance.panelSettings.GetComponent<SettingsMenu>();

        instance.panelConfirm = GameObject.Find("PanelConfirm").GetComponent<UnityEngine.UI.Image>();
        confirmWindow = instance.panelConfirm.GetComponent<ConfirmWindow>();
        instance.panelConfirmSize = instance.panelConfirm.rectTransform.localScale;
        ClosePanelConfirm();

        instance.sliderMenu = GameObject.Find("SliderMenu").GetComponent<SliderMenu>();
        instance.sliderMenu.CloseSliderMenu();

        SettingsInit.UpdateVirtualSecond();
        instance.timer = GameObject.Find("Timer").GetComponent<Timer>();
        instance.timer.StartTimer();
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
    public int GetTimerTime()
    {
        if (instance.timer == null) return -1;
        return instance.timer.TotalSeconds;
    }
    public void ShowPanelResult(LevelInfo info, int playerRate, int totalSeconds, string issues)
    {
        if(instance.finishWindow != null) 
            instance.finishWindow.Show(info, playerRate, totalSeconds, issues);
    }
    public void HidePanelResult()
    {
        instance.finishWindow.Hide();
    }
    public void ShowObjectContent(IListable list, bool hasPlate = true)
    {
        scrollPanel.RetrieveData(list.Foods.ToList(), hasPlate, list.CanPull);
        scrollView.localScale = scrollViewSize;
    }
    public void UpdateObjectContent(IListable list)
    {
        scrollPanel.RetrieveData(list.Foods.ToList(), canPull: list.CanPull);
    }
    public void HideObjectContent()
    {
        scrollView.localScale = Vector3.zero;
    }
    public void ShowObjectInfo(string nameText, string descText, string dataText)
    {
        instance.panelInfo.ShowInfo(nameText, descText, dataText);
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
        panelSettings.gameObject.SetActive(true);
        settingsMenu.OpenTab("Game");
        settingsMenu.UpdateKeys();
        settingsMenu.UpdateSettings();
    }
    public void HideSettings()
    {
        panelSettings.gameObject.SetActive(false);
    }
    public void OpenSliderMenu(Action<List<FoodComponent>, int> onSelect, List<FoodComponent> list)
    {
        if(sliderMenu != null && !sliderMenu.IsOpen)
        {
            sliderMenu.OpenSliderMenu(onSelect, list);
        }
    }
    public void OpenPanelConfirm(string text, Action<bool> OnConfirm)
    {
        if (confirmWindow == null) return;
        panelConfirm.rectTransform.localScale = panelConfirmSize;
        confirmWindow.AddListener(OnConfirm, ClosePanelConfirm, text);
    }
    public void ExitToMainMenu()
    {
        Scenes.SwitchScene("MainMenu");
    }
    public void Restart()
    {
        Scenes.SwitchScene("Gameplay");
    }
    void ClosePanelConfirm()
    {
        if (confirmWindow == null) return;
        panelConfirm.rectTransform.localScale = Vector3.zero;
    }
}
