using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public bool HasWater;
    public List<SerializableFoodInfo> RecipeContent;
    public Recipe()
    {
        RecipeContent = new List<SerializableFoodInfo>();
    }
    //public static List<SerializableFoodInfo> RemoveDuplicatesFoods(List<SerializableFoodInfo> recipe)
    //{
    //    List<SerializableFoodInfo> newRecipe = new List<SerializableFoodInfo>();
    //    for (int i = 0; i < recipe.Count; i++)
    //    {
    //        if (!newRecipe.Contains(recipe[i]))
    //        {
    //            newRecipe.Add(recipe[i]);
    //        }
    //        else
    //        {
    //            newRecipe[newRecipe.IndexOf(recipe[i])].Count++;
    //        }
    //    }
    //    return newRecipe;
    //}
    public static List<SerializableFoodInfo> RemoveDuplicates(List<SerializableFoodInfo> recipe)
    {
        List<SerializableFoodInfo> newRecipe = new List<SerializableFoodInfo>();
        for (int i = 0; i < recipe.Count; i++)
        {
            var containsElement = newRecipe.FirstOrDefault(f => f.FoodId == recipe[i].FoodId && f.GramsWeight == recipe[i].GramsWeight);
            if (containsElement == null)
            {
                newRecipe.Add(recipe[i].Clone());
            } 
            else
            {           
                containsElement.Count++;             
            }
        }
        for (int i = 0; i < newRecipe.Count; i++)
        {
            newRecipe[i].Params.Clear();
            var allFoods = recipe.Where(f => f.FoodId == newRecipe[i].FoodId && f.GramsWeight == newRecipe[i].GramsWeight).ToList();
            foreach(var food in allFoods)
            {
                foreach(var param in food.Params)
                {
                    var key = newRecipe[i].Params.FirstOrDefault(k => k.ParamName == param.ParamName);
                    if (key == null)
                    {
                        newRecipe[i].Params.Add(param);
                    }
                    else
                    {
                        key.ParamValue += param.ParamValue;
                    }
                }
            }
            foreach (var newParam in newRecipe[i].Params)
            {
                newParam.ParamValue /= allFoods.Count;
            }
        }

        return newRecipe;
    }
}
