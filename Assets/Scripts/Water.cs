using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class Water : MonoBehaviour, IListable
{
    public bool IsBoiled { get; private set; } = false;
    public bool IsBoilingNow { get; private set; } = false;
    UnityEvent<FoodComponent> putFoodFromWater;
    [SerializeField]
    public List<FoodComponent> Foods { get; set; }
    float yMaxLevel;
    float yMinLevel;

    private void Awake()
    {
        yMaxLevel = transform.localPosition.y;
        yMinLevel = -1.05f;
        Foods = new List<FoodComponent>();
        putFoodFromWater = new UnityEvent<FoodComponent>();
    }
    private void Start()
    {
        putFoodFromWater.AddListener(Parents.GetInstance().Player.GetComponent<PlayerRaycast>().PickFood);
    }
    public void HeatFood(float t)
    {
        foreach(FoodComponent food in Foods)
        {
            food.AddFoodParameterValue("BoilProgress", t);
        }
    }
    public void AddFood(FoodComponent food)
    {
        //if (Foods.Select(f => f.gameObject).Contains(food.gameObject))
        //{
        //    byte index = (byte)Foods.FindIndex(f => f.gameObject == food.gameObject);
        //    if (index >= 0)
        //    {
        //        Foods[index].FoodInfo.GramsWeight += food.FoodInfo.GramsWeight;
        //    }
        //}
        //else
        //{
        //    Foods.Add(food.CloneFood());
        //    if (!food.IsPour)
        //        food.OnPull.AddListener(PutFood);
        //}
    }
    public void AddFood(List<FoodInfo> foods)
    {
        //for (int i = 0; i < foods.Count; i++)
        //{
        //    if (Foods.Select(f=>f.FoodGameObject).Contains(foods[i].FoodGameObject))
        //    {
        //        byte index = (byte)Foods.FindIndex(f => f.FoodGameObject == foods[i].FoodGameObject);
        //        if (index >= 0)
        //            Foods[index].GramsWeight += foods[i].GramsWeight;
        //    }
        //    else
        //    {
        //        Foods.Add(foods[i].CloneFood());
        //        if (!foods[i].IsPour)
        //            foods[i].OnPull.AddListener(PutFood);
        //    }
        //}
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
                    //var f = Foods.FirstOrDefault(f => f.FoodGameObject == food);
                    //if (f!=null)
                    //{
                    //    Foods.Remove(f);
                    //}
                    //putFoodFromWater.Invoke(f);
                }
            }
        }
    }
}
