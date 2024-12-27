using DG.Tweening;
using UnityEngine;

public class Tap : MonoBehaviour
{
    Tween currentAnim;
    public void FillContainer(IHeated container)
    {
        if (currentAnim != null)
            return;
        currentAnim = DOTween.Sequence()
        .Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0.5f))
        .SetDelay(0.5f)
        .Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f))
        .OnComplete(() => { currentAnim = null; container.AddWater(); })
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
