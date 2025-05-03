using UnityEngine;

public interface IHeated
{
    public HeatedInfo HeatedInfo { get; set; }
    public void AddWater();
    public void OnBoiling(byte level);
    public void StopHeating();
    public void StartHeating();
    string GetInfo()
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
            result= $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg\nвремя: {time}";
        }
        else
        {
            result = $"\nтемпература: {HeatedInfo.Temperature.ToString("F1")} C\nмасса воды: {(HeatedInfo.CurrentMassKG - HeatedInfo.MinMassKG).ToString("F1")} kg";
        }
        return result;
    }
}
