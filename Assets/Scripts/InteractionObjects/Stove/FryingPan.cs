using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        HeatedInfo = new HeatedInfo(temperature: 20, minMassKG: 1, currentMassKG: 1, maxMassKG: 5, hasWater: false, time: 0);

        plate = GetComponent<Plate>();
        //info.ObjectName = "—ковородка";

        plate.OnUpdateInfo.AddListener(UpdateFoodsInfo);
        UpdateFoodsInfo($"ћакс вес - {plate.MaxWeight} г");
        //plate.GetComponent<ShowObjectInfo>().
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
            //костыль выполнени€ метода интерфейса по умолчанию, т.к. нельз€ наследовать несколько классов чтобы использовать абстрактный
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
        byte time = 0;
        while (HeatedInfo.Temperature > 20f)
        {
            if (time > 2)
            {
                if (HeatedInfo.HeatingTime > 0)
                {
                    HeatedInfo.HeatingTime = 0;
                }
                HeatedInfo.Temperature += -0.1f * (HeatedInfo.Temperature - 20f);
            }
            else
            {
                time++;
            }
            yield return new WaitForSeconds(SettingsInit.VirtualSecond);
        }
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
