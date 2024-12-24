using UnityEngine;

public class HeatedInfo
{
    float t;
    public float Temperature
    {
        get { return t; }
        set
        {
            if (value < 200 && value >= 0)
                t = value;
            else if (value > 200)
                t = 200;
            else if (value < 20)
                t = 20;
        }
    }
    public float CurrentMassKG { get; set; }
    public readonly float MaxMassKG;
    public bool HasWater { get; set; }

    public HeatedInfo(float temperature, float currentMassKG, float maxMassKG, bool hasWater)
    {
        Temperature = temperature;
        CurrentMassKG = currentMassKG;
        MaxMassKG = maxMassKG;
        HasWater = hasWater;
    }
}
