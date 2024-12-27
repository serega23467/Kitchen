using UnityEngine;
using UnityEngine.Events;

public class Garlic : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "Чеснок";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 40;
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
        AllCutTypes = new CutType[] { CutType.None, CutType.Finely };
        CurrentCutType = CutType.None;
        GetComponent<ShowObjectInfo>().ObjectData = $"{GramsWeight} г";
    }
}
