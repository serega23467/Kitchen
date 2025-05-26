using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class FoodComponent : MonoBehaviour, ICloneable
{
    public string FoodName;
    [HideInInspector]
    public UnityEvent<FoodComponent> OnPull;
    [SerializeField]
    public byte CutCount;
    [SerializeField]
    GameObject nextCuttedPart;
    [SerializeField]
    GameObject pourPrefab;
    ShowObjectInfo info;
    [HideInInspector]
    public Plate plate;
    public FoodInfo FoodInfo { get; set; }
    ShakingAnim anim;
    private void Start()
    {
        bool isInstanceSpawned = true;
        anim = GetComponent<ShakingAnim>();
        info = GetComponent<ShowObjectInfo>();
        if (FoodInfo==null)
        {
            isInstanceSpawned = false;
            FoodInfo = Resources.Load<FoodInfo>("Food/" + FoodName).Clone();
            //FoodInfo.CurrentCutType = CutType.None;
        }
        //FoodInfo.Params = new List<FoodParametr>();
        if (FoodInfo.IsPour)
        {
            if (isInstanceSpawned)
            {
                info.ObjectInfo = $"{FoodInfo.GramsWeight} г";
            }
            else
            {
                info.ObjectInfo = $"{FoodInfo.GramsWeight} г за раз";
            }
        }
        else
        {
            info.ObjectInfo = $"{FoodInfo.GramsWeight.ToString("N1")} г\n({Translator.GetInstance().GetTranslate(FoodInfo.CurrentCutType.ToString())})";
        }
        OnPull = new UnityEvent<FoodComponent>();
        UpdateParamsData();
    }
    public bool TryAddParameterValue(string paramName, float value)
    {
        if (FoodInfo != null)
        {
            var param = FoodInfo.Params.FirstOrDefault(p => p.ParamName == paramName);
            if(param == null)
            {
                if(TryAddParameter(paramName))
                {
                    param = FoodInfo.Params.FirstOrDefault(p => p.ParamName == paramName);
                }
                else
                {
                    return false;
                }
            }
            param.ParamValue += value;
            UpdateParamsData();
            return true;
        }
        return false;
    }
    public bool TrySetParameterValue(string paramName, float value)
    {
        if(FoodInfo!=null)
        {
            var param = FoodInfo.Params.FirstOrDefault(p => p.ParamName == paramName);
            if (param == null)
            {
                if (TryAddParameter(paramName))
                {
                    param = FoodInfo.Params.FirstOrDefault(p => p.ParamName == paramName);
                }
                else
                {
                    return false;
                }
            }
            param.ParamValue = value;
            UpdateParamsData();
            return true;
        }
        return false;
    }
    public void Cut()
    {
        if (nextCuttedPart==null)
        {
            return;
        }
        Plate plateForParts = plate;
        if(plate!=null)plate.RemoveFood(this);
        for (int i = 0; i < CutCount; i++)
        {
            var part = Instantiate(nextCuttedPart, transform.position+new Vector3(UnityEngine.Random.Range(-0.10f, 0.10f),0, UnityEngine.Random.Range(-0.10f, 0.10f)), Quaternion.identity,  transform.parent);
            var partFoodComponent = part.GetComponent<FoodComponent>();
            partFoodComponent.FoodInfo = FoodInfo.Clone();
            partFoodComponent.FoodInfo.GramsWeight = FoodInfo.GramsWeight/(float)CutCount;
            foreach(var param in partFoodComponent.FoodInfo.Params)
            {
                if (param.IsSpice)
                {
                    param.ParamValue /= CutCount;
                }
            }
            if(plateForParts != null)
            {
                if(plateForParts.TryAddFood(partFoodComponent))
                {
                    var dgo = part.GetComponent<DraggableObject>();
                    if(dgo!=null)
                    {
                        dgo.OffRigidbody();
                        dgo.CanDrag = false;
                    }
                }
            }
            else
            {
                part.gameObject.transform.parent = Parents.GetInstance().FoodParent.transform;
            }
        }
        Destroy(gameObject);
        
    }
    public bool GetPour(out FoodComponent food)
    {
        food = null;
        if(pourPrefab == null || !FoodInfo.IsPour)
        {
            return false;
        }
        var pour = Instantiate(pourPrefab, transform.position, Quaternion.identity, transform.parent);

        float scaleFactor = FoodInfo.GramsWeight / 40f;
        Vector3 scale = pour.transform.localScale;
        pour.gameObject.transform.localScale = new Vector3(scale.x*scaleFactor, scale.y*scaleFactor, scale.z*scaleFactor);
        food = pour.GetComponent<FoodComponent>();
        food.FoodInfo = FoodInfo.Clone();
        anim?.Shake();
        return true;
    }
    public object Clone()
    {
        return this.MemberwiseClone();
    }
    void UpdateParamsData()
    {
        info.ObjectData = "";
        if (FoodInfo == null)
        {
            return;
        }
        else if (FoodInfo.Params.Count <= 0)
        {
            return;
        }
        StringBuilder sb = new StringBuilder();
        foreach (var p in FoodInfo.Params)
        {
            sb.AppendLine($"{p.Desc} - {p.ParamValue.ToString("N1")} {p.ValueChar}");
        }
        info.ObjectData = sb.ToString();
    }
    bool TryAddParameter(string paramName)
    {
        if (FoodInfo != null)
        {
            var param = FoodInfo.Params.FirstOrDefault(p => p.ParamName == name);
            if (param != null) return true;
            FoodParametr foodParametr;
            if (FoodParametersPool.GetInstance().TryGetParameter(paramName, out foodParametr))
            {
                FoodInfo.Params.Add(foodParametr);
                return true;
            }
            return false;
        }
        return false;
    }
}
