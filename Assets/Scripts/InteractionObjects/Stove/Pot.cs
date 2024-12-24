using System.Collections;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(ShowObjectInfo))]
public class Pot : MonoBehaviour, IHeated
{
    Water potWater = null;
    ShowObjectInfo info;
    ParticleSystem smoke;
    public HeatedInfo HeatedInfo { get; set; }

    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Кастрюля";
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
        HeatedInfo = new HeatedInfo(temperature: 90, currentMassKG: 5, maxMassKG: 5, hasWater: false);

        potWater = GetComponentInChildren<Water>();
        potWater.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(info != null)
        {
            info.ObjectData = $"\nt: {HeatedInfo.Temperature.ToString("F1")}\nm: {HeatedInfo.CurrentMassKG.ToString("F1")}";
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
    public void AddWater()
    {
        HeatedInfo.HasWater = true;
        HeatedInfo.CurrentMassKG = HeatedInfo.MaxMassKG;
        potWater.gameObject.SetActive(true);
        potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MaxMassKG);
    }
    public void OnBoiling(byte level)
    {
        if(HeatedInfo.HasWater && potWater.isActiveAndEnabled)
        {
            if(HeatedInfo.CurrentMassKG<1f)
            {
                HeatedInfo.HasWater = false;
                potWater.gameObject.SetActive(false);

                return;
            }
            switch (level)
            {
                case 1:
                    HeatedInfo.CurrentMassKG -= 0.1f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MaxMassKG);
                    break;
                case 2:
                    HeatedInfo.CurrentMassKG -= 0.2f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MaxMassKG);
                    break;
                case 3:
                    HeatedInfo.CurrentMassKG -= 0.3f / 60f;
                    potWater.ChangeWaterLevel(HeatedInfo.CurrentMassKG, HeatedInfo.MaxMassKG);
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
        while (HeatedInfo.Temperature > 20f)
        {
            HeatedInfo.Temperature += -0.1f * (HeatedInfo.Temperature - 20f);
            yield return new WaitForSeconds(1f / 10f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
