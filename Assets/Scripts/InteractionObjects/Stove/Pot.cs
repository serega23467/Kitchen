using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;

[RequireComponent(typeof(ShowObjectInfo))]
public class Pot : MonoBehaviour, IHeated
{
    [HideInInspector]
    public UnityEvent<FoodComponent> OnRemoveFromWater;
    [SerializeField]
    Water potWater = null;
    [SerializeField]
    Material waterWithFoodMaterial;
    Material waterMaterial;

    Renderer waterRend;
    ShowObjectInfo info;
    ParticleSystem smoke;


    public HeatedInfo HeatedInfo { get; set; }

    void Start()
    {
        OnRemoveFromWater = new UnityEvent<FoodComponent>();
        info = GetComponent<ShowObjectInfo>();
        //info.ObjectName = "Кастрюля";
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
        HeatedInfo = new HeatedInfo(temperature: 20,minMassKG: 1, currentMassKG: 1, maxMassKG: 5, hasWater: false, time: 0);

        potWater = GetComponentInChildren<Water>();
        waterRend = potWater.gameObject.GetComponentInChildren<Renderer>();
        waterMaterial = waterRend.material;

        potWater.Foods.CollectionChanged += ChangeWaterMaterial;
        potWater.gameObject.SetActive(false);
        //УБРАТЬ
        AddWater();
    }

    private void Update()
    {
        if(info != null)
        {
            //костыль выполнения метода интерфейса по умолчанию, т.к. нельзя наследовать несколько классов чтобы использовать абстрактный
            var iheated = this as IHeated;
            info.ObjectData = iheated.GetInfo();

            if (potWater != null && smoke != null)
            {
                if (HeatedInfo.Temperature >= 100 && HeatedInfo.HasWater)
                {
                    if (!smoke.isPlaying)
                    {
                        smoke.Play();
                    }               
                }
                else if (smoke.isPlaying)
                {
                    smoke.Stop();
                }
            }
        }
    }
    void ChangeWaterMaterial(object sender, NotifyCollectionChangedEventArgs e)
    {
        if(potWater!=null && waterRend!=null && waterWithFoodMaterial!=null)
        {
            if (potWater.Foods.Count() > 0)
                waterRend.material = waterWithFoodMaterial;
            else
                waterRend.material = waterMaterial;
        }
    }
    public void SpiceFood(SpiceComponent spice)
    {
        potWater.SpiceFood(spice);
    }
    public void HeatWater(float t)
    {
        potWater.HeatFood(t);
    }
    public void AddWater()
    {
        HeatedInfo.HasWater = true;
        HeatedInfo.CurrentMassKG = HeatedInfo.MaxMassKG;
        HeatedInfo.Temperature = 20f;
        potWater.gameObject.SetActive(true);
        potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MinMassKG, HeatedInfo.MaxMassKG);
    }

    public void PutToWater(FoodComponent food)
    {

        potWater.AddFood(food);

        OnRemoveFromWater.RemoveAllListeners();
        OnRemoveFromWater.AddListener(potWater.PutFood);
    }
    public void PutToWater(List<FoodComponent> foods)
    {
        potWater.AddFood(foods);

        OnRemoveFromWater.RemoveAllListeners();
        OnRemoveFromWater.AddListener(potWater.PutFood);
    }
    public ObservableCollection<FoodComponent> GetFoodsClone()
    {
        ObservableCollection<FoodComponent> foods = new ObservableCollection<FoodComponent>();
        for(int i = 0; i< potWater.Foods.Count; i++)
        {
            foods.Add(potWater.Foods[i].Clone() as FoodComponent);
        }
        return foods;
    }
    public void OnBoiling(byte level)
    {
        if(HeatedInfo.HasWater && potWater.isActiveAndEnabled)
        {
            if(HeatedInfo.CurrentMassKG<=HeatedInfo.MinMassKG)
            {
                HeatedInfo.CurrentMassKG = HeatedInfo.MinMassKG;
                HeatedInfo.HasWater = false;
                potWater.gameObject.SetActive(false);
                return;
            }
            switch (level)
            {
                case 1:
                    HeatedInfo.CurrentMassKG -= 0.05f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MinMassKG, HeatedInfo.MaxMassKG);
                    break;
                case 2:
                    HeatedInfo.CurrentMassKG -= 0.07f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MinMassKG, HeatedInfo.MaxMassKG);
                    break;
                case 3:
                    HeatedInfo.CurrentMassKG -= 0.1f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MinMassKG, HeatedInfo.MaxMassKG);
                    break;
            }
        }
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
            if(time > 2)
            {
                if(HeatedInfo.HeatingTime > 0)
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
}
