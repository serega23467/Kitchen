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
    Outline outline;
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        UIElements.GetInstance().HideObjectInfo();
    }
    public void ShowInfo()
    { 
        if(ObjectInfo!="")
        {
            UIElements.GetInstance().ShowObjectInfo(ObjectName, ObjectInfo);
        }
    }
    public void HideInfo()
    {
        UIElements.GetInstance().HideObjectInfo();
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
