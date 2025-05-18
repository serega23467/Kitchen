using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class FryingPan : MonoBehaviour, IHeated, IListable
{

    ShowObjectInfo info;
    Plate plate;
    string foodDataBoofer = "";
    public HeatedInfo HeatedInfo { get; set; }
    public ObservableCollection<FoodComponent> Foods { get => plate.Foods; set=>Foods = value; }

    public bool CanPull { get; private set; } = false;

    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        HeatedInfo = new HeatedInfo(temperature: 20, minMassKG: 1, currentMassKG: 1, maxMassKG: 5, hasWater: false, time: 0);

        plate = GetComponent<Plate>();
        //info.ObjectName = "Сковородка";

        plate.OnUpdateInfo.AddListener(UpdateFoodsInfo);
        //plate.GetComponent<ShowObjectInfo>().
    }
    public List<FoodComponent> GetFoods()
    {
        return plate.Foods.ToList();
    }
    public void HeatFood(float t)
    {
        foreach (FoodComponent food in plate?.Foods.ToList())
        {
            food.TryAddParameterValue("FryProgress", t);
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
            info.ObjectData = foodDataBoofer + "\n\n" + iheated.GetInfo(false);
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
}
