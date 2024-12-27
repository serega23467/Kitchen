using UnityEngine;
using UnityEngine.Events;

public class Onion : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "Ћук";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 100;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get; set; } = false;
    public CutType[] AllCutTypes { get; set; }
    public CutType CurrentCutType { get; set; }
    public GameObject FoodGameObject { get; set; }
    public float TemperatureWithoutWaterSum { get; set; }
    public GameObject GetPlatePrefab()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        FoodGameObject = gameObject;
        OnPull = new UnityEvent<GameObject>();
        AllCutTypes = new CutType[] { CutType.None, CutType.Finely, CutType.Medium, CutType.Large };
        CurrentCutType = CutType.None;
        GetComponent<ShowObjectInfo>().ObjectData = $"{GramsWeight} г";
    }
}
