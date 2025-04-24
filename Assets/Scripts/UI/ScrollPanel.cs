using System.Collections.Generic;
using UnityEngine;

public class ScrollPanel : MonoBehaviour
{
    public RecyclingListView theList;
    private List<FoodInfo> data = new List<FoodInfo>();
    void Start()
    {
        RetrieveData(new List<FoodInfo>());
    }

    public void RetrieveData(List<FoodInfo> food)
    {
        theList.ItemCallback = PopulateItem;
        data.Clear();
        if (food.Count > 0)
        {
            data.AddRange(food);
        }
        theList.RowCount = data.Count;
    }
    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as ListElement;
        child.Food = data[rowIndex];
    }
}
