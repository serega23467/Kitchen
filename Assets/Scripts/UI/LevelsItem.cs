using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsItem : RecyclingListViewItem
{
    [SerializeField]
    TMP_Text levelName;
    [SerializeField]
    TMP_Text levelDesc;
    [SerializeField]
    Image levelPicture;
    [SerializeField]
    Button buttonPlay;
    [SerializeField]
    Image[] stars;
    [SerializeField]
    TMP_Text time;

    Level levelInfo;
    public Level LevelInfo
    {
        get { return levelInfo; }
        set 
        {
            if (value == null) return;
            levelInfo = value;

            Sprite loadedSprite = Resources.Load<Sprite>(levelInfo.ImageName);
            if (loadedSprite != null)
            {
                levelPicture.sprite = loadedSprite; 
            }
            else
            {
                Sprite def = Resources.Load<Sprite>("DefaultLevel");
                if (def != null) levelPicture.sprite = def;
            }
            levelName.text = levelInfo.Name;
            levelDesc.text = levelInfo.Description;

            int minutes = levelInfo.Seconds / 60;
            int seconds = levelInfo.Seconds;
            if (minutes > 0)
            {
                seconds %= 60;
            }
            time.text = $"{minutes}мин {seconds}с";
            for(int i = 0; i < levelInfo.Rate; i++)
            {
                if (i > stars.Length-1) return;
                stars[i].color = Color.yellow;
            }
            buttonPlay.onClick.RemoveAllListeners();
            buttonPlay.onClick.AddListener(LoadLevel);
        }
    }
    void LoadLevel()
    {
        DataTable scoreboard = DB.GetTable("SELECT Id FROM CurrentLevel;");
        if(int.TryParse(scoreboard.Rows[0][0].ToString(), out int result))
        {
            DB.ExecuteQueryWithoutAnswer($"UPDATE CurrentLevel SET LevelId = '{levelInfo.Id}' Where Id = {result}");
            Scenes.SwitchScene("Gameplay");
        }
    }
}
