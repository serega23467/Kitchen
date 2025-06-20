using System.Collections;
using System.Linq;
using UnityEngine;

public class StoveFire : MonoBehaviour
{
    public void StartFire(IHeated heated, byte level, float virtualSecondInSeconds, StoveFireType type = StoveFireType.Burner)
    {
        heated.StartHeating();
        StartCoroutine(Fire(heated, level, virtualSecondInSeconds, type));
    }
    IEnumerator Fire(IHeated heated, byte level, float second, StoveFireType type)
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
                float mass = Mathf.Clamp(heated.HeatedInfo.CurrentMassKG, 1f, float.MaxValue);
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
            heated.Heat(delta, type);
            yield return new WaitForSeconds(second);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
