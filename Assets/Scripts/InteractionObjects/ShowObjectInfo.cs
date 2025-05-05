using System.Collections.Generic;
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
    Outline outline;
    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    void Start()
    {
        UIElements.GetInstance().HideObjectInfo();
        UIElements.GetInstance().HideObjectContent();
    }
    public void ShowInfo()
    {
        if (ObjectInfo != "" || ObjectData!="" || ObjectName !="")
            UIElements.GetInstance().ShowObjectInfo(ObjectName + "\n", ObjectInfo, ObjectData);
    }
    public void HideInfo()
    {
        UIElements.GetInstance().HideObjectInfo();
    }
    public void ShowContent(IListable list, bool hasPlate = true)
    {
        UIElements.GetInstance().ShowObjectContent(list, hasPlate);
    }
    public void UpdateContent(List<FoodComponent> list)
    {
        UIElements.GetInstance().UpdateObjectContent(list);
    }
    public void HideContent()
    {
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
