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
                timerText.text = Translator.GetInstance().GetTimeBySeconds(totalSeconds);
            }
            totalSeconds++;
            yield return new WaitForSeconds(second);
        }
    }
}
