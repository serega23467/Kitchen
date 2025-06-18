using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ConfirmWindow : MonoBehaviour, IHideble
{
    [SerializeField]
    Button buttonYes;
    [SerializeField]
    Button buttonNo;
    [SerializeField]
    TMP_Text askText;

    public bool IsActive => gameObject.activeSelf;
    private void Start()
    {
        Hide();
    }
    public void Show(string text, Action<bool> OnConfirm)
    {
        gameObject.SetActive(true);
        AddListener(OnConfirm, Hide, text);
    }
    public void Hide()
    {
        buttonYes.onClick.RemoveAllListeners();
        buttonNo.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
        
    }
    public void AddListener(Action<bool> OnConfirm, Action OnClose, string text)
    {
        askText.text = text;
        buttonYes.onClick.AddListener(delegate () { OnConfirm.Invoke(true); OnClose.Invoke(); buttonYes.onClick.RemoveAllListeners(); });
        buttonNo.onClick.AddListener(delegate () { OnConfirm.Invoke(false); OnClose.Invoke(); buttonNo.onClick.RemoveAllListeners(); });
    }
}
