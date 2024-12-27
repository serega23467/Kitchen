using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlateDish : MonoBehaviour, IListable
{
    [SerializeField]
    GameObject content;
    public List<IFood> Etalon { get; set; }
    public List<IFood> Foods { get; set; }
    private void Start()
    {
        Foods = new List<IFood>();
        Etalon = new List<IFood>();
        content.SetActive(false);
    }
    public void AddDish(List<IFood> foods)
    {
        Foods.Clear();
        if (foods.Count > 0)
        {
            content.SetActive(true);
            Foods.AddRange(foods);
        }
        else
        {
            content.SetActive(false);
        }

    }
    public bool Compare(out string result)
    {
        bool isGood = true;
        StringBuilder sb = new StringBuilder();
        if (Foods.Count > 0 && Etalon.Count > 0)
        {
            var foods = Foods.OrderBy(f => f.FoodName).ToList();
            var etalon = Etalon.OrderBy(f => f.FoodName).ToList();
            if (Foods.Count != Etalon.Count)
            {
                sb.Append("Разное количество ингридиентов!\n");
                isGood = false;
            }
            for (int i = 0; i < Etalon.Count; i++)
            {
                if (foods[i].FoodName != etalon[i].FoodName)
                {
                    sb.Append($"Не хватает ингридиента: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].GramsWeight - etalon[i].GramsWeight) > 5)
                {
                    sb.Append($"Неправильные грамовки у {foods[i]}, {foods[i].GramsWeight}/{etalon[i].GramsWeight}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].TemperatureSum - etalon[i].TemperatureSum) > 100)
                {
                    sb.Append($"Переварен: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].TemperatureWithoutWaterSum - etalon[i].TemperatureWithoutWaterSum) > 100)
                {
                    sb.Append($"Пережарен: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
            }
        }
        else
        {
            sb.Append("Нету ингридиентов!\n");
            isGood = false;
        }
        result = sb.ToString();
        return isGood;
    }
}
