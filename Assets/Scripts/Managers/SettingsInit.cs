using Assets.Scripts.UI;
using BrightnessPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public static class SettingsInit 
{
    static float virtualSecond = 0;

    static string currentLevelName;
    static UnityEvent onUpdateKeys;
    public static float VirtualSecond
    {
        get 
        { 
            if(virtualSecond == 0)
            {
                virtualSecond = DB.GetVirtualSecond();
            }
            return virtualSecond; 
        }
        private set { virtualSecond = value; }
    }
    public static string CurrentLevelName
    {
        get
        {
            if (currentLevelName == "")
            {
                currentLevelName = DB.GetCurrentLevelName();
            }
            return currentLevelName;
        }
        private set { currentLevelName = value; }
    }

    public static readonly Vector2Int[] Resolutions = new Vector2Int[]
    {
           new Vector2Int(1920, 1080),
           new Vector2Int(1280, 720),
           new Vector2Int(800, 600),
           new Vector2Int(640, 480)
    };
    public static void ChangeBrighNoSave(float value)
    {
        BrightnessSingleton.GetInstance().SetBrightness(value);
    }
    public static void InitVideo()
    {
        if(!BrightnessSingleton.GetInstance().IsSetted)
            BrightnessSingleton.GetInstance().SetBrightness(DB.GetBrightness());
        int rIndex = DB.GetResolution();
        bool isFullScreen = DB.GetScreenMode();
        Screen.SetResolution(Resolutions[rIndex].x, Resolutions[rIndex].y, isFullScreen);
    }
    public static void InitAudio()
    {
        if(AudioManager.Instance != null && !AudioManager.Instance.IsSetted)
        {
            foreach (var nameValue in DB.GetAudioValues())
            {
                AudioManager.Instance.ChangeVolume(nameValue.Value/100, nameValue.Key);
            }
        }
    }
    public static void InitControls(PlayerControls playerControls)
    {
        List<SettingValue> controls = DB.GetControls();
        if (controls.Count <= 0) return;

        int lastIndex = 0;
        foreach (var action in playerControls.asset.actionMaps.First().actions)
        {
            for (int j = 0; j < action.bindings.Count; j++)
            {
                var control = controls.FirstOrDefault(c => c.BindingIndex == j+lastIndex);
                if (control!=null)
                {
                    action.Disable();
                    action.ApplyBindingOverride(j, $"<Keyboard>/{control.Value}");
                    action.Enable();
                }
            }
            lastIndex += action.bindings.Count;
        }
    }
    public static void UpdateVirtualSecond()
    {
        virtualSecond = DB.GetVirtualSecond();
    }
    public static void UpdateCurrentLevelName()
    {
        currentLevelName = DB.GetCurrentLevelName();
    }
    public static void AddListenerOnUpdateKeys(UnityAction action)
    {
        if (action == null) return;
        if(onUpdateKeys==null) onUpdateKeys = new UnityEvent();
        onUpdateKeys.AddListener(action);
    }
    public static void UpdateKeysInterface()
    {
        onUpdateKeys.Invoke();
    }
}
