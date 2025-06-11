using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField]
    RecyclingListView theList;
    List<Level> levelsList;
    private void Awake()
    {
        levelsList = new List<Level>();
    }
    public void UpdateLevels()
    {
        List<Level> levels = new List<Level>();
        DataTable scoreboard = DB.GetTable("SELECT l.Id, l.Name, l.Description, l.ImageName, l.Rate, l.Seconds, r.RecipeFileName " +
            "FROM Levels l JOIN Recipes r ON r.Id = l.RecipeId;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new Level() { 
                Id = int.Parse(scoreboard.Rows[i][0].ToString()),
                Name = scoreboard.Rows[i][1].ToString(), 
                Description = scoreboard.Rows[i][2].ToString(), 
                ImageName = scoreboard.Rows[i][3].ToString(), 
                Rate = int.Parse(scoreboard.Rows[i][4].ToString()),
                Seconds = int.Parse(scoreboard.Rows[i][5].ToString()),
                FolderName = scoreboard.Rows[i][6].ToString(),             
            };
            if(CheckLock(info.Id))
            {
                info.IsLocked = CheckLock(info.Id);
                info.ImageName = "";
            }
            levels.Add(info);
        }
        RetrieveData(levels);
    }
    public void HideLevelsMenu()
    {
        gameObject.SetActive(false);
    }
    void RetrieveData(List<Level> panels)
    {
        theList.ItemCallback = PopulateItem;
        levelsList.Clear();
        if (panels.Count > 0)
        {
            levelsList.AddRange(panels);
        }
        theList.RowCount = panels.Count;
        theList.Refresh();
    }
    void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as LevelsItem;
        child.LevelInfo = levelsList[rowIndex];
    }
    bool CheckLock(int levelId)
    {
        DataTable scoreboard = DB.GetTable($"SELECT LockedId FROM LockedLevels WHERE LockedId = {levelId}");
        if(scoreboard.Rows.Count>0)
        {
            return true;
        }
        return false;
    }
}
