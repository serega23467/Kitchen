using System.Collections.Generic;
using UnityEngine;

public static class KeyValidator
{

    static List<KeyCode> bannedKeys = new List<KeyCode> { KeyCode.Escape, KeyCode.LeftApple, KeyCode.RightApple,
        KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6,
        KeyCode.KeypadEnter, KeyCode.Backspace
    };
    static Dictionary<KeyCode, string> keysWithAnalogNames = new Dictionary<KeyCode, string>()
    {
        { KeyCode.LeftControl, "leftCtrl" },
        { KeyCode.Alpha0, "0" },
        { KeyCode.Alpha1, "1" },
        { KeyCode.Alpha2, "2" },
        { KeyCode.Alpha3, "3" },
        { KeyCode.Alpha4, "4" },
        { KeyCode.Alpha5, "5" },
        { KeyCode.Alpha6, "6" },
        { KeyCode.Alpha7, "7" },
        { KeyCode.Alpha8, "8" },
        { KeyCode.Alpha9, "9" },
    };
    public static bool CheckKey(KeyCode key)
    {
        return !bannedKeys.Contains(key);
    }
    public static string GetKey(KeyCode key)
    {
        string keyName = key.ToString();
        if(keysWithAnalogNames.ContainsKey(key))
        {
            keyName = keysWithAnalogNames[key];
        }
        return keyName;
    }
}
