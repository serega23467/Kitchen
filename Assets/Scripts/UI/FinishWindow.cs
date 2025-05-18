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
    RectTransform rect;
    Vector3 size = Vector3.zero;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        size = rect.localScale;
        Hide();
    }
    public void Show(LevelInfo info, int playerRate, int totalSeconds, string issues)
    {
        rect.localScale = size;
        if (playerRate > 0)
            resultText.text = "спнбемэ опнидем";
        else
            resultText.text = "спнбемэ ме опнидем";
        for (int i = 0; i < playerRate; i++)
        {
            if (i > stars.Length - 1) return;
            stars[i].color = Color.yellow;
        }
        etalonTime.text = Translator.GetInstance().GetTimeBySeconds(info.CookTime);
        totalTime.text = Translator.GetInstance().GetTimeBySeconds(totalSeconds);
        issuesText.text = issues;
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
    }

}
