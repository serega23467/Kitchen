using UnityEngine;

[System.Serializable]
public class SerializableFoodParameter
{
    public string ParamName;
    public float ParamValue;
    public bool IsSpice = false;
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        SerializableFoodParameter value2 = obj as SerializableFoodParameter;
        if (value2 == null)
        {
            return false;
        }
        if (ParamName != value2.ParamName)
        {
            return false;
        }
        if (ParamValue != value2.ParamValue)
        {
            return false;
        }
        if (IsSpice != value2.IsSpice)
        {
            return false;
        }
        return true;
    }
    public SerializableFoodParameter Clone()
    {
        return this.MemberwiseClone() as SerializableFoodParameter;
    }
}
