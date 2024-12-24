using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Water : MonoBehaviour
{
    public bool IsBoiled { get; private set; }
    public bool IsBoilingNow { get; private set; }
    public List<IFood> Foods { get; private set; }
    public void HeatFood(float t)
    {
        foreach(IFood food in Foods)
        {
            food.TemperatureSum += t;
        }
    }
    public void AddFood(IFood food)
    {
        if(Foods.Contains(food))
        {
            byte index = (byte)Foods.FindIndex(f=>f.Equals(food));
            if (index >= 0)
                Foods[index].GramsWeight += food.GramsWeight;
        }
        else
        { 
            Foods.Add(food);
        }
    }
}
