using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIElements
{
    GameObject canvas;
    UnityEngine.UI.Image panelInfo;
    UnityEngine.UI.Image panelResult;
    UnityEngine.UI.Image panelRecipe;
    TMP_Text panelInfoTextName;
    TMP_Text panelInfoTextDescription;
    TMP_Text panelInfoTextData;
    TMP_Text panelResultText;
    TMP_Text panelResultHeaderText;

    Vector3 panelInfoSize;
    Vector3 panelResultSize;
    Vector3 scrollViewSize;
    Vector3 panelRecipeSize;

    RectTransform scrollView;
    ScrollPanel scrollPanel;

    private static UIElements instance;
    private UIElements() { }
    public static UIElements GetInstance()
    {
        if (instance == null)
        {
            instance = new UIElements();
            instance.canvas = GameObject.Find("Canvas");
            instance.scrollPanel = instance.canvas.GetComponent<ScrollPanel>();

            instance.scrollView = GameObject.Find("Scroll View").GetComponent<RectTransform>();
            instance.scrollViewSize = instance.scrollView.localScale;

            instance.panelRecipe = GameObject.Find("PanelRecipe").GetComponent<UnityEngine.UI.Image>();
            instance.panelRecipeSize = instance.panelRecipe.rectTransform.localScale;

            instance.panelInfo = GameObject.Find("PanelInfo").GetComponent<UnityEngine.UI.Image>();
            instance.panelInfoTextName = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[0];
            instance.panelInfoTextDescription = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[1];
            instance.panelInfoTextData = instance.panelInfo.GetComponentsInChildren<TMP_Text>()[2];
            instance.panelInfoSize = instance.panelInfo.rectTransform.localScale;

            instance.panelResult = GameObject.Find("PanelResult").GetComponent<UnityEngine.UI.Image>();
            instance.panelResultHeaderText = instance.panelResult.GetComponentsInChildren<TMP_Text>()[0];
            instance.panelResultText = instance.panelResult.GetComponentsInChildren<TMP_Text>()[1];
            instance.panelResultSize = instance.panelResult.rectTransform.localScale;

        }
        return instance;
    }
    public void ShowPanelRecipe()
    {
        instance.panelRecipe.rectTransform.localScale = instance.panelRecipeSize;
    }
    public void HidePanelRecipe()
    {
        instance.panelRecipe.rectTransform.localScale = new Vector3(0, 0, 0);
    }
    public void ShowPanelResult(string result, string text)
    {
        instance.panelResultHeaderText.text = result;
        instance.panelResultText.text = text;
        instance.panelResult.rectTransform.localScale = instance.panelResultSize;
    }
    public void HidePanelResult()
    {
        instance.panelResult.rectTransform.localScale = new Vector3 (0, 0, 0);
    }
    public void ShowObjectContent(IListable list)
    {
        scrollPanel.RetrieveData(list.Foods);
        scrollView.localScale = scrollViewSize;
    }
    public void UpdateObjectContent(IListable list)
    {
        scrollPanel.RetrieveData(list.Foods);
    }
    public void HideObjectContent()
    {
        scrollView.localScale = Vector3.zero;
    }
    public void ShowObjectInfo(string nameText, string descText, string dataText)
    {
        panelInfoTextName.text = nameText;
        panelInfoTextDescription.text = descText;
        panelInfoTextData.text = dataText;
        panelInfo.rectTransform.localScale = panelInfoSize;
    }
    public void HideObjectInfo()
    {
        panelInfo.rectTransform.localScale = Vector3.zero;
    }
}
