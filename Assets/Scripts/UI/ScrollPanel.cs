using System.Collections.Generic;
using UnityEngine;

public class ScrollPanel : MonoBehaviour
{
    public RecyclingListView theList;
    private List<IFood> data = new List<IFood>();
    void Start()
    {
        RetrieveData(new List<IFood>());
    }

    public void RetrieveData(List<IFood> food)
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
