using System.Collections;
using UnityEngine;

public class StoveFire : MonoBehaviour
{
    IHeated heatedOb = null;
    public void StartFire(IHeated heated, byte level)
    {
        heatedOb = heated;
        heatedOb.StartHeating();
        StartCoroutine(Fire(heated, level));
    }
    IEnumerator Fire(IHeated heated, byte level)
    {
        while (true)
        {
            if(heated.Temperature < 100)
            {
                float heatAdded = level*1000;
                float mass = heated.MassKG;
                float deltaTemperature = (heatAdded / (mass * 4180f));
                heated.Temperature += deltaTemperature;
                yield return new WaitForSeconds(1f);
            }
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
