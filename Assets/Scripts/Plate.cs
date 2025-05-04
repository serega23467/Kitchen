using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<string> OnUpdateInfo;
    [SerializeField]
    float maxWeight = 500f;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    Vector3 randomRange = Vector3.zero;
    ShowObjectInfo info;

    List<FoodComponent> foods;
    private void Awake()
    {
        OnUpdateInfo = new UnityEvent<string>();
    }
    private void Start()
    {
        foods = new List<FoodComponent>();
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Тарелка";
    }
    public List<FoodComponent> GetFoodList()
    {
        return foods; 
    }
    public bool TryAddFood(FoodComponent food)
    {
        if (food == null) return false;
        if (this.foods.Sum(f => f.FoodInfo.GramsWeight) + food.FoodInfo.GramsWeight > maxWeight)
        {
            return false;
        }
        else
        {
            if (food.plate != null)
            {
                var plate2 = food.plate;
                plate2.RemoveFood(food);
            }

            this.foods.Add(food);
            Vector3 randomOffset = Vector3.zero;
            randomOffset.x = UnityEngine.Random.Range(-randomRange.x, randomRange.x);
            randomOffset.y = UnityEngine.Random.Range(-randomRange.y, randomRange.y);
            randomOffset.z = UnityEngine.Random.Range(-randomRange.z, randomRange.z);

            food.gameObject.transform.position = transform.position + offset + randomOffset;
            food.gameObject.transform.parent = transform;
            food.plate = this;
            UpdateInfo();
            return true;
        }
    }
    public void RemoveFood(FoodComponent food)
    {
        if (food == null) return;
        this.foods.Remove(food);
        food.plate = null;
        UpdateInfo();   
    }
    public void SpiceFood(SpiceComponent spice)
    {
        foreach(FoodComponent food in foods)
        {
            spice.AddSpiceTo(food);
        }
    }
    public List<FoodComponent> MoveAllFood()
    {
        foreach (FoodComponent food in foods)
        {
            food.transform.parent = Parents.GetInstance().FoodParent.transform;
            food.plate = null;
        }
        List<FoodComponent> list = foods;
        foods = new List<FoodComponent>();
        UpdateInfo();
        return list;
    }
    public void UpdateInfo()
    {
        if (foods.Count<1)
        {
            info.ObjectData = "";
            return;
        }
        List<FoodComponent> uniqFood = foods.DistinctBy(f => f.FoodName).ToList();
        StringBuilder sb = new StringBuilder();
        foreach (var f in uniqFood)
        {
            var weight = foods.Where(food => food.FoodName == f.FoodName).Sum(f => f.FoodInfo.GramsWeight);
            sb.AppendLine($"{f.FoodInfo.FoodName} - {weight.ToString("N1") + " г"}\n({Translator.GetInstance().GetTranslate(f.FoodInfo.CurrentCutType.ToString())})");
        }
        info.ObjectData = sb.ToString();
        OnUpdateInfo.Invoke(sb.ToString());
    }    
}
