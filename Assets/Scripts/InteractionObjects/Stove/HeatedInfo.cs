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
    public int HeatingTime { get; set; }
    public readonly float MinMassKG;
    public readonly float MaxMassKG;
    public bool HasWater { get; set; }

    public HeatedInfo(float temperature, float minMassKG, float currentMassKG, float maxMassKG, bool hasWater, int time)
    {
        Temperature = temperature;
        MinMassKG = minMassKG;
        CurrentMassKG = currentMassKG;
        MaxMassKG = maxMassKG;
        HasWater = hasWater;
        HeatingTime = time;
    }
}
