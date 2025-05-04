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
            int hours = totalSeconds / 3600;
            if (hours > 0)
            {
                totalSeconds %= 60;
            }
            int minutes = totalSeconds / 60;
            if (minutes > 0)
            {
                totalSeconds %= 60;
            }
            int seconds = totalSeconds;

            string hoursString = hours.ToString();
            hoursString = hoursString.Length < 2 ? "0" + hoursString : hoursString;

            string minutesString = minutes.ToString();
            minutesString = minutesString.Length < 2 ? "0" + minutesString : minutesString;

            string secondsString = seconds.ToString();
            secondsString = secondsString.Length < 2 ? "0" + secondsString : secondsString;

            string time = $"{hoursString}:{minutesString}:{secondsString}";
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
