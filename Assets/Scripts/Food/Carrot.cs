using UnityEngine;
using UnityEngine.Events;

public class Carrot : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "Морковь";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 75;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get; set; } = false;
    public CutType[] AllCutTypes { get; set; }
    public CutType CurrentCutType { get; set; }
    public GameObject FoodGameObject { get; set; }
    public float TemperatureWithoutWaterSum { get; set; }
    public IFood CloneFood()
    {
        return new Carrot()
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
        FoodGameObject = this.gameObject;
        OnPull = new UnityEvent<GameObject>();
        AllCutTypes = new CutType[] { CutType.None, CutType.Finely, CutType.Medium, CutType.Large };
        CurrentCutType = CutType.None;
        GetComponent<ShowObjectInfo>().ObjectData = $"{GramsWeight} г";
    }
}
