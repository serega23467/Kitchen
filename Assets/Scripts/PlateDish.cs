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
        Etalon = new List<IFood>()
        {
            new Chicken(){CurrentCutType =CutType.Medium, GramsWeight = 500, TemperatureSum = 272/2, TemperatureWithoutWaterSum = 0},
            new Rice(){GramsWeight = 160, TemperatureSum = 157/2, TemperatureWithoutWaterSum = 0},
            new Salt(){GramsWeight = 20, TemperatureSum = 88 / 2, TemperatureWithoutWaterSum = 0},
            new Butter(){GramsWeight=40, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7},
            new Carrot(){GramsWeight = 75, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            new Onion(){GramsWeight = 100, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            new Garlic(){GramsWeight = 100, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            new TomatoPasta(){GramsWeight=60, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7},
            new Dill(){GramsWeight=30, TemperatureSum=7, TemperatureWithoutWaterSum=0 }

        };
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
                sb.Append("������ ���������� ������������!\n");
                isGood = false;
                result = sb.ToString();
                return isGood;
            }
            for (int i = 0; i < Etalon.Count; i++)
            {
                if (foods[i].FoodName != etalon[i].FoodName)
                {
                    sb.Append($"�� ������� �����������: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].GramsWeight - etalon[i].GramsWeight) > 5)
                {
                    sb.Append($"������������ �������� � {foods[i]}, {foods[i].GramsWeight}/{etalon[i].GramsWeight}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].TemperatureSum - etalon[i].TemperatureSum) > 100)
                {
                    sb.Append($"���������: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
                else if (Mathf.Abs(foods[i].TemperatureWithoutWaterSum - etalon[i].TemperatureWithoutWaterSum) > 100)
                {
                    sb.Append($"���������: {etalon[i].FoodName}!\n");
                    isGood = false;
                }
                else if (foods[i].CurrentCutType != etalon[i].CurrentCutType)
                {
                    sb.Append($"����� ���� ��������: {etalon[i].CurrentCutType}, � �� {foods[i].CurrentCutType}!\n");
                    isGood = false;
                }
            }
        }
        else
        {
            sb.Append("���� ������������!\n");
            isGood = false;
        }
        result = sb.ToString();
        return isGood;
    }
}
