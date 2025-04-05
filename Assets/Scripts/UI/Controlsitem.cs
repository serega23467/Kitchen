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

    SettingValue controlsInfo;
    public SettingValue ControlsInfo
    {
        get { return controlsInfo; }
        set
        {
            if (value == null) return;
            controlsInfo = value;
            conrolAction.text = controlsInfo.Name;
            controlKey.text = controlsInfo.Value;
            button.onClick.AddListener(delegate { controlsInfo.OnValueChange.Invoke(controlsInfo.Id); });
        }
    }
}
