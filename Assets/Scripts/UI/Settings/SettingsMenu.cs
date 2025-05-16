using Assets.Scripts.UI;
using BrightnessPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    List<Image> tabs;
    [SerializeField]
    RecyclingListView theList;

    [SerializeField]
    Slider sliderSens;
    [SerializeField]
    TMP_Text sensValue;

    [SerializeField]
    Slider sliderTime;
    [SerializeField]
    TMP_Text timeValue;

    [SerializeField]
    Slider sliderBrigh;
    [SerializeField]
    TMP_Text brighValue;

    [SerializeField]
    TMP_Dropdown resolutionDropdown;

    [SerializeField]
    Toggle toogle;

    List<SettingValue> controlPanels;
    List<SettingValue> settingValues;
    Vector3 tabScale;
    int selectedKeyId = -1;
    private void Awake()
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

        sliderBrigh.minValue = -1.8f;
        sliderBrigh.maxValue = 1.8f;
        sliderBrigh.onValueChanged.AddListener(UpdateBrighValue);

        if (sliderTime != null)
        { 
            sliderTime.minValue = 1;
            sliderTime.maxValue = 60;
            sliderTime.onValueChanged.AddListener(UpdateTimeValue);
        }

        resolutionDropdown.onValueChanged.AddListener(UpdateResolution);

        toogle.onValueChanged.AddListener(UpdateScreenMode);

        foreach (var resolution in SettingsInit.Resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.x + "x" + resolution.y));
        }
    }
    public void UpdateKeys()
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
    public void UpdateSettings()
    {
        settingValues.Clear();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            settingValues.Add(info);
        }

        sliderSens.value = int.Parse(settingValues.FirstOrDefault(s => s.Name == "Чувствительность").Value);
        if (BrightnessSingleton.GetInstance().IsSetted)
        {
            sliderBrigh.value = BrightnessSingleton.GetInstance().GetBrightness();
        }
        else
        {
            sliderBrigh.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Яркость").Value);
        }
        if (sliderTime != null)
            sliderTime.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Множитель времени").Value);

        resolutionDropdown.value = int.Parse(settingValues.FirstOrDefault(s => s.Name == "Разрешение").Value);


        toogle.isOn = Convert.ToBoolean(int.Parse(settingValues.FirstOrDefault(s => s.Name == "Режим экрана").Value));
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
        if (selectedKeyId > -1) return;
        selectedKeyId = id;
        var toChange = controlPanels.FirstOrDefault(p => p.Id == selectedKeyId);
        toChange.Value = "-";
        RetrieveData(new List<SettingValue>(controlPanels));
    }
    public void OpenTab(string name)
    {
        var tab = tabs.FirstOrDefault(t => t.gameObject.name == name);
        if (tab == null) return;
        tab.rectTransform.localScale = tabScale;
        foreach (var item in tabs.Where(t => t.gameObject.name != name))
        {
            item.rectTransform.localScale = Vector3.zero;
        }
    }
    public void Apply()
    {
        DataTable scoreboard = DB.GetTable("SELECT Id, Action, Button FROM SettingsControls UNION ALL SELECT * FROM SettingsValues ;");
        int endControlsIndex = controlPanels.Count - 1;
        controlPanels.AddRange(settingValues);
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            if (!controlPanels[i].Equals(info) && controlPanels[i].Id == info.Id)
            {
                if (i > endControlsIndex)
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsValues SET Value = '{controlPanels[i].Value}' Where Id = {info.Id}");
                }
                else
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsControls SET Button = '{controlPanels[i].Value}' Where Id = {info.Id}");
                }
            }
        }
        BrightnessSingleton.GetInstance().IsSetted = false;
        UpdateKeys();
        UpdateSettings();
        SettingsInit.InitVideo();
    }
    public void HideSettings()
    {
        gameObject.SetActive(false);
    }    
    void UpdateScreenMode(bool mode)
    {
        var set = settingValues.FirstOrDefault(s => s.Name == "Режим экрана");
        if (set != null) set.Value = Convert.ToInt32(mode).ToString();
    }
    void UpdateResolution(int index)
    {
        var set = settingValues.FirstOrDefault(s => s.Name == "Разрешение");
        if (set != null) set.Value = index.ToString();
    }
    void UpdateSensValue(float value)
    {
        sensValue.text = Convert.ToInt32(value).ToString();
        var set = settingValues.FirstOrDefault(s => s.Name == "Чувствительность");
        if (set != null) set.Value = sensValue.text;
    }
    void UpdateBrighValue(float value)
    {
        brighValue.text = Math.Round(value, 1).ToString();
        var set = settingValues.FirstOrDefault(s => s.Name == "Яркость");
        if (set != null) set.Value = brighValue.text;
        SettingsInit.ChangeBrighNoSave(value);
    }
    void UpdateTimeValue(float time)
    {
        timeValue.text = time.ToString();
        var set = settingValues.FirstOrDefault(s => s.Name == "Множитель времени");
        if (set != null) set.Value = timeValue.text;
    }
    void Update()
    {
        if(selectedKeyId>-1)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        string keyStr = KeyValidator.GetKey(key);
                        if (!KeyValidator.CheckKey(key))
                        {
                            keyStr = "-";
                        }
                        var toChange = controlPanels.FirstOrDefault(p => p.Id == selectedKeyId);
                        if (toChange == null) return;
                        toChange.Value = keyStr;
                        selectedKeyId = -1;
                        RetrieveData(new List<SettingValue>(controlPanels));
                    }
                }
            }
        }
    }
}
