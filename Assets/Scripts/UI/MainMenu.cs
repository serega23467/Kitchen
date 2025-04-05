using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image settings;
    [SerializeField]
    List<Image> tabs;
    [SerializeField]
    RecyclingListView theList;
    [SerializeField]
    Slider sliderSens;
    [SerializeField]
    TMP_Text sensValue;
    List<SettingValue> controlPanels;
    List<SettingValue> settingValues;
    Vector3 tabScale;
    //TabType currentTab = TabType.Game;
    private void Start()
    {
        controlPanels = new List<SettingValue>();
        settingValues = new List<SettingValue>();
        if (tabs != null && tabs.Count > 0)
        {
            tabScale = tabs[0].rectTransform.localScale;
        }
        sliderSens.minValue = 1;
        sliderSens.maxValue = 100;
        sliderSens.onValueChanged.AddListener(UpdateSensValue);
        UpdateSensValue(sliderSens.value);
    }
    public void OpenSettings()
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(true);
            OpenTab("Game");
            UpdateKeys();
            UpdateSettings();
        }
    }
    public void CloseSettings() 
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(false);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    void UpdateKeys()
    {
        List<SettingValue> controls = new List<SettingValue>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            info.OnValueChange.AddListener(ChangeButton);
            controls.Add(info);
        }
        RetrieveData(controls);
    }
    void UpdateSettings()
    {
        settingValues.Clear();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            settingValues.Add(info);
        }
        sliderSens.value = int.Parse(settingValues.FirstOrDefault(s => s.Name == "Чувствительность").Value);
    }
    void RetrieveData(List<SettingValue> panels)
    {
        theList.ItemCallback = PopulateItem;
        controlPanels.Clear();
        if (panels.Count > 0)
        {
            controlPanels.AddRange(panels);
        }
        theList.RowCount = panels.Count;
        theList.Refresh();
    }
    void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as Controlsitem;
        child.ControlsInfo = controlPanels[rowIndex];
    }
    void ChangeButton(int id)
    {
        StartCoroutine(WaitForKeyPress(id));
    }
    public void OpenTab(string name)
    {
        var tab = tabs.FirstOrDefault(t=>t.gameObject.name == name);
        if (tab == null) return;
        tab.rectTransform.localScale = tabScale;
        foreach (var item in tabs.Where(t => t.gameObject.name != name))
        {
            item.rectTransform.localScale = Vector3.zero;
        }
    }
    public void Apply()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls UNION ALL SELECT * FROM SettingsValues ;");
        int endControlsIndex = controlPanels.Count - 1;
        controlPanels.AddRange(settingValues);
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            if (!controlPanels[i].Equals(info) && controlPanels[i].Id == info.Id)
            {
                if(i> endControlsIndex)
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsValues SET Value = '{controlPanels[i].Value}' Where Id = {info.Id}");
                }
                else
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsControls SET Button = '{controlPanels[i].Value}' Where Id = {info.Id}");
                }
            }
        }
        UpdateKeys();
        UpdateSettings();
    }
    void UpdateSensValue(float value)
    {
        sensValue.text = Convert.ToInt32(value).ToString();
        var set = settingValues.FirstOrDefault(s => s.Name == "Чувствительность");
        if (set!=null) set.Value = sensValue.text;
    }
    IEnumerator WaitForKeyPress(int id)
    {
        var toChange = controlPanels.FirstOrDefault(p => p.Id == id);
        toChange.Value = "-";
        RetrieveData(new List<SettingValue>(controlPanels));
        // Ожидаем нажатия клавиши
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Проверяем, какая клавиша была нажата
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {            
                string keyStr = key.ToString();
                if (key == KeyCode.Escape)
                {
                    keyStr = "-";
                }
                toChange.Value = keyStr;
                RetrieveData(new List<SettingValue>(controlPanels));
                break;
            }
        }
    }

}
