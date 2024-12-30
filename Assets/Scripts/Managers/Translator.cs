using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            };
        }
        return instance;
    }
    public string GetTranslate(string word)
    {
        return translate[word];
    }
}
