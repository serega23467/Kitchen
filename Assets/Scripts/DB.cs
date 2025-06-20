using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;
using System;
using Assets.Scripts.UI;

public static class DB
{
    private const string fileName = "Kitchen.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static DB()
    {
        DBPath = GetDatabasePath();
    }
    /// <summary> Возвращает путь к БД. Если её нет в нужной папке на Андроиде, то копирует её с исходного apk файла. </summary>
    private static string GetDatabasePath()
    {
#if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_STANDALONE
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
#endif
    }
    /// <summary> Распаковывает базу данных в указанный путь. </summary>
    /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }
    /// <summary> Этот метод закрывает подключение к БД. </summary>
    private static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }
    /// <summary> Этот метод выполняет запрос query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    private static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }
    /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
    /// <param name="query"> Собственно запрос. </param>
    /// <returns> Возвращает значение 1 строки 1 столбца, если оно имеется. </returns>
    private static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }
    /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    private static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }
    public static Dictionary<string, string> GetActionKeyPairs()
    {
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            pairs.Add(scoreboard.Rows[i][1].ToString(), scoreboard.Rows[i][2].ToString());
        }
        return pairs;
    }
    public static string GetCurrentLevelName()
    {
        string levelName = "";
        DataTable scoreboard = DB.GetTable("SELECT r.RecipeFileName FROM CurrentLevel " +
            "cl JOIN Levels l ON cl.LevelId = l.Id " +
            "JOIN Recipes r ON l.RecipeId = r.Id" +
            " WHERE cl.Id  = 1; ");
        levelName = scoreboard.Rows[0][0]?.ToString();
        return levelName;
    }
    public static Dictionary<string, float> GetAudioValues()
    {
        Dictionary<string, float> values = new Dictionary<string, float>();
        DataTable scoreboard = DB.GetTable("SELECT Id, Name, Value FROM SettingsValues WHERE Name LIKE '%Громкость%'");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            values.Add(Translator.GetInstance().GetTranslate(scoreboard.Rows[i][1].ToString()), float.Parse(scoreboard.Rows[i][2].ToString()));
        }
        return values;
    }
    public static float GetVirtualSecond()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Множитель времени';");
        float secs = 1f / float.Parse(scoreboard.Rows[0][2].ToString());
        return secs;
    }
    public static float GetSensetivity()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Чувствительность';");
        float sens = float.Parse(scoreboard.Rows[0][2].ToString()) / 100;
        if (sens < 0.5f)
        {
            return 0.01f + (0.25f - 0.01f) * (sens / 0.5f);
        }
        else
        {
            return 0.25f + (1f - 0.25f) * ((sens - 0.5f) / 0.5f);
        }
    }
    public static bool GetScreenMode()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Режим экрана';");
        return Convert.ToBoolean(int.Parse(scoreboard.Rows[0][2].ToString()));
    }
    public static int GetResolution()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Разрешение';");
        return int.Parse(scoreboard.Rows[0][2].ToString());
    }
    public static float GetBrightness()
    {
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues WHERE Name = 'Яркость';");
        return float.Parse(scoreboard.Rows[0][2].ToString());
    }
    public static void UpdateLevelInfo(int rate, int time)
    {
        DataTable scoreboard = DB.GetTable("SELECT cl.LevelId, l.Rate, l.Seconds FROM CurrentLevel cl JOIN Levels l On l.Id = cl.LevelId WHERE cl.Id = 1 ");
        int levelId = int.Parse(scoreboard.Rows[0][0].ToString());
        int oldRate = int.Parse(scoreboard.Rows[0][1].ToString());
        int oldSeconds = int.Parse(scoreboard.Rows[0][2].ToString());

        if (rate > oldRate)
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE Levels SET Rate = {rate}, Seconds = {time} WHERE Id = {levelId}");
        }
        else if (rate == oldRate)
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE Levels SET Seconds = {time} WHERE Id = {levelId}");
        }

        if (rate >= 3)
        {
            DB.ExecuteQueryWithoutAnswer($"DELETE FROM LockedLevels WHERE NeedForUnlockId = {levelId}");
        }
    }
    public static List<SettingValue> GetControls()
    {
        List<SettingValue> controls = new List<SettingValue>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsControls;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString(), BindingIndex = int.Parse(scoreboard.Rows[i][3].ToString()) };
            controls.Add(info);
        }
        return controls;
    }
    public static List<SettingValue> GetSettingValues()
    {
        List<SettingValue> settingValues = new List<SettingValue>();
        DataTable scoreboard = DB.GetTable("SELECT * FROM SettingsValues;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            settingValues.Add(info);
        }
        return settingValues;
    }
    public static bool TryUpdateCurrentLevel(int newId)
    {
        DataTable scoreboard = DB.GetTable("SELECT Id FROM CurrentLevel;");
        if (int.TryParse(scoreboard.Rows[0][0].ToString(), out int result))
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE CurrentLevel SET LevelId = {newId} Where Id = {result}");
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void ApplySettings(List<SettingValue> settings, int endControlsIndex)
    {
        DataTable scoreboard = DB.GetTable("SELECT Id, Action, Button FROM SettingsControls UNION ALL SELECT * FROM SettingsValues ;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new SettingValue() { Id = int.Parse(scoreboard.Rows[i][0].ToString()), Name = scoreboard.Rows[i][1].ToString(), Value = scoreboard.Rows[i][2].ToString() };
            if (!settings[i].Equals(info) && settings[i].Id == info.Id)
            {
                if (i > endControlsIndex)
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsValues SET Value = '{settings[i].Value}' Where Id = {info.Id}");
                }
                else
                {
                    DB.ExecuteQueryWithoutAnswer($"UPDATE SettingsControls SET Button = '{settings[i].Value}' Where Id = {info.Id}");
                }
            }
        }
    }
    public static List<Level> GetLevels()
    {
        List<Level> levels = new List<Level>();
        DataTable scoreboard = DB.GetTable("SELECT l.Id, l.Name, l.Description, l.ImageName, l.Rate, l.Seconds, r.RecipeFileName " +
            "FROM Levels l JOIN Recipes r ON r.Id = l.RecipeId;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new Level()
            {
                Id = int.Parse(scoreboard.Rows[i][0].ToString()),
                Name = scoreboard.Rows[i][1].ToString(),
                Description = scoreboard.Rows[i][2].ToString(),
                ImageName = scoreboard.Rows[i][3].ToString(),
                Rate = int.Parse(scoreboard.Rows[i][4].ToString()),
                Seconds = int.Parse(scoreboard.Rows[i][5].ToString()),
                FolderName = scoreboard.Rows[i][6].ToString(),
            };
            if (CheckLock(info.Id))
            {
                info.IsLocked = CheckLock(info.Id);
                info.ImageName = "";
            }
            levels.Add(info);
        }
        return levels;
    }
    static bool CheckLock(int levelId)
    {
        DataTable scoreboard = DB.GetTable($"SELECT LockedId FROM LockedLevels WHERE LockedId = {levelId}");
        if (scoreboard.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
}