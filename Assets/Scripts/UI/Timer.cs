using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int TotalSeconds { get { return totalSeconds; } }
    [SerializeField]
    TMP_Text timerText;
    int totalSeconds;
    bool isStarted = false;

    public void StartTimer()
    {
        if(!isStarted)
            StartCoroutine(TimerTick(SettingsInit.VirtualSecond));
    }
    IEnumerator TimerTick(float second)
    {
        isStarted = true;
        totalSeconds = 0;
        timerText.text = $"00:00:00";
        while (true)
        {
            if (totalSeconds > 0)
            {
                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600)/ 60;
                int seconds = totalSeconds%60;

                string hoursString = hours.ToString();
                hoursString = hoursString.Length < 2 ? "0" + hoursString : hoursString;

                string minutesString = minutes.ToString();
                minutesString = minutesString.Length < 2 ? "0" + minutesString : minutesString;

                string secondsString = seconds.ToString();
                secondsString = secondsString.Length < 2 ? "0" + secondsString : secondsString;

                timerText.text = $"{hoursString}:{minutesString}:{secondsString}";
            }
            totalSeconds++;
            yield return new WaitForSeconds(second);
        }
    }
}
