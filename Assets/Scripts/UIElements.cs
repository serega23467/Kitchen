using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElements
{
    Image panelInfo;
    TMP_Text panelInfoTextName;
    TMP_Text panelInfoTextDescription;
    Vector3 panelInfoSize;

    private static UIElements instance;
    private UIElements() { }
    public static UIElements GetInstance()
    {
        if (instance == null)
        {
            instance = new UIElements();
            instance.panelInfo = GameObject.Find("PanelInfo").GetComponent<Image>();
            instance.panelInfoTextName = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[0];
            instance.panelInfoTextDescription = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[1];
            instance.panelInfoSize = instance.panelInfo.rectTransform.localScale;
        }
        return instance;
    }
    public void ShowObjectInfo(string nameText, string descText)
    {
        panelInfoTextName.text = nameText;
        panelInfoTextDescription.text = descText;
        panelInfo.rectTransform.localScale = panelInfoSize;
    }
    public void HideObjectInfo()
    {
        panelInfo.rectTransform.localScale = Vector3.zero;
    }
}
