using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    [SerializeField]
    Button buttonYes;
    [SerializeField]
    Button buttonNo;
    [SerializeField]
    TMP_Text askText;
    public void AddListener(Action<bool> OnConfirm, Action OnClose, string text)
    {
        askText.text = text;
        buttonYes.onClick.AddListener(delegate () { OnConfirm.Invoke(true); OnClose.Invoke(); buttonYes.onClick.RemoveAllListeners(); });
        buttonNo.onClick.AddListener(delegate () { OnConfirm.Invoke(false); OnClose.Invoke(); buttonNo.onClick.RemoveAllListeners(); });
    }
}
