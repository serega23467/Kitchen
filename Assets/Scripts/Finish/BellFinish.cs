using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BellFinish : MonoBehaviour
{
    [SerializeField]
    TableCollisions table;
    [SerializeField]
    AudioSource dingSource;
    public static LevelInfo Level { get; private set; }
    private void Awake()
    {
        SettingsInit.UpdateCurrentLevelName();
        Level = JsonLoader.LoadLevelInfo(SettingsInit.CurrentLevelName);      
    }
    private void Start()
    {
        if (dingSource != null) dingSource.volume = 0.2f;
    }
    public void DingBell(Action OnFinishMenuOpen)
    {
        var finishPlate = table.finishPlate;

        if (finishPlate!=null)
        {
            SaveDish(finishPlate.GetRecipe());
            //int rate = Compare(out string result, Level.Recipe, finishPlate.GetRecipe());
            //int time = UIElements.GetInstance().GetTimerTime();
            //if(dingSource!=null) dingSource.Play();
            //OnFinishMenuOpen?.Invoke();
            //UIElements.GetInstance().ShowPanelResult(Level, rate, time, result);
            //DB.UpdateLevelInfo(rate, time);
        }
        else
        {
            UIElements.ShowToast($"�������� ����� �� ����!");
        }

    }
    public void SaveDish(Recipe recipe)
    {
        Level = new LevelInfo();
        Level.RecipeTextPictureName = "";
        Level.RecipeFoodPictureName = "";
        Level.Recipe = recipe;
        Level.CookTime = UIElements.GetInstance().GetTimerTime();
        JsonLoader.SaveLevelInfo(Level, "CremlinMeat");

        UIElements.ShowToast($"�������!");
    }
    public int Compare(out string result, Recipe recipeEtalon, Recipe recipe)
    {
        result = "";

        bool hasRightAmountOfProducts = true;
        bool hasCorrectCookParams = true;
        bool hasCorrectSpiceParams = true;

        int score = 0;
        StringBuilder sb = new StringBuilder();
        if(recipe.HasWater!=recipeEtalon.HasWater)
        {
            if (recipeEtalon.HasWater)
            {
                sb.AppendLine("����� ������ ���� �����, ���������� � ������� �������!");
            }
            else
            {
                sb.AppendLine("����� ������ ���������� � ������� �������!");
            }
            result = sb.ToString();

            return 0;
        }

        var recipeContent = Recipe.RemoveDuplicates(recipe.RecipeContent);
        var etalonContent = recipeEtalon.RecipeContent;

        foreach(var r in recipeContent.DistinctBy(f=>f.FoodId))
        {
            if (!etalonContent.Select(f => f.FoodId).Contains(r.FoodId))
            {
                sb.AppendLine("������ ������� " + r.FoodName);
                hasRightAmountOfProducts = false;
            }
            else
            {
                var foods = recipeContent.Where(f => f.FoodId == r.FoodId);
                var etalonFoods = etalonContent.Where(f => f.FoodId == r.FoodId);
                foreach (var food in foods)
                {
                    if(!etalonFoods.Select(f=>f.CurrentCutType).Contains(food.CurrentCutType))
                    {
                        sb.AppendLine("������������ ������� " + food.FoodName);
                        hasRightAmountOfProducts = false;
                    }
                }
            }
        }

        foreach (var etalonFood in etalonContent)
        {
            var foodsWithEtalonId = recipeContent.Where(f => f.FoodId == etalonFood.FoodId);
            if (foodsWithEtalonId == null || foodsWithEtalonId.Count()<=0)
            {
                sb.AppendLine("��� " + etalonFood.FoodName);
                hasRightAmountOfProducts = false;
            }
            else 
            {
                var currentFood = foodsWithEtalonId.FirstOrDefault(f => f.CurrentCutType == etalonFood.CurrentCutType);
                if (currentFood == null)
                {
                    continue;
                }
                if (GetDiffPercent(currentFood.Count, etalonFood.Count) > 20f || GetDiffPercent(currentFood.Count, etalonFood.Count) < -20f)
                {
                    sb.AppendLine("�� ������������� ���������� " + etalonFood.FoodName + $" {currentFood.Count}/{etalonFood.Count}");
                    hasRightAmountOfProducts = false;
                }
                else 
                {
                    foreach (var realP in currentFood.Params)
                    {
                        var paramInEtalon = currentFood.Params.FirstOrDefault(p => p.ParamName == realP.ParamName);
                        if (paramInEtalon == null && realP.ParamValue>10)
                        {
                            if (FoodParametersPool.GetInstance().TryGetParameter(realP.ParamName, out FoodParametr fullRealParam))
                            {
                                sb.AppendLine($"{currentFood}: " + fullRealParam.NoNeedThisError);
                            }
                            else
                            {
                                sb.AppendLine($"{currentFood}: ������: " + realP.ParamName);
                            }
                            if (realP.IsSpice)
                            {
                                hasCorrectSpiceParams = false;
                            }
                            else
                            {
                                hasCorrectCookParams = false;
                            }
                        }
                    }
                    foreach (var paramEtalon in etalonFood.Params)
                    {
                        var paramInFoods = currentFood.Params.FirstOrDefault(p=>p.ParamName== paramEtalon.ParamName);
                        if(paramInFoods == null)
                        {
                            if(FoodParametersPool.GetInstance().TryGetParameter(paramEtalon.ParamName, out FoodParametr fullParamEtalon))
                            {
                                sb.AppendLine(fullParamEtalon.NoThisError + " " + etalonFood.FoodName);
                            }
                            else
                            {
                                sb.AppendLine("���: " + paramEtalon.ParamName + " " + etalonFood.FoodName);
                            }
                            if(!fullParamEtalon.IsSpice)
                            {
                                hasCorrectCookParams = false;
                            }
                            else
                            {
                                hasCorrectSpiceParams = false;
                            }
                        }
                        else if (FoodParametersPool.GetInstance().TryGetParameter(paramInFoods.ParamName, out FoodParametr fullParam))
                        {
                            fullParam.ParamValue = paramInFoods.ParamValue;

                            float error = GetDiffPercent(paramEtalon.ParamValue, fullParam.ParamValue);
                            if (error > 20f || error < -20f)
                            {
                                if(error > 20f)
                                {
                                    sb.AppendLine(fullParam.TooLittleError + " " + etalonFood.FoodName);
                                }
                                else
                                {
                                    sb.AppendLine(fullParam.TooMuchError + " " + etalonFood.FoodName);
                                }

                                if(!fullParam.IsSpice)
                                {
                                    hasCorrectCookParams = false;
                                }
                                else
                                {
                                    hasCorrectSpiceParams = false;
                                }
                            }
                        }
                    }
                }
            }

        }
        result = sb.ToString();
        if(hasRightAmountOfProducts && hasCorrectCookParams)
        {
            score++;
            if (hasCorrectSpiceParams)
            {
                score++;
                if(Level.CookTime>=UIElements.GetInstance().GetTimerTime())
                {
                    score++;
                }
            }
        }
        return score;
    }
    public static float GetDiffPercent(float a, float b)
    {
        float dif = a - b;
        float avg = (a + b) / 2;

        float perc = (dif / avg) * 100;
        return perc;
    }
}
