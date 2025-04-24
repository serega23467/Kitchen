using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour
{
    [SerializeField]
    float maxWeight = 500f;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    Vector3 randomRange = Vector3.zero;
    ShowObjectInfo info;

    public List<FoodComponent> Food { get; private set; }
    private void Start()
    {
        Food = new List<FoodComponent>();
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Тарелка";
    }
    public bool TryAddFood(FoodComponent food)
    {
        if (Food.Sum(f => f.FoodInfo.GramsWeight) + food.FoodInfo.GramsWeight > maxWeight)
        {
            return false;
        }
        else
        {
            Food.Add(food);
            Vector3 randomOffset = Vector3.zero;
            randomOffset.x = UnityEngine.Random.Range(-randomRange.x, randomRange.x);
            randomOffset.y = UnityEngine.Random.Range(-randomRange.y, randomRange.y);
            randomOffset.z = UnityEngine.Random.Range(-randomRange.z, randomRange.z);

            food.gameObject.transform.position = transform.position+offset+randomOffset;
            food.gameObject.transform.parent = transform;
            food.plate = this;
            UpdateInfo();
            return true;
        }
    }
    public void RemoveFood(FoodComponent food)
    {
        Food.Remove(food);
        food.transform.parent = Parents.GetInstance().FoodParent.transform;
        food.plate = null;
        UpdateInfo();   
    }
    public void UpdateInfo()
    {
        if (Food.Count<1)
        {
            info.ObjectData = "";
            return;
        }
        List<FoodComponent> uniqFood = Food.DistinctBy(f => f.FoodName).ToList();
        StringBuilder sb = new StringBuilder();
        foreach (var f in uniqFood)
        {
            var weight = Food.Where(food => food.FoodName == f.FoodName).Sum(f => f.FoodInfo.GramsWeight);
            sb.AppendLine($"{f.FoodInfo.FoodName} - {weight}\n({Translator.GetInstance().GetTranslate(f.FoodInfo.CurrentCutType.ToString())})");
        }
        info.ObjectData = sb.ToString();
    }    
}
