using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem.HID;

public class ScrollPanel : MonoBehaviour, IHideble
{
    [SerializeField]
    private RecyclingListView theList;
    public bool IsActive => gameObject.activeSelf;
    private List<FoodComponent> allElements = new List<FoodComponent>();
    private List<FoodComponent> data = new List<FoodComponent>();
    bool hasPlateInHands = false;
    bool canPullFood = false;


    void Start()
    {
        RetrieveData(new List<FoodComponent>());
        Hide();
    }

    public void RetrieveData(List<FoodComponent> food, bool hasPlate = true, bool canPull = false)
    {
        hasPlateInHands = hasPlate;
        canPullFood = canPull;
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
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        Clear();
        gameObject.SetActive(false);
    }
    void Clear()
    {
        RetrieveData(new List<FoodComponent>());
    }
    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {
        var child = item as ListElement;
        child.CanPull = canPullFood;
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
