using Assets.Scripts.UI;
using BrightnessPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public static class SettingsInit 
{
    static float virtualSecond = 0;

    static string currentLevelName;
    public static float VirtualSecond
    {
        get 
        { 
            if(virtualSecond == 0)
            {
                virtualSecond = GetVirtualSecond();
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
                currentLevelName = GetCurrentLevelName();
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
            BrightnessSingleton.GetInstance().SetBrightness(GetBrightness());
        int rIndex = GetResolution();
        bool isFullScreen = GetScreenMode();
        Screen.SetResolution(Resolutions[rIndex].x, Resolutions[rIndex].y, isFullScreen);
    }
    public static void InitAudio()
    {
        if(AudioManager.Instance != null && !AudioManager.Instance.IsSetted)
        {
            foreach (var nameValue in GetAudioValues())
            {
                AudioManager.Instance.ChangeVolume(nameValue.Value/100, nameValue.Key);
            }
        }
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
    public static void UpdateVirtualSecond()
    {
        virtualSecond = GetVirtualSecond();
    }
    public static void UpdateCurrentLevelName()
    {
        currentLevelName = GetCurrentLevelName();
    }
    public static void UpdateLevelInfo(int rate, int time)
    {
        DataTable scoreboard = DB.GetTable("SELECT cl.LevelId, l.Rate, l.Seconds FROM CurrentLevel cl JOIN Levels l On l.Id = cl.LevelId WHERE cl.Id = 1 ");
        int levelId = int.Parse(scoreboard.Rows[0][0].ToString());
        int oldRate = int.Parse(scoreboard.Rows[0][1].ToString());
        int oldSeconds = int.Parse(scoreboard.Rows[0][2].ToString());

        if(rate > oldRate)
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE Levels SET Rate = {rate}, Seconds = {time} Where Id = {levelId}");
        }
        else if (rate == oldRate)
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE Levels SET Seconds = {time} Where Id = {levelId}");
        }
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
    static float GetVirtualSecond()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Множитель времени';");
        float secs = 1f/float.Parse(scoreboard.Rows[0][2].ToString());
        return secs;
    }
    static string GetCurrentLevelName()
    {
        string levelName = "";
        DataTable scoreboard = DB.GetTable("SELECT r.RecipeFileName FROM CurrentLevel " +
            "cl JOIN Levels l ON cl.LevelId = l.Id " +
            "JOIN Recipes r ON l.RecipeId = r.Id" +
            " WHERE cl.Id  = 1; ");
        levelName = scoreboard.Rows[0][0]?.ToString();
        return levelName;   
    }
    static Dictionary<string, float> GetAudioValues()
    {
        Dictionary<string, float> values = new Dictionary<string, float>();
        DataTable scoreboard = DB.GetTable("SELECT Id, Name, Value FROM SettingsValues WHERE Name LIKE '%Громкость%'");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            values.Add(Translator.GetInstance().GetTranslate(scoreboard.Rows[i][1].ToString()), float.Parse(scoreboard.Rows[i][2].ToString()));
        }
        return values;
    }
}
