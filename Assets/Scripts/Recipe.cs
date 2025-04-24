using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public List<FoodInfo> RecipeContent { get; set; }
    public Recipe()
    {
        RecipeContent = new List<FoodInfo>();
    }
}
