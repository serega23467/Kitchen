using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class FryingPan : MonoBehaviour, IHeated
{
    [SerializeField]
    AudioSource fryingPanSource;

    ShowObjectInfo info;
    Plate plate;
    string foodDataBoofer = "";
    public HeatedInfo HeatedInfo { get; set; }

    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        HeatedInfo = new HeatedInfo(temperature: 20, minMassKG: 0, currentMassKG: 0, maxMassKG: 5, hasWater: false, time: 0);

        plate = GetComponent<Plate>();

        plate.OnUpdateInfo.AddListener(UpdateFoodsInfo);
        UpdateFoodsInfo($"Макс вес - {plate.MaxWeight} г");
    }
    public List<FoodComponent> GetFoods()
    {
        return plate.Foods.ToList();
    }
    public void Heat(float t, StoveFireType type)
    {
        if(fryingPanSource !=null && !fryingPanSource.isPlaying) 
            fryingPanSource.Play();
        float temp = t;
        if(plate.Foods.Where(f => f.FoodName == "Butter").Any())
        {
            temp /= 10;
        }
        foreach (FoodComponent food in plate.Foods.ToList())
        {
            switch(type)
            {
                case StoveFireType.Oven:
                    food.TryAddParameterValue("BakeProgress", temp);
                    break;
                case StoveFireType.Burner:
                    food.TryAddParameterValue("FryProgress", temp);
                    break;
            }
        }
    }
    void UpdateFoodsInfo(string data)
    {
        foodDataBoofer = data;
    }
    private void Update()
    {
        if (info != null)
        {
            //костыль выполнения метода интерфейса по умолчанию, т.к. нельзя наследовать несколько классов чтобы использовать абстрактный
            var iheated = this as IHeated;
            info.ObjectData = iheated.GetInfo(false) + "\n" + foodDataBoofer;
        }
    }
    public void AddWater()
    {
        throw new System.NotImplementedException();
    }
    public void StartHeating()
    {
        StopAllCoroutines();
    }
    public void StopHeating()
    {
        StartCoroutine(Cooling());
        if (fryingPanSource != null  && fryingPanSource.isPlaying)
            fryingPanSource.Stop();
    }
    IEnumerator Cooling()
    {
        int time = 0;
        float startT = HeatedInfo.Temperature;
        while (HeatedInfo.Temperature > 20f)
        {
            if (time > 2)
            {
                if (HeatedInfo.HeatingTime > 0)
                {
                    HeatedInfo.HeatingTime = 0;
                }
                HeatedInfo.Temperature = startT * Mathf.Exp(-0.0008f * (1 / Mathf.Clamp(plate.Foods.Sum(f=>f.FoodInfo.GramsWeight)/5, 0.08f, float.MaxValue)) * time);
            }
            time++;
            yield return new WaitForSeconds(SettingsInit.VirtualSecond);
        }
        if (HeatedInfo.Temperature < 20f) HeatedInfo.Temperature = 20f;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void OnBoiling(byte level)
    {
        throw new System.NotImplementedException();
    }
    public void PutFood(FoodComponent food)
    {
        plate.PutFood(food);
    }
}
