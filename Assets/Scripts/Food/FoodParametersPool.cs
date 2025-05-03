using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodParametersPool : MonoBehaviour
{
    static FoodParametersPool instance;
    List<FoodParametr> poolList;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        poolList = new List<FoodParametr>();
    }
    public static FoodParametersPool GetInstance()
    {
        return instance;
    }
    public bool TryGetParameter(string paramName, out FoodParametr foodParametr)
    {
        foodParametr = null;
        if (!poolList.Select(p=>p.ParamName).Contains(paramName))
        {
            var param = Resources.Load<FoodParametr>("Food/Params/" + paramName).Clone();
            if(param == null)
            {
                return false;
            }
            poolList.Add(param);
        }
        foodParametr = poolList.FirstOrDefault(p => p.ParamName == paramName).Clone();
        return true;

    }
}
