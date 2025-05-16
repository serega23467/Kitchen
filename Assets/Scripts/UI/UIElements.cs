using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIElements : MonoBehaviour
{
    GameObject canvas;
    UnityEngine.UI.Image panelInfo;
    UnityEngine.UI.Image panelResult;
    UnityEngine.UI.Image panelMenu;
    UnityEngine.UI.Image panelSettings;
    UnityEngine.UI.Image panelConfirm;

    TMP_Text panelInfoTextName;
    TMP_Text panelInfoTextDescription;
    TMP_Text panelInfoTextData;
    TMP_Text panelResultText;
    TMP_Text panelResultHeaderText;

    Vector3 panelInfoSize;
    Vector3 panelResultSize;
    Vector3 scrollViewSize;
    Vector3 panelMenuSize;
    Vector3 panelSettingsSize;
    Vector3 panelConfirmSize;

    RectTransform scrollView;
    ScrollPanel scrollPanel;
    SettingsMenu settingsMenu;
    SliderMenu sliderMenu;
    ConfirmWindow confirmWindow;
    Timer timer;
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

        instance.panelInfo = GameObject.Find("PanelInfo").GetComponent<UnityEngine.UI.Image>();
        instance.panelInfoTextName = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[0];
        instance.panelInfoTextDescription = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[1];
        instance.panelInfoTextData = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[2];
        instance.panelInfoSize = instance.panelInfo.rectTransform.localScale;

        instance.panelResult = GameObject.Find("PanelResult").GetComponent<UnityEngine.UI.Image>();
        instance.panelResultHeaderText = instance.panelResult.GetComponentsInChildren<TMP_Text>()[0];
        instance.panelResultText = instance.panelResult.GetComponentsInChildren<TMP_Text>()[1];
        instance.panelResultSize = instance.panelResult.rectTransform.localScale;

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
    public int GetTimerTime()
    {
        if (instance.timer == null) return -1;
        return instance.timer.TotalSeconds;
    }
    public void ShowPanelResult(string result, string text)
    {
        instance.panelResultHeaderText.text = result;
        instance.panelResultText.text = text;
        instance.panelResult.rectTransform.localScale = instance.panelResultSize;
    }
    public void HidePanelResult()
    {
        instance.panelResult.rectTransform.localScale = new Vector3 (0, 0, 0);
    }
    public void ShowObjectContent(IListable list, bool hasPlate = true)
    {
        scrollPanel.RetrieveData(list.Foods.ToList(), hasPlate);
        scrollView.localScale = scrollViewSize;
    }
    public void UpdateObjectContent(List<FoodComponent> list)
    {
        scrollPanel.RetrieveData(list);
    }
    public void HideObjectContent()
    {
        scrollView.localScale = Vector3.zero;
    }
    public void ShowObjectInfo(string nameText, string descText, string dataText)
    {
        panelInfoTextName.text = nameText;
        panelInfoTextDescription.text = descText;
        panelInfoTextData.text = dataText;
        panelInfo.rectTransform.localScale = panelInfoSize;
    }
    public void HideObjectInfo()
    {
        panelInfo.rectTransform.localScale = Vector3.zero;
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
