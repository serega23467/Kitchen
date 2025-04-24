using System.Collections;
using System.Linq;
using UnityEngine;

public class StoveFire : MonoBehaviour
{
    public void StartFire(IHeated heated, byte level)
    {
        heated.StartHeating();
        StartCoroutine(Fire(heated, level));
    }
    IEnumerator Fire(IHeated heated, byte level)
    {
        float maxT = 250;
        if(heated.HeatedInfo.HasWater)
        {
            maxT = 100;
        }
        while (true)
        {
            float delta = 0;
            if (!heated.HeatedInfo.HasWater)
            {
                maxT = 250;
            }
            if (heated.HeatedInfo.Temperature < maxT)
            {
                float heatAdded = level * 1000;
                float mass = heated.HeatedInfo.CurrentMassKG;
                float deltaTemperature = (heatAdded / (mass * 4180f));
                heated.HeatedInfo.Temperature += deltaTemperature;
                delta = deltaTemperature;
            }
            else if (heated.HeatedInfo.HasWater && heated.HeatedInfo.Temperature >= maxT)
            {
                heated.OnBoiling(level);
                float heatAdded = level * 1000;
                float mass = heated.HeatedInfo.CurrentMassKG;
                float deltaTemperature = (heatAdded / (mass * 4180f));
                delta = deltaTemperature;
            }
            heated.HeatedInfo.HeatingTime++;
            if (heated is Pot)
            {
                Pot pot = heated as Pot;
                pot.HeatWater(delta);
            }
            else if (heated is FryingPan)
            {
                FryingPan pan = heated as FryingPan;
                if (pan.Foods.Where(f=>f.FoodName=="Butter").Any())
                {
                    delta /= 10;
                }
                pan.HeatFood(delta);
            }
            yield return new WaitForSeconds(1f/10f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
