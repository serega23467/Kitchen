using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class FryingPan : MonoBehaviour, IHeated, IListable
{

    ShowObjectInfo info;
    public List<IFood> Foods { get; set; }
    public HeatedInfo HeatedInfo { get; set; }
    UnityEvent<IFood> putFood;

    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "Сковородка";
        HeatedInfo = new HeatedInfo(temperature: 20, minMassKG: 1, currentMassKG: 1, maxMassKG: 5, hasWater: false, time: 0);
        Foods = new List<IFood>();
        putFood = new UnityEvent<IFood>();
        putFood.AddListener(Parents.GetInstance().Player.GetComponent<PlayerRaycast>().PickFood);
    }
    public void HeatFood(float t)
    {
        foreach (IFood food in Foods)
        {
            food.TemperatureWithoutWaterSum += t;
        }
    }
    public void AddFood(IFood food)
    {
        if (Foods.Contains(food))
        {
            byte index = (byte)Foods.FindIndex(f => f.Equals(food));
            if (index >= 0)
                Foods[index].GramsWeight += food.GramsWeight;
        }
        else
        {
            Foods.Add(food);
            if (!food.IsPour)
                food.OnPull.AddListener(PutFood);
        }
    }
    private void Update()
    {
        if (info != null)
        {
            int totalSeconds = HeatedInfo.HeatingTime;
            if (totalSeconds > 0)
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
                info.ObjectData = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nвремя: {time}";
            }
            else
            {
                info.ObjectData = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C";
            }
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
            yield return new WaitForSeconds(1f / 10f);
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
    void PutFood(GameObject food)
    {
        if (food != null)
        {
            if (Parents.GetInstance().Player.GetComponent<PlayerRaycast>().CurrentDraggableObject != null)
            {
                if (Parents.GetInstance().Player.GetComponent<PlayerRaycast>().CurrentDraggableObject.GetComponent<Plate>() != null)
                {
                    Foods.Remove(food.GetComponent<IFood>());
                    putFood.Invoke(food.GetComponent<IFood>());
                }
            }
        }
    }
}
