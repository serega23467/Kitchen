using System.Collections;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(ShowObjectInfo))]
public class Pot : MonoBehaviour, IHeated
{
    [SerializeField]
    float t = 20;
    public Water PotWater = null;
    byte m;
    ShowObjectInfo info;
    ParticleSystem smoke;
    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Кастрюля";
        smoke = GetComponentInChildren<ParticleSystem>();
        smoke.Pause();
    }
    private void Update()
    {
        info.ObjectInfo = $"t: {Temperature.ToString("F1")}";
        if (PotWater != null && smoke != null)
        {
            if (Temperature >= 100)
            {
                if (!smoke.isPlaying)
                {
                    smoke.Play();
                }               
            }
            else if (smoke.isPlaying)
            {
                smoke.Pause();
            }
        }
    }
    public float Temperature
    {
        get { return t; }
        set
        {
            if (value < 200 && value >= 0)
                t = value;
            else if (value > 200)
                t = 200;
            else if (value < 20)
                t = 20;
        }
    }
    public byte MassKG { get; set; } = 5;
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
        while (Temperature > 20f)
        {
            Temperature += -0.1f * (Temperature - 20f);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
