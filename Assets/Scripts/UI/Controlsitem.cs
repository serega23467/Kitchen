using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controlsitem : RecyclingListViewItem
{
    [SerializeField]
    TMP_Text conrolAction;
    [SerializeField]
    TMP_Text controlKey;
    [SerializeField]
    Button button;

    ControlsInfo controlsInfo;
    public ControlsInfo ControlsInfo
    {
        get { return controlsInfo; }
        set
        {
            if (value == null) return;
            controlsInfo = value;
            conrolAction.text = controlsInfo.Action;
            controlKey.text = controlsInfo.Key;
            button.onClick.AddListener(delegate { controlsInfo.OnChangeKey.Invoke(controlsInfo.Id); });
        }
    }
}
