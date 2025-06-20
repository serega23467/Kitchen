﻿using UnityEngine;

public interface IHeated
{
    public HeatedInfo HeatedInfo { get; set; }
    public void AddWater();
    public void OnBoiling(byte level);
    public void StopHeating();
    public void StartHeating();
    public void Heat(float t, StoveFireType type);
    string GetInfo(bool hasWater = true)
    {
        string result = string.Empty;
        int totalSeconds = HeatedInfo.HeatingTime;
        if (totalSeconds > 0)
        {
           
            string time = Translator.GetTimeBySeconds(totalSeconds);
            if(hasWater) 
                result= $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} кг\nвремя: {time}";
            else
                result = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nвремя: {time}";
        }
        else
        {
            if(hasWater)
                result = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} кг";
            else
                result = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C";
        }
        return result;
    }
}
