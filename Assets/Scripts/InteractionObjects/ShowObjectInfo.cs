using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ShowObjectInfo : MonoBehaviour
{
    public string ObjectName = "";
    public string ObjectInfo = "";
    public bool CanShowContent = false;
    [HideInInspector]
    public string ObjectData = "";

    string translatedInfo = "";

    Outline outline;
    bool hasFinishOutline = false;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    void Start()
    {
        UpdateTranslate();
        SettingsInit.AddListenerOnUpdateKeys(UpdateTranslate);
    }
    void UpdateTranslate()
    {
        if (ObjectInfo != "" && ObjectInfo.Contains('\''))
        {
            translatedInfo = Translator.ReplaceActionToKey(ObjectInfo);
        }
    }
    public void ShowInfo()
    {
        if (ObjectInfo != "" || ObjectData!="" || ObjectName !="")
        {
            if(translatedInfo!="")
                UIElements.GetInstance().ShowObjectInfo(ObjectName + "\n", translatedInfo, ObjectData);
            else
                UIElements.GetInstance().ShowObjectInfo(ObjectName + "\n", ObjectInfo, ObjectData);
        }
    }
    public void HideInfo()
    {
        UIElements.GetInstance().HideObjectInfo();
    }
    public void ShowContent(IListable list, bool hasPlate = true)
    {
        UIElements.GetInstance().ShowObjectContent(list, hasPlate);
    }
    public void UpdateContent(IListable list)
    {
        UIElements.GetInstance().UpdateObjectContent(list);
    }
    public void HideContent()
    {
        UIElements.GetInstance().HideObjectContent();
    }
    public void SetOutline(bool hasOutline)
    {
        if(!hasFinishOutline)
            outline.enabled = hasOutline;
    }

    public void SetFinishOutline(bool hasOutline)
    {
        outline.enabled = hasOutline;
        hasFinishOutline = hasOutline;
        if(hasOutline)
            SetOutlineColor(Color.green);
        else
            SetOutlineColor(Color.white);

    }
    void SetOutlineColor(Color color)
    {
        outline.OutlineColor = color;
    }
}
