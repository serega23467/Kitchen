using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(ShowObjectInfo))]
public class Pot : MonoBehaviour, IHeated
{
    [SerializeField]
    Water potWater = null;
    ShowObjectInfo info;
    ParticleSystem smoke;
    public HeatedInfo HeatedInfo { get; set; }

    void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Кастрюля";
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
        HeatedInfo = new HeatedInfo(temperature: 20,minMassKG: 1, currentMassKG: 1, maxMassKG: 5, hasWater: false, time: 0);

        potWater = GetComponentInChildren<Water>();
        potWater.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(info != null)
        {
            int totalSeconds = HeatedInfo.HeatingTime;
            if(totalSeconds > 0) 
            { 
                int hours = totalSeconds / 3600;
                if (hours > 0)
                {
                    totalSeconds %= 60;
                }
                int minutes = totalSeconds / 60;
                if (minutes > 0)
                {
                    totalSeconds %= 60;
                }
                int seconds = totalSeconds;
                string time = $"{hours}:{minutes}:{seconds}";
                info.ObjectData = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg\nвремя: {time}";
            }
            else
            {
                info.ObjectData = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg";
            }
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
    public List<IFood> GetFoods()
    {
        return potWater.Foods;
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
    public void PutToWater(IFood food)
    {
        potWater.AddFood(food);
    }
    public void PutToWater(List<IFood> food)
    {
        potWater.AddFood(food);
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
            yield return new WaitForSeconds(1f / 10f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
