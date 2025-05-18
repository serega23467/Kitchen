using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListElement : RecyclingListViewItem
{
    [SerializeField]
    TMP_Text foodName;
    [SerializeField]
    TMP_Text foodWeight;
    [SerializeField]
    TMP_Text foodCount;
    [SerializeField]
    public Button Button;
    public bool HasPlate = false;
    public bool CanPull = false;
    FoodComponent food;

    int count = 0;
    public int Count
    {
        get { return count; }
        set 
        { 
            count = value;
            foodCount.text = count.ToString() + " רע";
        }
    }

    public FoodComponent Food
    {     
        get { return food; }
        set
        {
            food = value;
            foodName.text = value.FoodInfo.FoodName;
            foodWeight.text = value.FoodInfo.GramsWeight.ToString("N1") + " ד";
            if(!HasPlate || !CanPull)
            {
                Button.gameObject.transform.localScale = Vector3.zero;
            }    
            else
            {
                Button.gameObject.transform.localScale = Vector3.one;
            }
        }
    }
}
