using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodParametr", menuName = "Scriptable Objects/FoodParametr")]
[System.Serializable]
public class FoodParametr : ScriptableObject
{
    public string ParamName;
    [HideInInspector]
    public float ParamValue;
    public string Desc = "";
    public string ValueChar = "";
    public bool IsSpice = false;
    public FoodParametr Clone()
    {
        return this.MemberwiseClone() as FoodParametr;
    }
}
