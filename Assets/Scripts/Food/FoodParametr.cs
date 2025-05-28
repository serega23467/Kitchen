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
    public string TooMuchError = "";
    public string TooLittleError = "";
    public string NoThisError = "";
    public string NoNeedThisError = "";

    public bool IsSpice = false;
    public FoodParametr Clone()
    {
        return this.MemberwiseClone() as FoodParametr;
    }
    public SerializableFoodParameter GetSerializbleFoodParameter()
    {
        SerializableFoodParameter sfoodParam = new SerializableFoodParameter();
        sfoodParam.ParamName = ParamName;
        sfoodParam.ParamValue = ParamValue;
        sfoodParam.IsSpice = IsSpice;
        return sfoodParam;
    }
}
