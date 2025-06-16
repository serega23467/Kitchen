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
using UnityEngine.Events;
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
    Slider sliderVolume;
    [SerializeField]
    TMP_Text volumeValue;

    [SerializeField]
    Slider sliderMusic;
    [SerializeField]
    TMP_Text musicValue;

    [SerializeField]
    Slider sliderMenuMusic;
    [SerializeField]
    TMP_Text menuMusicValue;

    [SerializeField]
    Slider sliderEffects;
    [SerializeField]
    TMP_Text effectsValue;

    [SerializeField]
    TMP_Dropdown resolutionDropdown;

    [SerializeField]
    Toggle toogle;

    List<SettingValue> controlPanels;
    List<SettingValue> settingValues;
    Vector3 tabScale;
    
    int selectedKeyId = -1;
    bool isUpdated = false;
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

        UnityAction<float> updateVolumeMethod = delegate (float value) { UpdateVolumeValue(value, volumeValue, "Громкость"); };
        sliderVolume.onValueChanged.AddListener(updateVolumeMethod);

        UnityAction<float> updateMusicMethod = delegate (float value) { UpdateVolumeValue(value, musicValue, "Громкость музыки"); };
        sliderMusic.onValueChanged.AddListener(updateMusicMethod);

        UnityAction<float> updateMenuMusicMethod = delegate (float value) { UpdateVolumeValue(value, menuMusicValue, "Громкость музыки в главном меню"); };
        sliderMenuMusic.onValueChanged.AddListener(updateMenuMusicMethod);

        UnityAction<float> updateEffectsMethod = delegate (float value) { UpdateVolumeValue(value, effectsValue, "Громкость звуков"); };
        sliderEffects.onValueChanged.AddListener(updateEffectsMethod);

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
    private void Start()
    {
        Hide();
    }
    public void UpdateKeys()
    {
        List<SettingValue> controls = DB.GetControls();
        foreach(var control in controls)
        {
            control.OnValueChange.AddListener(ChangeButton);
        }
        RetrieveData(controls);
    }
    public void UpdateSettings()
    {
        if (isUpdated) return;

        settingValues.Clear();
        settingValues = DB.GetSettingValues();

        if(AudioManager.Instance.IsSetted)
        {
            sliderVolume.value = AudioManager.Instance.GetVolume(Translator.GetInstance().GetTranslate("Громкость"));
            sliderMusic.value = AudioManager.Instance.GetVolume(Translator.GetInstance().GetTranslate("Громкость музыки"));
            sliderMenuMusic.value = AudioManager.Instance.GetVolume(Translator.GetInstance().GetTranslate("Громкость музыки в главном меню"));
            sliderEffects.value = AudioManager.Instance.GetVolume(Translator.GetInstance().GetTranslate("Громкость звуков"));
        }
        else
        {
            sliderVolume.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Громкость").Value);
            sliderMusic.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Громкость музыки").Value);
            sliderMenuMusic.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Громкость музыки в главном меню").Value);
            sliderEffects.value = float.Parse(settingValues.FirstOrDefault(s => s.Name == "Громкость звуков").Value);
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
        isUpdated = true;


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
        int endControlsIndex = controlPanels.Count - 1;
        controlPanels.AddRange(settingValues);
        DB.ApplySettings(controlPanels, endControlsIndex);
       

        BrightnessSingleton.GetInstance().IsSetted = false;
        AudioManager.Instance.IsSetted = false;
        UpdateKeys();
        UpdateSettings();
        SettingsInit.InitVideo();
        SettingsInit.InitAudio();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
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
    void UpdateVolumeValue(float volume, TMP_Text tmpText, string setName)
    {
        tmpText.text = volume.ToString();
        var set = settingValues.FirstOrDefault(s => s.Name == setName);
        if (set != null) set.Value = tmpText.text;
        AudioManager.Instance.ChangeVolume(volume/100, Translator.GetInstance().GetTranslate(setName));
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
