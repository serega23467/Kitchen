using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour, IListable, IFinish
{

    public bool CanPull { get; private set; } = true;
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
    [SerializeField]
    GameObject content;
    bool isShowedContent = false;
    ShowObjectInfo info;
    MeshRenderer contentMaterial;
    Texture2D contentEtalonTexture;

    [HideInInspector]
    public float MaxWeight { get => maxWeight; }
    public ObservableCollection<FoodComponent> Foods { get; set; }
    public bool HasWater { get; set; }
    public bool CanFinish { get=> canFinish; }

    private void Awake()
    {
        OnUpdateInfo = new UnityEvent<string>();
        HasWater = false;
    }
    private void Start()
    {
        Foods = new ObservableCollection<FoodComponent>();
        Foods.CollectionChanged += UpdateInfo;
        if(content!= null)
        {
            contentMaterial = content.GetComponent<MeshRenderer>();
            if (contentMaterial != null)
            {
                contentEtalonTexture = Resources.Load<Texture2D>("Levels/" + SettingsInit.CurrentLevelName + "/" + BellFinish.Level.RecipeFoodPictureName);
                contentMaterial.material.mainTexture = contentEtalonTexture;
            }
            content.SetActive(false);
        }
        info = GetComponent<ShowObjectInfo>();
        info.ObjectData = $"Макс вес - {maxWeight} г";
    }
    public bool TryAddFood(FoodComponent food)
    {
        if (food == null) return false;
        if (this.Foods.Sum(f => f.FoodInfo.GramsWeight) + food.FoodInfo.GramsWeight > maxWeight)
        {
            UIElements.ShowToast($"{food.FoodInfo.FoodName}: не добавлено. Максимальная масса {maxWeight}г превышена!");
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

            food.gameObject.transform.parent = transform;
            food.gameObject.transform.localPosition = Vector3.zero + offset + randomOffset;
            food.gameObject.transform.localRotation = Quaternion.identity;
            if(isShowedContent && content!=null)
            {
                food.gameObject.transform.localScale = Vector3.zero;
            }
            food.plate = this;
            food.OnPull.RemoveAllListeners();
            food.OnPull.AddListener(PutFood);
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
        Foods.Clear();
        return list;
    }
    public void UpdateInfo(object sender=null, NotifyCollectionChangedEventArgs e=null)
    {
        if (Foods.Count<1)
        {
            info.ObjectData = $"Макс вес - {maxWeight} г";
            OnUpdateInfo.Invoke(info.ObjectData);
            return;
        }
        List<FoodComponent> uniqFood = Foods.DistinctBy(f => f.FoodName).ToList();
        StringBuilder sb = new StringBuilder();
        foreach (var f in uniqFood)
        {
            var weight = Foods.Where(food => food.FoodName == f.FoodName).Sum(f => f.FoodInfo.GramsWeight);
            sb.AppendLine($"{f.FoodInfo.FoodName} - {weight.ToString("N1") + " г"}\n({Translator.GetInstance().GetTranslate(f.FoodInfo.CurrentCutType.ToString())})");
        }
        sb.AppendLine($"Макс вес - {maxWeight} г");
        info.ObjectData = sb.ToString();
        UpdateContent();
        OnUpdateInfo.Invoke(info.ObjectData);


    }
    void UpdateContent()
    {
        if (content == null) return;
        if (Foods.Count <= 0)
        {
            content.SetActive(false);
            return;
        }
        if (contentMaterial != null && contentEtalonTexture != null)
        {
            var etalonFoods = BellFinish.Level.Recipe.RecipeContent;
            if(BellFinish.Level.Recipe.HasWater)
            {
                content.SetActive(false);
                return;
            }
            if (CompareWithEtalon(etalonFoods))
            {
                foreach(var f in Foods)
                {
                    f.transform.localScale = Vector3.zero;
                }
                content.SetActive(true);
                isShowedContent = true;
                return;
            }

        }
        if (Foods.Select(f=>f.gameObject.transform).Any(t=>t.localScale==Vector3.zero))
        {
            foreach (var f in Foods)
            {
                f.transform.localScale = Vector3.one;
            }
        }
        content.SetActive(false);
    }
    bool CompareWithEtalon(List<SerializableFoodInfo> foods)
    {
        var dishFood = Foods.Select(f => f.FoodInfo.GetSerializableFoodInfo()).ToList();
        dishFood = Recipe.RemoveDuplicates(dishFood);
        if (dishFood.Count != foods.Count)
        {
            return false;
        }
        var sortedFoodsCurrent = dishFood.OrderBy(f => f.FoodId).ThenBy(f => f.CurrentCutType).ToList();
        var sortedFoodsEtalon = foods.OrderBy(f => f.FoodId).ThenBy(f => f.CurrentCutType).ToList();

        for (int i = 0; i < foods.Count; i++)
        {
            if (sortedFoodsCurrent[i].FoodId != sortedFoodsEtalon[i].FoodId)
            {
                return false;
            }
            else if (sortedFoodsCurrent[i].GramsWeight - sortedFoodsEtalon[i].GramsWeight > 0.01f)
            {
                return false;
            }
            else if (BellFinish.GetDiffPercent(sortedFoodsCurrent[i].Count, sortedFoodsEtalon[i].Count) > 20f || BellFinish.GetDiffPercent(sortedFoodsCurrent[i].Count, sortedFoodsEtalon[i].Count) < -20f)
            {
                return false;
            }
        }
        return true;
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

    public void PutFood(FoodComponent food)
    {
        if (Parents.GetInstance().Player.PickFood(food))
        {
            UIElements.GetInstance().UpdateObjectContent(this);
        }
    }
}
