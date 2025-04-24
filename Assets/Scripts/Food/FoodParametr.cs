using UnityEngine;

[CreateAssetMenu(fileName = "FoodParametr", menuName = "Scriptable Objects/FoodParametr")]
[System.Serializable]
public class FoodParametr : ScriptableObject
{
    public string ParamName;
    public float ParamValue;
    public FoodParametr(string name, float value)
    {
        ParamName = name;
        ParamValue = value;
    }
}
