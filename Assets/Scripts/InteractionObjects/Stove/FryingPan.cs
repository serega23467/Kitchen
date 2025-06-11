using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class FryingPan : MonoBehaviour, IHeated, IListable
{
    [SerializeField]
    AudioSource fryingPanSource;

    ShowObjectInfo info;
    Plate plate;
    string foodInfoBoofer = "";
    public HeatedInfo HeatedInfo { get; set; }
    public ObservableCollection<FoodComponent> Foods { get => plate.Foods; set=>Foods = value; }

    public bool CanPull { get; private set; } = false;

    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        foodInfoBoofer = info.ObjectInfo;
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
    public void HeatFood(float t, StoveFireType type)
    {
        if(fryingPanSource !=null && !fryingPanSource.isPlaying) 
            fryingPanSource.Play();
        foreach (FoodComponent food in plate?.Foods.ToList())
        {
            switch(type)
            {
                case StoveFireType.Oven:
                    food.TryAddParameterValue("BakeProgress", t);
                    break;
                case StoveFireType.Burner:
                    food.TryAddParameterValue("FryProgress", t);
                    break;
            }
        }
    }
    void UpdateFoodsInfo(string data)
    {
        info.ObjectData = data;
    }
    private void Update()
    {
        if (info != null)
        {
            //костыль выполнени€ метода интерфейса по умолчанию, т.к. нельз€ наследовать несколько классов чтобы использовать абстрактный
            var iheated = this as IHeated;
            info.ObjectInfo = foodInfoBoofer + "\n"+ iheated.GetInfo(false);
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
}
