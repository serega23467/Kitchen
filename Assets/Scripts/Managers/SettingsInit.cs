using Assets.Scripts.UI;
using BrightnessPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public static class SettingsInit 
{
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
        BrightnessSingleton.GetInstance().SetBrightness(GetBrightness());
        int rIndex = GetResolution();
        bool isFullScreen = GetScreenMode();
        Screen.SetResolution(Resolutions[rIndex].x, Resolutions[rIndex].y, GetScreenMode());
    }
    public static void InitControls(PlayerControls playerControls)
    {
        List<SettingValue> controls = new List<SettingValue>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString(), BindingIndex = int.Parse(scoreboard.Rows[i][3].ToString()) };
            controls.Add(info);
        }
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
    static bool GetScreenMode()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Режим экрана';");
        return Convert.ToBoolean(int.Parse(scoreboard.Rows[0][2].ToString()));
    }
    static int GetResolution()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Разрешение';");
        return int.Parse(scoreboard.Rows[0][2].ToString());
    }
    static float GetBrightness()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Яркость';");
        return float.Parse(scoreboard.Rows[0][2].ToString());
    }
    public static float GetSensetivity()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Чувствительность';");
        float sens = float.Parse(scoreboard.Rows[0][2].ToString())/100;
        if(sens< 0.5f)
        {
            return 0.01f + (0.25f - 0.01f) * (sens / 0.5f);
        }
        else
        {
            return 0.25f + (1f - 0.25f) * ((sens-0.5f) / 0.5f);
        }
    }
}
