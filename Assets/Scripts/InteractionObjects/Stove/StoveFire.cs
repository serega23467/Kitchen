using System.Collections;
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
            }
            else if (heated.HeatedInfo.HasWater && heated.HeatedInfo.Temperature >= maxT)
            {
                heated.OnBoiling(level);
            }
            yield return new WaitForSeconds(1f/10f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
