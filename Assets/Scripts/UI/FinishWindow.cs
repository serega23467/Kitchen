using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishWindow : MonoBehaviour
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
    Vector3 size = Vector3.zero;

    private void Start()
    {
        Hide();
    }
    public void Show(LevelInfo info, int playerRate, int totalSeconds, string issues)
    {
        gameObject.SetActive(true);
        if (playerRate > 0)
            resultText.text = "спнбемэ опнидем";
        else
            resultText.text = "спнбемэ ме опнидем";
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
