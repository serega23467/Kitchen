using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CutBoard : MonoBehaviour
{
    public void CutObject(Plate plate)
    {
        if (plate.Food != null)
        {
            if (!plate.Food.IsPour)
            {
                Array.Sort(plate.Food.AllCutTypes);
                int currentIndex = Array.IndexOf(plate.Food.AllCutTypes, plate.Food.CurrentCutType);
                if (currentIndex != -1)
                {
                    if (currentIndex < plate.Food.AllCutTypes.Length - 1)
                    {
                        plate.Food.CurrentCutType = plate.Food.AllCutTypes[currentIndex + 1];
                        plate.UpdateInfo();
                    }
                }
            }
        }
    }
}
