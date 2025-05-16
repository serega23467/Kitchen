using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonLoader
{
    public static void SaveLevelInfo(LevelInfo levelInfo, string fileName)
    {
        string json = JsonUtility.ToJson(levelInfo, true);
        string path = Path.Combine(Application.dataPath, fileName + ".json");

        File.WriteAllText(path, json);
    }

    public static LevelInfo LoadLevelInfo(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/" + fileName + "/" + fileName);

        if (jsonFile != null)
        {
            LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonFile.text);
            return levelInfo;
        }
        else
        {
            return null;
        }

    }
}
