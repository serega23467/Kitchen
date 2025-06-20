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
public class Pot : MonoBehaviour, IHeated, IListable
{
    [SerializeField]
    Material waterWithFoodMaterial;
    Material waterMaterial;

    [SerializeField]
    AudioSource potSource;

    Water potWater = null;

    Renderer waterRend;
    ShowObjectInfo info;
    ParticleSystem smoke;

    public bool CanPull { get; private set; } = true;
    public ObservableCollection<FoodComponent> Foods { get; set; }

    public HeatedInfo HeatedInfo { get; set; }
    string foodInfoBoofer = "";
    private void Awake()
    {
        Foods = new ObservableCollection<FoodComponent>();
    }
    void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        foodInfoBoofer = info.ObjectInfo;

        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();

        HeatedInfo = new HeatedInfo(temperature: 20,minMassKG: 0, currentMassKG: 0, maxMassKG: 5, hasWater: false, time: 0);

        potWater = GetComponentInChildren<Water>();
        waterRend = potWater.gameObject.GetComponentInChildren<Renderer>();
        waterMaterial = waterRend.material;

        Foods.CollectionChanged += ChangeWaterMaterial;
        potWater.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(info != null)
        {
          //костыль выполнения метода интерфейса по умолчанию, т.к. нельзя наследовать несколько классов чтобы использовать абстрактный
            var iheated = this as IHeated;
            info.ObjectInfo = foodInfoBoofer + "\n" + iheated.GetInfo();

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
    public void SpiceFood(SpiceComponent spice)
    {
        foreach (FoodComponent food in Foods)
        {
            spice.AddSpiceTo(food);
        }
    }
    public void Heat(float t, StoveFireType type=StoveFireType.Burner)
    {
        if(HeatedInfo.HasWater)
        {
            if (potSource != null && !potSource.isPlaying)
                potSource.Play();
            foreach (FoodComponent food in Foods)
            {
                food.TryAddParameterValue("BoilProgress", t);
            }
        }
    }
    public void AddWater()
    {
        if(!HeatedInfo.HasWater)
        {
            HeatedInfo.HasWater = true;
            HeatedInfo.CurrentMassKG = HeatedInfo.MaxMassKG;
            HeatedInfo.Temperature = 20f;
            potWater.gameObject.SetActive(true);
        }
        else
        {
            StopAllCoroutines();
            if (HeatedInfo.Temperature < 20f) HeatedInfo.Temperature = 20f;
            float addMass = HeatedInfo.MaxMassKG - HeatedInfo.CurrentMassKG;
            float t = ((HeatedInfo.CurrentMassKG * HeatedInfo.Temperature) + (addMass * 20f)) / (addMass + HeatedInfo.CurrentMassKG);
            HeatedInfo.CurrentMassKG = HeatedInfo.MaxMassKG;
            HeatedInfo.Temperature = t;
            StartCoroutine(Cooling());
        }
        potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MinMassKG, HeatedInfo.MaxMassKG);
    }
    public void AddFood(FoodComponent food)
    {
        food.gameObject.transform.parent = transform;
        food.gameObject.transform.localScale = Vector3.zero;
        Foods.Add(food);

        food.OnPull.RemoveAllListeners();
        food.OnPull.AddListener(PutFood);
    }
    public void AddFood(List<FoodComponent> foods)
    {
        for (int i = 0; i < foods.Count; i++)
        {
            AddFood(foods[i]);
        }
    }
    public ObservableCollection<FoodComponent> GetFoodsClone()
    {
        ObservableCollection<FoodComponent> foods = new ObservableCollection<FoodComponent>();
        for(int i = 0; i< Foods.Count; i++)
        {
            foods.Add(Foods[i].Clone() as FoodComponent);
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
        if (potSource != null && potSource.isPlaying)
            potSource.Stop();
    }
    public void RemoveFood(FoodComponent food)
    {
        Foods.Remove(food);
        UIElements.GetInstance().UpdateObjectContent(this);
    }
    public void PutFood(FoodComponent food)
    {
        if (Parents.GetInstance().Player.PickFood(food))
        {
            RemoveFood(food);
        }
    }
    void ChangeWaterMaterial(object sender, NotifyCollectionChangedEventArgs e)
    {
        if(potWater!=null && waterRend!=null && waterWithFoodMaterial!=null)
        {
            if (Foods.Count() > 0)
                waterRend.material = waterWithFoodMaterial;
            else
                waterRend.material = waterMaterial;
        }
    }
    IEnumerator Cooling()
    {
        int time = 0;
        float startT = HeatedInfo.Temperature;
        while (HeatedInfo.Temperature > 20f)
        {
            if(time > 2)
            {
                if(HeatedInfo.HeatingTime > 0)
                {
                    HeatedInfo.HeatingTime = 0;
                }

                HeatedInfo.Temperature = startT* Mathf.Exp(-0.0008f*(1/ Mathf.Clamp(HeatedInfo.CurrentMassKG,0.1f, float.MaxValue))*time);
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
}
