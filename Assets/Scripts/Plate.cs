using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour
{
    [SerializeField]
    GameObject content;
    ShowObjectInfo info;
    public IFood Food { get; private set; }
    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Тарелка";
        content.SetActive(false);
    }
    public void AddFood(IFood food)
    {
        if(Food == null || Food.FoodGameObject != food.FoodGameObject)
        {
            Food = food.CloneFood();
        }
        else if (Food.FoodGameObject == food.FoodGameObject)
        {
            Food.GramsWeight += food.GramsWeight;
        }
        UpdateInfo();
        content.SetActive(true);        
    }
    public void RemoveFood(out IFood food)
    {
        food = Food;
        if (Food != null)
        {
            content.SetActive(false);
            Food = null;
            UpdateInfo();
        }
    }
    public void UpdateInfo()
    {
        if(Food == null)
        {
            info.ObjectData = "";
            return;
        }
        info.ObjectData = $"{Food.FoodName} - {Food.GramsWeight} г\nнарезка - {Translator.GetInstance().GetTranslate(Food.CurrentCutType.ToString())}";
    }    
}
