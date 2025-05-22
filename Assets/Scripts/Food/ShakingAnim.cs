using DG.Tweening;
using UnityEngine;

public class ShakingAnim : MonoBehaviour
{
    [SerializeField]
    Vector3 angle = new Vector3(0, 0, 10);
    [SerializeField]
    float time = 0.5f;
    Tween shakeAnim;
    private void Awake()
    {
        shakeAnim = transform
            .DOShakeRotation(time, angle, vibrato:1)
            .SetAutoKill(false);

    }
    public void Shake()
    {
        shakeAnim.Restart();
    }
}
