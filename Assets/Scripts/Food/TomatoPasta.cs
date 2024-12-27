using UnityEngine;
using UnityEngine.Events;

public class TomatoPasta : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "Томатная паста";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 30;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get; set; } = true;
    public CutType[] AllCutTypes { get; set; }
    public CutType CurrentCutType { get; set; }
    public GameObject FoodGameObject { get; set; } = null;
    public float TemperatureWithoutWaterSum { get; set; }
    public GameObject GetPlatePrefab()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        OnPull = new UnityEvent<GameObject>();
        AllCutTypes = new CutType[] { CutType.None };
        CurrentCutType = CutType.None;
        GetComponent<ShowObjectInfo>().ObjectData = $"{GramsWeight} г";
    }
}
