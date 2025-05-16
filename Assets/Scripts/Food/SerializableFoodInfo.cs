using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class SerializableFoodInfo
{
    public string FoodId;
    public string FoodName;
    public float GramsWeight;
    public bool IsPour;
    public CutType[] AllCutTypes;
    [HideInInspector]
    public List<SerializableFoodParameter> Params = new List<SerializableFoodParameter>();
    [HideInInspector]
    public CutType CurrentCutType;

    public int Count = 1;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        SerializableFoodInfo value2 = obj as SerializableFoodInfo;
        if (value2 == null)
        {
            return false;
        }
        if (Params.Count != value2.Params.Count)
        {
            return false;
        }
        for (int i = 0; i < Params.Count; i++)
        {
            if (!Params[i].Equals(value2.Params[i]))
                return false;
        }
        if (FoodId != value2.FoodId)
        {
            return false;
        }
        if (FoodName != value2.FoodName)
        {
            return false;
        }
        if (GramsWeight!=value2.GramsWeight)
        {
            return false;
        }
        if (IsPour != value2.IsPour)
        {
            return false;
        }
        if (CurrentCutType != value2.CurrentCutType)
        {
            return false;
        }
        return true;
    }
    public SerializableFoodInfo Clone()
    {
        SerializableFoodInfo clone = this.MemberwiseClone() as SerializableFoodInfo;
        List<SerializableFoodParameter> cloneParams = new List<SerializableFoodParameter>();
        foreach (var param in Params)
        {
            cloneParams.Add(param.Clone());
        }
        clone.Params = cloneParams;
        return clone;
    }
}
