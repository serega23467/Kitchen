using UnityEngine;
using UnityEngine.Events;

public interface IFood 
{
    public GameObject FoodGameObject { get; set; }
    public string FoodName { get; set; }
    public float TemperatureSum { get; set; }
    public float TemperatureWithoutWaterSum { get; set; }
    public int GramsWeight { get; set; }
    public bool IsPour { get; set; }
    public UnityEvent<GameObject> OnPull { get; set; }
    public CutType[] AllCutTypes { get; set; }
    public CutType CurrentCutType { get; set; }
    public GameObject GetPlatePrefab();
}
