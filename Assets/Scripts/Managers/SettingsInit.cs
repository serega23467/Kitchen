using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
public static class SettingsInit 
{

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
