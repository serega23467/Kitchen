using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PanelInfo : MonoBehaviour
{
    [SerializeField]
    TMP_Text headerText;
    [SerializeField]
    TMP_Text infoText;
    [SerializeField]
    TMP_Text dataText; 

    RectTransform panelRect;
    Vector3 size = Vector3.zero;
    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
        size = panelRect.localScale;
        Hide();
    }
    public void ShowInfo(string header, string info, string data)
    {
        headerText.text = header;

        infoText.text = info;
        infoText.fontSize = GetFontSize(info.Length);
        Debug.Log(info.Length);

        dataText.text = data;
        dataText.fontSize = GetFontSize(data.Length);
        Debug.Log(data.Length);

        panelRect.localScale = size;
    }
    public void Hide()
    {
        panelRect.localScale = Vector3.zero;
    }
    int GetFontSize(int charCount)
    {
        int minChars = 1;
        int maxChars = 1000;
        int minCharsForBigValues = maxChars;
        int maxCharsForBigValues = 2000;

        int minFont = 20;
        int minFontForBigValues = 10;
        int maxFont = 40;
        int maxFontForBigValues = 30;

        if (charCount < minChars)
        {
            return 0;
        }
        int fontSize = 0;
        if (charCount > maxChars)
        {
            float t = (float)(charCount - minCharsForBigValues) / (maxCharsForBigValues - minCharsForBigValues);
            fontSize = (int)(maxFontForBigValues - t * (maxFontForBigValues - minFontForBigValues));
        }
        else
        {
            float t = (float)(charCount - minChars) / (maxChars - minChars);
            fontSize = (int)(maxFont - t * (maxFont - minFont));
        }

        return fontSize;
    }
 
}
