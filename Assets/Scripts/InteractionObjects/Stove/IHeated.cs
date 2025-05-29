using UnityEngine;

public interface IHeated
{
    public HeatedInfo HeatedInfo { get; set; }
    public void AddWater();
    public void OnBoiling(byte level);
    public void StopHeating();
    public void StartHeating();
    string GetInfo(bool hasWater = true)
    {
        string result = string.Empty;
        int totalSeconds = HeatedInfo.HeatingTime;
        if (totalSeconds > 0)
        {
           
            string time = Translator.GetTimeBySeconds(totalSeconds);
            if(hasWater) 
                result= $"\n�����������: {HeatedInfo.Temperature.ToString("F1")} C\n����� ����: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg\n�����: {time}";
            else
                result = $"\n�����������: {HeatedInfo.Temperature.ToString("F1")} C\n�����: {time}";
        }
        else
        {
            if(hasWater)
                result = $"\n�����������: {HeatedInfo.Temperature.ToString("F1")} C\n����� ����: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg";
            else
                result = $"\n�����������: {HeatedInfo.Temperature.ToString("F1")} C";
        }
        return result;
    }
}
