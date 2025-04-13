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
        { KeyCode.LeftControl, "leftCtrl" }
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
