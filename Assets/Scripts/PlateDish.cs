using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class PlateDish : MonoBehaviour, IListable
{
    [SerializeField]
    GameObject content;
    public List<FoodComponent> Etalon { get; set; }
    public List<FoodComponent> Foods { get; set; }
    private void Start()
    {
        Foods = new List<FoodComponent>();
        Etalon = new List<FoodComponent>()
        {
            //new FoodInfo(){FoodName="Chicken", CurrentCutType =CutType.Medium, GramsWeight = 500, TemperatureSum = 272/2, TemperatureWithoutWaterSum = 0},
            //new FoodInfo(){FoodName="Rice", GramsWeight = 120, TemperatureSum = 157/2, TemperatureWithoutWaterSum = 0},
            //new FoodInfo(){FoodName="Salt", GramsWeight = 20, TemperatureSum = 88 / 2, TemperatureWithoutWaterSum = 0},
            //new FoodInfo(){FoodName="Butter", GramsWeight=40, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7},
            //new FoodInfo(){FoodName="Carrot", GramsWeight = 75, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            //new FoodInfo(){FoodName="Onion", GramsWeight = 100, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            //new FoodInfo(){FoodName="Garlic", GramsWeight = 40, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7, CurrentCutType = CutType.Finely},
            //new FoodInfo(){FoodName="TomatoPasta", GramsWeight=60, TemperatureSum = 84/ 2, TemperatureWithoutWaterSum = 7},
            //new FoodInfo(){FoodName="Dill", GramsWeight=30, TemperatureSum=7, TemperatureWithoutWaterSum=0 }

        };
        content.SetActive(false);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    try
        //    {
        //        var serializedData = objectsToSave.Select(@object => @object.GetSaveLoadData()).ToList();

        //        if (!Directory.Exists(SaveDataFolder))
        //            Directory.CreateDirectory(SaveDataFolder);

        //        var saveFile = new SaveFile(serializedData);
        //        var serializedSaveFile = JsonConvert.SerializeObject(saveFile);

        //        //todo: make async
        //        File.WriteAllText(SaveFilePath, serializedSaveFile);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //        throw;
        //    }

        //}
    }
    public void AddDish(List<FoodInfo> foods)
    {
        //Foods.Clear();
        //if (foods.Count > 0)
        //{
        //    content.SetActive(true);
        //    Foods.AddRange(foods);
        //}
        //else
        //{
        //    content.SetActive(false);
        //}

    }
    public bool Compare(out string result)
    {
        bool isGood = true;
        result = "убрать эту строку";
        //StringBuilder sb = new StringBuilder();
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
        //        //else if (Mathf.Abs(foods[i].TemperatureSum - etalon[i].TemperatureSum) > 100)
        //        //{
        //        //    sb.Append($"Переварен: {etalon[i].FoodName}!\n");
        //        //    isGood = false;
        //        //}
        //        //else if (Mathf.Abs(foods[i].TemperatureWithoutWaterSum - etalon[i].TemperatureWithoutWaterSum) > 100)
        //        //{
        //        //    sb.Append($"Пережарен: {etalon[i].FoodName}!\n");
        //        //    isGood = false;
        //        //}
        //        //else if (foods[i].CurrentCutType != etalon[i].CurrentCutType)
        //        //{
        //        //    sb.Append($"Нужно было нарезать: {cutTypeEtalon}, а не {cutTypeCurrent}!\n");
        //        //    isGood = false;
        //        //}
        //    }
        //}
        //else
        //{
        //    sb.Append("Нету ингридиентов!\n");
        //    isGood = false;
        //}
        //result = sb.ToString();
        return isGood;
    }
}
