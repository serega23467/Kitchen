using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BellFinish : MonoBehaviour
{
    [SerializeField]
    TableCollisions table;
    public static LevelInfo Level { get; private set; }
    private void Awake()
    {
        SettingsInit.UpdateCurrentLevelName();
        Level = JsonLoader.LoadLevelInfo(SettingsInit.CurrentLevelName);      
    }
    public void DingBell()
    {
        var finishPlate = table.finishPlate;

        if (finishPlate!=null)
        {
            SaveDish(finishPlate.GetRecipe());
        }

    }
    public void SaveDish(Recipe recipe)
    {
        Level = new LevelInfo();
        Level.RecipeTextPictureName = "";
        Level.RecipeFoodPictureName = "";
        Level.Recipe = recipe;
        Level.CookTime = UIElements.GetInstance().GetTimerTime();
        JsonLoader.SaveLevelInfo(Level, "Harcho3");

        Debug.Log("Успешно");
    }
    public void Compare(out string result, string etalon, Recipe recipe)
    {
        bool isGood = true;
        result = "";
        StringBuilder sb = new StringBuilder();
        //if (Foods.Count > 0 && Etalon.Count > 0)
        //{
        //    var foods = Foods.OrderBy(f => f.FoodName).ToList();
        //    var etalon = Etalon.OrderBy(f => f.FoodName).ToList();
        //    if (Foods.Count != Etalon.Count)
        //    {
        //        sb.Append("Разное количество ингридиентов!\n");
        //        isGood = false;
        //        result = sb.ToString();
        //        return isGood;
        //    }
        //    for (int i = 0; i < Etalon.Count; i++)
        //    {
        //        string cutTypeEtalon = Translator.GetInstance().GetTranslate(etalon[i].CurrentCutType.ToString());
        //        string cutTypeCurrent = Translator.GetInstance().GetTranslate(foods[i].CurrentCutType.ToString());
        //        if (foods[i].FoodName != etalon[i].FoodName)
        //        {
        //            sb.Append($"Не хватает ингридиента: {etalon[i].FoodName}!\n");
        //            isGood = false;
        //        }
        //        else if (Mathf.Abs(foods[i].GramsWeight - etalon[i].GramsWeight) > 5)
        //        {
        //            sb.Append($"Неправильные грамовки у {foods[i].FoodName}, {foods[i].GramsWeight}/{etalon[i].GramsWeight}!\n");
        //            isGood = false;
        //        }
        //        else if (Mathf.Abs(foods[i].TemperatureSum - etalon[i].TemperatureSum) > 100)
        //        {
        //            sb.Append($"Переварен: {etalon[i].FoodName}!\n");
        //            isGood = false;
        //        }
        //        else if (Mathf.Abs(foods[i].TemperatureWithoutWaterSum - etalon[i].TemperatureWithoutWaterSum) > 100)
        //        {
        //            sb.Append($"Пережарен: {etalon[i].FoodName}!\n");
        //            isGood = false;
        //        }
        //        else if (foods[i].CurrentCutType != etalon[i].CurrentCutType)
        //        {
        //            sb.Append($"Нужно было нарезать: {cutTypeEtalon}, а не {cutTypeCurrent}!\n");
        //            isGood = false;
        //        }
        //    }
        //}
        //else
        //{
        //    sb.Append("Нету ингридиентов!\n");
        //    isGood = false;
        //}
        //result = sb.ToString();
        //return isGood;
    }
}
