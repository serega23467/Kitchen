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
    Button button;
    FoodInfo food;

    public FoodInfo Food
    {     
        get { return food; }
        set
        {
            food = value;
            foodName.text = value.FoodName;
            foodWeight.text = value.GramsWeight.ToString();
            if(food.IsPour)
            {
                button.gameObject.transform.localScale = new Vector3(0,0,0);
            }    
            else
            {
                //button.onClick.AddListener (delegate { food.OnPull.Invoke(food.FoodGameObject); });
            }
        }
    }
}
