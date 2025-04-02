using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
public static class SettingsInit
{
    //static SettingsInit()
    //{
    //    playerControls = new PlayerControls();
    //}
    public static void InitControls(PlayerControls playerControls)
    {

        List<ControlsInfo> controls = new List<ControlsInfo>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new ControlsInfo() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Action = scoreboard.Rows[i][1].ToString(), Key = scoreboard.Rows[i][2].ToString() };
            controls.Add(info);
        }
        if (controls.Count <= 0) return;
        var moveAction = playerControls.FindAction("Movement");
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Action == "Вперёд").Key, Keyboard.current.wKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Action == "Назад").Key, Keyboard.current.sKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Action == "Влево").Key, Keyboard.current.aKey);
        RebindKey(moveAction, controls.FirstOrDefault(c => c.Action == "Вправо").Key, Keyboard.current.dKey);
    }
    static void RebindKey(InputAction moveAction, string newKey, InputControl control)
    {      
        var bindingIndex = moveAction.GetBindingIndexForControl(control); 

        moveAction.Disable();

        moveAction.ApplyBindingOverride(bindingIndex, $"<Keyboard>/{newKey}");

        moveAction.Enable();

    }
}
