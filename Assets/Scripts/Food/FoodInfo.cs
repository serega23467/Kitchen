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
    [HideInInspector]
    public List<FoodParametr> Params = new List<FoodParametr>();
    [HideInInspector]
    public CutType CurrentCutType;
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
    public SerializableFoodInfo GetSerializableFoodInfo()
    {
        SerializableFoodInfo sfoodInfo = new SerializableFoodInfo();
        sfoodInfo.FoodId = FoodId;
        sfoodInfo.FoodName = FoodName;
        sfoodInfo.GramsWeight= GramsWeight;
        sfoodInfo.IsPour = IsPour;
        sfoodInfo.AllCutTypes = AllCutTypes;
        List<SerializableFoodParameter> sparams = new List<SerializableFoodParameter>(); 
        foreach(var p in Params)
        {
            sparams.Add(p.GetSerializbleFoodParameter());
        }
        sfoodInfo.Params = sparams;
        sfoodInfo.CurrentCutType = CurrentCutType;
        return sfoodInfo;
    }
}