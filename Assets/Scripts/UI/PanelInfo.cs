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
        infoText.fontSize = UIElements.GetFontSize(info.Length);

        dataText.text = data;
        dataText.fontSize = UIElements.GetFontSize(data.Length);

        panelRect.localScale = size;
    }
    public void Hide()
    {
        panelRect.localScale = Vector3.zero;
    }
}
