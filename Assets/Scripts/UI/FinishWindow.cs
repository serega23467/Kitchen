using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishWindow : MonoBehaviour, IHideble
{
    [SerializeField]
    TMP_Text resultText;
    [SerializeField]
    TMP_Text totalTime;
    [SerializeField]
    TMP_Text etalonTime;
    [SerializeField] 
    TMP_Text issuesText;
    [SerializeField]
    Image[] stars;
    public bool IsActive => gameObject.activeSelf;
    private void Start()
    {
        Hide();
    }
    public void Show(LevelInfo info, int playerRate, int totalSeconds, string issues)
    {
        gameObject.SetActive(true);
        if (playerRate > 0)
            resultText.text = "УРОВЕНЬ ПРОЙДЕН";
        else
            resultText.text = "УРОВЕНЬ НЕ ПРОЙДЕН";
        for (int i = 0; i < playerRate; i++)
        {
            if (i > stars.Length - 1) return;
            stars[i].color = Color.yellow;
        }
        etalonTime.text = Translator.GetTimeBySeconds(info.CookTime);
        totalTime.text = Translator.GetTimeBySeconds(totalSeconds);
        issuesText.text = issues;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
