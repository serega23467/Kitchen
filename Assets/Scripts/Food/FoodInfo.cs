using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodInfo", menuName = "Scriptable Objects/FoodInfo")]
[System.Serializable]
public class FoodInfo : ScriptableObject
{
    public string FoodId;
    public string FoodName;
    public float GramsWeight;
    public bool IsPour;
    public CutType[] AllCutTypes;
    public List<FoodParametr> Params { get; set; } = new List<FoodParametr>();
    public CutType CurrentCutType { get; set; }
    public FoodInfo Clone()
    {
        FoodInfo clone = this.MemberwiseClone() as FoodInfo;
        List<FoodParametr> cloneParams = new List<FoodParametr>();
        foreach (FoodParametr param in Params)
        {
            cloneParams.Add(param.Clone());
        }
        clone.Params = cloneParams;
        return clone;
    }
}