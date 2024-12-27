using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(ShowObjectInfo))]
public class Plate : MonoBehaviour
{
    [SerializeField]
    GameObject content;
    ShowObjectInfo info;
    public IFood Food { get; private set; }
    Dictionary<string, string> translator;
    private void Start()
    {
        info = GetComponent<ShowObjectInfo>();
        info.ObjectName = "�������";
        content.SetActive(false);
        translator = new Dictionary<string, string>()
        {
            { "None", "�� ��������" },           
            { "Large", "������" },
            { "Medium", "������" },
            { "Finely", "�����" },
        };
    }
    public void AddFood(IFood food)
    {
        if(Food == null || Food != food)
        {
            Food = food;
        }
        else if (Food == food)
        {
            Food.GramsWeight += food.GramsWeight;
        }
        UpdateInfo();
        content.SetActive(true);        
    }
    public void RemoveFood(out IFood food)
    {
        food = Food;
        if (Food != null)
        {
            content.SetActive(false);
            Food = null;
            UpdateInfo();
        }
    }
    public void UpdateInfo()
    {
        if(Food == null)
        {
            info.ObjectData = "";
            return;
        }
        info.ObjectData = $"{Food.FoodName} - {Food.GramsWeight} �\n������� - {translator[Food.CurrentCutType.ToString()]}";
    }    
}
