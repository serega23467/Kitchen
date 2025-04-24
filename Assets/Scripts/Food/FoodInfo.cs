using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodInfo", menuName = "Scriptable Objects/FoodInfo")]
[System.Serializable]
public class FoodInfo : ScriptableObject, ICloneable
{
    public string FoodId;
    public string FoodName;
    public float GramsWeight;
    public bool IsPour;
    public CutType[] AllCutTypes;
    public List<FoodParametr> Params { get; set; }
    public CutType CurrentCutType { get; set; }
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}