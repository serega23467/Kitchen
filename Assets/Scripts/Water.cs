using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;
public class Water : MonoBehaviour, IListable
{
    public bool IsBoiled { get; private set; } = false;
    public bool IsBoilingNow { get; private set; } = false;
    UnityEvent<IFood> putFoodFromWater;
    [SerializeField]
    public List<IFood> Foods { get; set; }
    float yMaxLevel;
    float yMinLevel;

    private void Awake()
    {
        yMaxLevel = transform.localPosition.y;
        yMinLevel = -1.05f;
        Foods = new List<IFood>();
        putFoodFromWater = new UnityEvent<IFood>();
    }
    private void Start()
    {
        putFoodFromWater.AddListener(Parents.GetInstance().Player.GetComponent<PlayerRaycast>().PickFood);
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
            if(!food.IsPour)
                food.OnPull.AddListener(PutFood);
        }
    }
    public void ChangeWaterLevel(float mass, float minMass, float fullMass)
    {
        var pos = transform.localPosition;
        float lerpT = Mathf.Clamp01((mass - fullMass-1) / (minMass - fullMass-1));
        float y = Mathf.Lerp(yMaxLevel, yMinLevel, lerpT);
        if (transform.localPosition != new Vector3(pos.x, y, pos.z))
            transform.localPosition = new Vector3(pos.x, y, pos.z);
    }
    void PutFood(GameObject food)
    {
        if(food != null)
        {
            if(Parents.GetInstance().Player.GetComponent<PlayerRaycast>().CurrentDraggableObject!= null)
            {
                if(Parents.GetInstance().Player.GetComponent<PlayerRaycast>().CurrentDraggableObject.GetComponent<Plate>() != null)
                {
                    Foods.Remove(food.GetComponent<IFood>());
                    putFoodFromWater.Invoke(food.GetComponent<IFood>());
                }
            }
        }
    }
}
