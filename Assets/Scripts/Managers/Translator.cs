 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Translator
{
    private static Translator instance;
    Dictionary<string, string> translate;
    private Translator() { }
    public static Translator GetInstance()
    {
        if (instance == null)
        {
            instance = new Translator();
            instance.translate = new Dictionary<string, string>()
            {
                { "None", "не нарезано" },
                { "Large", "крупно" },
                { "Medium", "средне" },
                { "Finely", "мелко" },
                {  "Громкость", "MasterVolume" },
                { "Громкость музыки", "MusicVolume" },
                { "Громкость музыки в главном меню", "MainMenuMusicVolume" },
                {  "Громкость звуков", "EffectsVolume"},
            };
        }
        return instance;
    }
    public string GetTranslate(string word)
    {
        return translate[word];
    }
    public static string ReplaceActionToKey(string original)
    {
        int startIndex = original.IndexOf('\'');

        int endIndex = original.IndexOf('\'', startIndex + 1);

        if (startIndex != -1 && endIndex != -1)
        {
            string prefix = original.Substring(0, startIndex);
            string suffix = original.Substring(endIndex + 1);

            var actionKeyDict = DB.GetActionKeyPairs();
            string actionName = original.Substring(startIndex+1, endIndex-startIndex-1);
            if (actionKeyDict.TryGetValue(actionName, out string keyboardKey))
            {
                return $"{prefix}{keyboardKey}{suffix}";
            }
        }
        return original;

    }
    public static string GetTimeBySeconds(int totalSeconds)
    {

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string hoursString = hours.ToString();
        hoursString = hoursString.Length < 2 ? "0" + hoursString : hoursString;

        string minutesString = minutes.ToString();
        minutesString = minutesString.Length < 2 ? "0" + minutesString : minutesString;

        string secondsString = seconds.ToString();
        secondsString = secondsString.Length < 2 ? "0" + secondsString : secondsString;

        return $"{hoursString}:{minutesString}:{secondsString}";
    }
}
