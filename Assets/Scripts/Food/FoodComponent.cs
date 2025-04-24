using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FoodComponent : MonoBehaviour, ICloneable
{
    public string FoodName;
    [SerializeField]
    public byte CutCount;
    [SerializeField]
    GameObject nextCuttedPart;
    [SerializeField]
    GameObject pourPrefab;
    UnityEvent<GameObject> OnPull;
    public Plate plate;
    public FoodInfo FoodInfo { get; set; }
    private void Start()
    {
        bool isInstanceSpawned = true;
        if(FoodInfo==null)
        {
            isInstanceSpawned = false;
            FoodInfo = Resources.Load<FoodInfo>("Food/" + FoodName);
            FoodInfo.CurrentCutType = CutType.None;
        }
        if (FoodInfo.IsPour)
        {
            if (isInstanceSpawned)
            {
                GetComponent<ShowObjectInfo>().ObjectData = $"{FoodInfo.GramsWeight} г";
            }
            else
            {
                GetComponent<ShowObjectInfo>().ObjectData = $"{FoodInfo.GramsWeight} г насыпи";
            }
        }
        else
        {
            GetComponent<ShowObjectInfo>().ObjectData = $"{FoodInfo.GramsWeight.ToString("N1")} г\n({Translator.GetInstance().GetTranslate(FoodInfo.CurrentCutType.ToString())})";
        }
        OnPull = new UnityEvent<GameObject>();
    }
    public void AddFoodParameterValue(string paramName, float value)
    {
        if (FoodInfo != null)
        {
            var param = FoodInfo.Params.FirstOrDefault(p => p.name == paramName);
            if (param == null)
            {
                FoodInfo.Params.Add(new FoodParametr(paramName, value));
            }
            else
            {
                param.ParamValue += value;
            }
        }
    }
    public void SetFoodParameterValue(string paramName, float value)
    {
        if(FoodInfo!=null)
        {
            var param = FoodInfo.Params.FirstOrDefault(p => p.name == paramName);
            if (param==null)
            {
                FoodInfo.Params.Add(new FoodParametr(paramName, value));
            }
            else
            {
                param.ParamValue = value;
            }
        }
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
            var part = Instantiate(nextCuttedPart, transform.position+new Vector3(UnityEngine.Random.Range(0, 0.15f),0, UnityEngine.Random.Range(0, 0.15f)), Quaternion.identity,  transform.parent);
            var partFoodComponent = part.GetComponent<FoodComponent>();
            partFoodComponent.FoodInfo = FoodInfo.Clone() as FoodInfo;
            partFoodComponent.FoodInfo.GramsWeight = FoodInfo.GramsWeight/(float)CutCount;
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
        food.FoodInfo = FoodInfo.Clone() as FoodInfo;
        return true;
    }
    public object Clone()
    {
        return this.MemberwiseClone();
    }
    public GameObject GetPlatePrefab()
    {
        throw new System.NotImplementedException();
    }

}
