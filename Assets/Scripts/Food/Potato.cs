using UnityEngine;
using UnityEngine.Events;

public class Potato : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "Картошка";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 12;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CutType[] AllCutTypes { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CutType CurrentCutType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public GameObject FoodGameObject { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float TemperatureWithoutWaterSum { get; set; }
    public IFood CloneFood()
    {
        return new Potato()
        {
            FoodName = this.FoodName,
            TemperatureSum = this.TemperatureSum,
            GramsWeight = this.GramsWeight,
            AllCutTypes = this.AllCutTypes,
            CurrentCutType = this.CurrentCutType,
            FoodGameObject = this.FoodGameObject,
            OnPull = this.OnPull,
            TemperatureWithoutWaterSum = this.TemperatureWithoutWaterSum,
        };
    }
    public GameObject GetPlatePrefab()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        OnPull = new UnityEvent<GameObject>();
    }
}
