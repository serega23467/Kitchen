using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour, IListable, IFinish
{
    [HideInInspector]
    public UnityEvent<string> OnUpdateInfo;
    [SerializeField]
    float maxWeight = 500f;
    [SerializeField]
    bool canFinish = true;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    Vector3 randomRange = Vector3.zero;
    ShowObjectInfo info;


    public ObservableCollection<FoodComponent> Foods { get; set; }
    public bool HasWater { get; set; }
    public bool CanFinish { get=> canFinish; set=> canFinish=value; }

    private void Awake()
    {
        OnUpdateInfo = new UnityEvent<string>();
        HasWater = false;
    }
    private void Start()
    {
        Foods = new ObservableCollection<FoodComponent>();
        info = GetComponent<ShowObjectInfo>();
        //info.ObjectName = "Тарелка";
    }
    public bool TryAddFood(FoodComponent food)
    {
        if (food == null) return false;
        if (this.Foods.Sum(f => f.FoodInfo.GramsWeight) + food.FoodInfo.GramsWeight > maxWeight)
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

            this.Foods.Add(food);
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
        this.Foods.Remove(food);
        food.plate = null;

        if(food.gameObject.layer != LayerMask.NameToLayer("DraggableObject"))
        {

            food.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
            foreach (Transform child in food.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
            }
        }

        UpdateInfo();   
    }
    public void SpiceFood(SpiceComponent spice)
    {
        foreach(FoodComponent food in Foods)
        {
            spice.AddSpiceTo(food);
        }
    }
    public List<FoodComponent> MoveAllFood()
    {
        foreach (FoodComponent food in Foods)
        {
            food.transform.parent = Parents.GetInstance().FoodParent.transform;
            food.plate = null;
        }
        List<FoodComponent> list = Foods.ToList();
        Foods = new ObservableCollection<FoodComponent>();
        UpdateInfo();
        return list;
    }
    public void UpdateInfo()
    {
        if (Foods.Count<1)
        {
            info.ObjectData = "";
            OnUpdateInfo.Invoke("");
            return;
        }
        List<FoodComponent> uniqFood = Foods.DistinctBy(f => f.FoodName).ToList();
        StringBuilder sb = new StringBuilder();
        foreach (var f in uniqFood)
        {
            var weight = Foods.Where(food => food.FoodName == f.FoodName).Sum(f => f.FoodInfo.GramsWeight);
            sb.AppendLine($"{f.FoodInfo.FoodName} - {weight.ToString("N1") + " г"}\n({Translator.GetInstance().GetTranslate(f.FoodInfo.CurrentCutType.ToString())})");
        }
        info.ObjectData = sb.ToString();
        OnUpdateInfo.Invoke(sb.ToString());
    }

    public Recipe GetRecipe()
    {
        ObservableCollection<FoodComponent> foods = new ObservableCollection<FoodComponent>();
        for (int i = 0; i < Foods.Count; i++)
        {
            foods.Add(Foods[i].Clone() as FoodComponent);
        }

        Recipe recipe = new Recipe();
        recipe.RecipeContent = Recipe.RemoveDuplicates(foods.Select(f => f.FoodInfo.GetSerializableFoodInfo()).ToList());
        recipe.HasWater = HasWater;

        return recipe;  
    }

    public void SetFinishOutline(bool hasOutline)
    {
        info.SetFinishOutline(hasOutline);
    }
}
