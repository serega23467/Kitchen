using UnityEngine;
using UnityEngine.Events;

public class Potato : MonoBehaviour, IFood
{
    public string FoodName { get; set; } = "��������";
    public float TemperatureSum { get; set; } = 0;
    public int GramsWeight { get; set; } = 12;
    public UnityEvent<GameObject> OnPull { get; set; }
    public bool IsPour { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CutType[] AllCutTypes { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public CutType CurrentCutType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public GameObject FoodGameObject { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float TemperatureWithoutWaterSum { get; set; }
    public GameObject GetPlatePrefab()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        OnPull = new UnityEvent<GameObject>();
    }
}
