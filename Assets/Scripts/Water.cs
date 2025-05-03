using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
public class Water : MonoBehaviour, IListable
{
    public bool IsBoiled { get; private set; } = false;
    public bool IsBoilingNow { get; private set; } = false;
    [SerializeField]
    public List<FoodComponent> Foods { get; set; }
    float yMaxLevel;
    float yMinLevel;

    private void Awake()
    {
        yMaxLevel = transform.localPosition.y;
        yMinLevel = -1.05f;
        Foods = new List<FoodComponent>();
    }
    public void HeatFood(float t)
    {
        foreach(FoodComponent food in Foods)
        {
            food.TryAddParameterValue("BoilProgress", t);
        }
    }
    public void SpiceFood(SpiceComponent spice)
    {
        foreach (FoodComponent food in Foods)
        {
            spice.AddSpiceTo(food);
        }
    }
    public void AddFood(FoodComponent food)
    {
        food.gameObject.transform.parent = transform;
        food.gameObject.transform.localScale = Vector3.zero;
        Foods.Add(food);
        //if (!food.FoodInfo.IsPour)
            //food.OnPull.AddListener(PutFood);
    }
    public void AddFood(List<FoodComponent> foods)
    {
        for (int i = 0; i < foods.Count; i++)
        {
            foods[i].gameObject.transform.parent = transform;
            foods[i].gameObject.transform.localScale = Vector3.zero;
            Foods.Add(foods[i]);
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
    public void PutFood(FoodComponent food)
    {
        if(food != null && food.plate!=null)
        {
            Foods.Remove(food);
            UIElements.GetInstance().UpdateObjectContent(Foods);
        }
    }
}
