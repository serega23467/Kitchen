using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class Water : MonoBehaviour
{
    public bool IsBoiled { get; private set; } = false;
    public bool IsBoilingNow { get; private set; } = false;
    public List<IFood> Foods { get; private set; }
    float yMaxLevel;
    float yMinLevel;
    private void Start()
    {
        yMaxLevel = transform.localPosition.y;
        yMinLevel = -1.05f;
    }
    public void HeatFood(float t)
    {
        foreach(IFood food in Foods)
        {
            food.TemperatureSum += t;
        }
    }
    public void AddFood(IFood food)
    {
        if(Foods.Contains(food))
        {
            byte index = (byte)Foods.FindIndex(f=>f.Equals(food));
            if (index >= 0)
                Foods[index].GramsWeight += food.GramsWeight;
        }
        else
        { 
            Foods.Add(food);
        }
    }
    public void ChangeWaterLevel(float mass, float fullMass)
    {
        float divider = fullMass / 6f;
        if (mass <= divider * 5f)
        {
            var pos = transform.localPosition;
            float y = Mathf.Lerp(yMaxLevel, yMinLevel, (1f / 4f));
            if(transform.localPosition != new Vector3(pos.x, y, pos.z))
                transform.localPosition = new Vector3(pos.x, y, pos.z);
        }
        else if (mass <= divider * 4)
        {
            var pos = transform.localPosition;
            float y = Mathf.Lerp(yMaxLevel, yMinLevel, (1f / 4f));
            if (transform.localPosition != new Vector3(pos.x, y, pos.z))
                transform.localPosition = new Vector3(pos.x, y, pos.z);
        }
        else if (mass <= divider * 3f)
        {
            var pos = transform.localPosition;
            float y = Mathf.Lerp(yMaxLevel, yMinLevel, (2f / 4f));
            if (transform.localPosition != new Vector3(pos.x, y, pos.z))
                transform.localPosition = new Vector3(pos.x, y, pos.z);
        }
        else if (mass <= divider * 2f)
        {
            var pos = transform.localPosition;
            float y = Mathf.Lerp(yMaxLevel, yMinLevel, (3f / 4f));
            if (transform.localPosition != new Vector3(pos.x, y, pos.z))
                transform.localPosition = new Vector3(pos.x, y, pos.z);
        }
        else if (mass <= divider*1.5f)
        {
            var pos = transform.localPosition;
            float y = Mathf.Lerp(yMaxLevel, yMinLevel, 1f);
            if (transform.localPosition != new Vector3(pos.x, y, pos.z))
                transform.localPosition = new Vector3(pos.x, y, pos.z);
        }
    }
}
