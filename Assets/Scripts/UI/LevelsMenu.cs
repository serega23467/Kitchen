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
        DataTable scoreboard = DB.GetTable("SELECT * FROM Levels;");
        for (int i = 0; i < scoreboard.Rows.Count; i++)
        {
            var info = new Level() { 
                Id = int.Parse(scoreboard.Rows[i][0].ToString()),
                Name = scoreboard.Rows[i][1].ToString(), 
                Description = scoreboard.Rows[i][2].ToString(), 
                ImageName= scoreboard.Rows[i][3].ToString(), 
                Rate = int.Parse(scoreboard.Rows[i][4].ToString()),
                Seconds = int.Parse(scoreboard.Rows[i][5].ToString()),
                RecipeId = int.Parse(scoreboard.Rows[i][6].ToString())
            };
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
}
