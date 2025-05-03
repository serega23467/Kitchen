using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem.HID;

public class ScrollPanel : MonoBehaviour
{
    public RecyclingListView theList;
    private List<FoodComponent> allElements = new List<FoodComponent>();
    private List<FoodComponent> data = new List<FoodComponent>();
    bool hasPlateInHands = false;
    void Start()
    {
        RetrieveData(new List<FoodComponent>());
    }

    public void RetrieveData(List<FoodComponent> food, bool hasPlate = true)
    {
        hasPlateInHands = hasPlate;

        theList.ItemCallback = PopulateItem;
        data.Clear();
        allElements.Clear();
        if (food.Count > 0)
        {
            allElements.AddRange(food);
            List<FoodComponent> uniqFood = allElements.DistinctBy(f => f.FoodName).ToList();
            data.AddRange(uniqFood);
        }
        theList.RowCount = data.Count;
        theList.Refresh();
    }
    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as ListElement;
        child.HasPlate = hasPlateInHands;
        child.Food = data[rowIndex];
        child.Count = allElements.Where(f=>f.FoodName == data[rowIndex].FoodName).Count();

        var foods = allElements.Where(f => f.FoodName == child.Food.FoodName).ToList();

        child.Button.onClick.RemoveAllListeners();
        child.Button.onClick.AddListener(delegate { UIElements.GetInstance().OpenSliderMenu(GetFood, foods); } );
    }
    void GetFood(List<FoodComponent> foods, int count)
    {
        if (foods.Count < count - 1) return;
        for(int i = 0; i < count; i++) 
        {
            foods[i].OnPull.Invoke(foods[i]);
        }
    }
}
