using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ConfirmWindow : MonoBehaviour
{
    [SerializeField]
    Button buttonYes;
    [SerializeField]
    Button buttonNo;
    [SerializeField]
    TMP_Text askText;

    RectTransform panelRect;
    Vector3 size = Vector3.zero;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
        size = panelRect.localScale;
    }
    private void Start()
    {
        Hide();
    }
    public void Show(string text, Action<bool> OnConfirm)
    {
        panelRect.localScale = size;
        AddListener(OnConfirm, Hide, text);
    }
    void Hide()
    {
        panelRect.localScale = Vector3.zero;
    }
    public void AddListener(Action<bool> OnConfirm, Action OnClose, string text)
    {
        askText.text = text;
        buttonYes.onClick.AddListener(delegate () { OnConfirm.Invoke(true); OnClose.Invoke(); buttonYes.onClick.RemoveAllListeners(); });
        buttonNo.onClick.AddListener(delegate () { OnConfirm.Invoke(false); OnClose.Invoke(); buttonNo.onClick.RemoveAllListeners(); });
    }
}
