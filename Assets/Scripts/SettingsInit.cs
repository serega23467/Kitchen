using Assets.Scripts.UI;
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
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            controls.Add(info);
        }
        if (controls.Count <= 0) return;
        var moveAction = playerControls.FindAction("Movement");
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Name == "Вперёд").Value, Keyboard.current.wKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Name == "Назад").Value, Keyboard.current.sKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Name == "Влево").Value, Keyboard.current.aKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Name == "Вправо").Value, Keyboard.current.dKey);
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
    static void RebindKey(InputAction moveAction, string newKey, InputControl control)
    {      
        var bindingIndex = moveAction.GetBindingIndexForControl(control); 

        moveAction.Disable();

        moveAction.ApplyBindingOverride(bindingIndex, $"<Keyboard>/{newKey}");

        moveAction.Enable();

    }
}
