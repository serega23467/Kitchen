using System;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField]
    AudioSource takeSource;
    [SerializeField]
    AudioSource cutSource;
    private void Start()
    {
        takeSource.volume = 0.1f;
        cutSource.volume = 0.1f;
    }
    public void TakeKnife()
    {
        if (takeSource != null && !takeSource.isPlaying)
        {
            takeSource.Play();
        }
    }
    public void Cut(FoodComponent food)
    {
        if(food.FoodInfo.CurrentCutType==CutType.Finely || food.FoodInfo.AllCutTypes.Length<=1 || food.FoodInfo.IsPour)
        {
            return;
        }
        Array.Sort(food.FoodInfo.AllCutTypes);
        int currentIndex = Array.IndexOf(food.FoodInfo.AllCutTypes, food.FoodInfo.CurrentCutType);
        if (currentIndex != -1)
        {
            if (currentIndex < food.FoodInfo.AllCutTypes.Length - 1)
            {
                food.FoodInfo.CurrentCutType = food.FoodInfo.AllCutTypes[currentIndex + 1];
            }
        }
        
        if(cutSource!=null)
        {
            cutSource.pitch = 0.8f + UnityEngine.Random.Range(-0.1f, 0.1f);
            cutSource.Play();
        }
        food.Cut();
    }
}
