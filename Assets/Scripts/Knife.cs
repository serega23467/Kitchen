using System;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public void Cut(FoodComponent food)
    {
        if(food.FoodInfo.CurrentCutType==CutType.Finely || food.FoodInfo.AllCutTypes.Length<=1 || food.FoodInfo.IsPour)
        {
            return;
        }
        Array.Sort(food.FoodInfo.AllCutTypes);
        int currentIndex = Array.IndexOf(food.FoodInfo.AllCutTypes, food.FoodInfo.CurrentCutType);
        if (currentIndex != -1)
        {
            if (currentIndex < food.FoodInfo.AllCutTypes.Length - 1)
            {
                food.FoodInfo.CurrentCutType = food.FoodInfo.AllCutTypes[currentIndex + 1];
            }
        }
        food.Cut();
    }
}
