using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LevelsMenu : MonoBehaviour, IHideble
{
    [SerializeField]
    RecyclingListView theList;
    List<Level> levelsList;

    public bool IsActive => gameObject.activeSelf;

    private void Awake()
    {
        levelsList = new List<Level>();
    }
    private void Start()
    {
        Hide();
    }
    public void UpdateLevels()
    {
        var levels = DB.GetLevels();
        RetrieveData(levels);
    }
    public void Hide()
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
