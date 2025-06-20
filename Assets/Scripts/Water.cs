using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
public class Water : MonoBehaviour
{
    float yMaxLevel;
    float yMinLevel;

    private void Awake()
    {
        yMaxLevel = transform.localPosition.y;
        yMinLevel = 0.01f;
    }
    public void ChangeWaterLevel(float mass, float minMass, float fullMass)
    {
        var pos = transform.localPosition;
        float lerpT = Mathf.Clamp01((mass - minMass) / (fullMass - minMass));
        float y = Mathf.Lerp(yMinLevel, yMaxLevel, lerpT);
        if (transform.localPosition != new Vector3(pos.x, y, pos.z))
            transform.localPosition = new Vector3(pos.x, y, pos.z);
    }  
}
