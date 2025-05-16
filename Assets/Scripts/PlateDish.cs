using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class PlateDish : MonoBehaviour, IListable, IFinish
{
    [SerializeField]
    GameObject content;
    [SerializeField]
    bool canFinish = true;
    ShowObjectInfo info;
    public ObservableCollection<FoodComponent> Foods { get; set; }
    public bool HasWater { get; set; }
    public bool CanFinish { get => canFinish; set => canFinish = value; }

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
        info = GetComponent<ShowObjectInfo>();
    }
    public void AddDish(ObservableCollection<FoodComponent> foods, bool hasWater)
    {
        Foods.Clear();
        if (foods.Count > 0)
        {
            content.SetActive(true);
            Foods.AddRange(foods);

            var etalonFoods = BellFinish.Level.Recipe.RecipeContent;
            if (contentMaterial != null && contentEtalonTexture != null && contentDefaultTexture != null)
            {
                if (CompareWithEtalon(etalonFoods))
                {
                    contentMaterial.material.mainTexture = contentEtalonTexture;
                }
                else
                {
                    if(contentMaterial.material.mainTexture != contentDefaultTexture)
                    {
                        contentMaterial.material.mainTexture = contentDefaultTexture;
                    }
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
        var sortedFoods1 = dishFood.OrderBy(f=>f.FoodId).ThenBy(f=>f.CurrentCutType).ToList();
        var sortedFoods2 = foods.OrderBy(f => f.FoodId).ThenBy(f => f.CurrentCutType).ToList();

        for (int i = 0; i<foods.Count; i++)
        {
            if (sortedFoods1[i].FoodId != sortedFoods2[i].FoodId)
            {
                return false;
            }
            else if (sortedFoods1[i].GramsWeight != sortedFoods2[i].GramsWeight)
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
