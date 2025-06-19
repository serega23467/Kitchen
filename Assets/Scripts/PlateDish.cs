using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlateDish : MonoBehaviour, IFinish
{
    [SerializeField]
    GameObject content;
    [SerializeField]
    bool canFinish = true;
    ShowObjectInfo info;
    public ObservableCollection<FoodComponent> Foods { get; set; }
    public bool HasWater { get; set; }
    public bool CanFinish { get=>canFinish; }

    Recipe recipe;
    MeshRenderer contentMaterial;
    Texture contentDefaultTexture;
    Texture2D contentEtalonTexture;

    private void Awake()
    {
        HasWater = true;
        Foods = new ObservableCollection<FoodComponent>();
    }
    private void Start()
    {
        contentMaterial = content.GetComponent<MeshRenderer>();
        if (contentMaterial != null)
        {
            contentDefaultTexture = contentMaterial.material.mainTexture;
            contentEtalonTexture = Resources.Load<Texture2D>("Levels/" + SettingsInit.CurrentLevelName + "/" + BellFinish.Level.RecipeFoodPictureName);
        }
        content.SetActive(false);

        Foods.CollectionChanged += UpdateContent;
        info = GetComponent<ShowObjectInfo>();
    }
    public void AddDish(ObservableCollection<FoodComponent> foods, bool hasWater)
    {
        Foods.Clear();
        if (foods.Count > 0)
        {
            Foods.AddRange(foods);           
        }

    }
    void UpdateContent(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (Foods.Count > 0)
        {
            content.SetActive(true);
        }
        else
        {
            content.SetActive(false);
            return;
        }
        if (contentMaterial != null && contentEtalonTexture != null && contentDefaultTexture != null)
        {
            var etalonFoods = BellFinish.Level.Recipe.RecipeContent;
            if (CompareWithEtalon(etalonFoods))
            {
                contentMaterial.material.mainTexture = contentEtalonTexture;
            }
            else
            {
                if (contentMaterial.material.mainTexture != contentDefaultTexture)
                {
                    contentMaterial.material.mainTexture = contentDefaultTexture;
                }
            }
        }
        else
        {
            content.SetActive(false);
        }
    }
    bool CompareWithEtalon(List<SerializableFoodInfo> foods)
    {
        var dishFood = Foods.Select(f => f.FoodInfo.GetSerializableFoodInfo()).ToList();
        dishFood = Recipe.RemoveDuplicates(dishFood);
        if(dishFood.Count != foods.Count)
        {
            return false;
        }
        var sortedFoodsEtalon = dishFood.OrderBy(f=>f.FoodId).ThenBy(f=>f.CurrentCutType).ToList();
        var sortedFoodsCurrent = foods.OrderBy(f => f.FoodId).ThenBy(f => f.CurrentCutType).ToList();

        for (int i = 0; i<foods.Count; i++)
        {
            if (sortedFoodsEtalon[i].FoodId != sortedFoodsCurrent[i].FoodId)
            {
                return false;
            }
            else if (sortedFoodsEtalon[i].GramsWeight - sortedFoodsCurrent[i].GramsWeight > 0.001f)
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
}
