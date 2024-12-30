using UnityEngine;
using UnityEngine.Events;

public class Salt : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "—оль";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 10;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get; set; } = true;
    public CutType[] AllCutTypes { get; set; }
    public CutType CurrentCutType { get; set; }
    public GameObject FoodGameObject { get; set; } = null;
    public float TemperatureWithoutWaterSum { get; set; }
    public IFood CloneFood()
    {
        return new Salt()
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
        FoodGameObject = gameObject;
        OnPull = new UnityEvent<GameObject>();
        AllCutTypes = new CutType[] { CutType.None };
        CurrentCutType = CutType.None;
        GetComponent<ShowObjectInfo>().ObjectData = $"{GramsWeight} г";
    }
}
