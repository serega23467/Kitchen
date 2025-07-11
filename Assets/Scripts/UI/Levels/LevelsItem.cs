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

            Sprite loadedSprite = Resources.Load<Sprite>("Levels/" + levelInfo.FolderName + "/" + levelInfo.ImageName);
            if (loadedSprite != null)
            {
                levelPicture.sprite = loadedSprite; 
            }
            else
            {
                Sprite def = Resources.Load<Sprite>("Levels/" + "DefaultLevel");
                if (def != null) levelPicture.sprite = def;
            }
            levelName.text = levelInfo.Name;
            levelDesc.text = levelInfo.Description;

            if (levelInfo.Seconds > 0)
                time.text = Translator.GetTimeBySeconds(levelInfo.Seconds);
            else
                time.text = "";

            for(int i = 0; i < levelInfo.Rate; i++)
            {
                if (i > stars.Length-1) return;
                stars[i].color = Color.yellow;
            }
            buttonPlay.onClick.RemoveAllListeners();
            if(levelInfo.IsLocked)
            {
                buttonPlay.gameObject.SetActive(false);
            }
            else
            {
                buttonPlay.gameObject.SetActive(true);
                buttonPlay.onClick.AddListener(LoadLevel);
            }
        }
    }
    void LoadLevel()
    {
        if(DB.TryUpdateCurrentLevel(levelInfo.Id))
        {         
            SceneLoader.PlayHideOnAwake = true;
            SceneLoader.SwitchScene("Gameplay");
            AudioManager.Instance.PlayMusic("gameplay");
        }
    }
}
