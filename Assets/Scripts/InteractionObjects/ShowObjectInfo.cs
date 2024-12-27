using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class ShowObjectInfo : MonoBehaviour
{
    public string ObjectName = "";
    public string ObjectInfo = "";
    [HideInInspector]
    public string ObjectData = "";
    public bool IsContentShowed { get; private set; } = false;
    Outline outline;
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        UIElements.GetInstance().HideObjectInfo();
        UIElements.GetInstance().HideObjectContent();
    }
    public void ShowInfo()
    { 
        if(ObjectInfo!="")
        {
            UIElements.GetInstance().ShowObjectInfo(ObjectName +"\n", ObjectInfo, ObjectData);
        }
    }
    public void HideInfo()
    {
        UIElements.GetInstance().HideObjectInfo();
    }
    public void ShowContent(IListable list)
    {
        IsContentShowed = true;
        UIElements.GetInstance().ShowObjectContent(list);
    }
    public void UpdateContent(IListable list)
    {
        UIElements.GetInstance().UpdateObjectContent(list);
    }
    public void HideContent()
    {
        IsContentShowed = false;
        UIElements.GetInstance().HideObjectContent();
    }
    public void SetOutline(bool hasOutline)
    {
        outline.enabled = hasOutline;
    }
    public void SetOutlineColor(Color color)
    {
        outline.OutlineColor = color;
    }
}
