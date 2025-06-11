using DG.Tweening;
using UnityEngine;

public class Tap : MonoBehaviour
{
    Tween currentAnim;
    [SerializeField]
    AudioSource waterSource;
    private void Start()
    {
        if (waterSource != null)
        {
            waterSource.volume = 0.1f;
        }
    }
    public void FillContainer(IHeated container)
    {
        if (currentAnim != null)
            return;
        if(waterSource != null) waterSource.Play();
        currentAnim = DOTween.Sequence()
        .Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0.5f))
        .SetDelay(0.5f)
        .Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f))
        .OnComplete(() => { currentAnim = null; container.AddWater(); if (waterSource != null) waterSource.Stop(); })
        .Play();
       //transform
       //.DOLocalRotate(new Vector3(0, 0, 50), 0.5f)
       //.d
       //.SetAutoKill(false)
       //.OnComplete(() => { currentAnim = null; })
       //.Play();
       // container.AddWater();
    }
}
