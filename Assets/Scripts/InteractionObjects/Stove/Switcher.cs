using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowObjectInfo))]
public class Switcher : MonoBehaviour
{
    [Range(0, 3)]
    public byte Id { get; set; }
    [SerializeField]
    float rotateAngle = 30;
    float currentAngle = 0;
    public byte Level { get; private set; } = 0;
    public UnityEvent<byte, byte> OnSwitched { get; private set; } = new UnityEvent<byte, byte> ();

    Tween openAnim;
    Tween closeAnim;
    Tween currentAnim;

    public void AddLevel()
    {
        if(Level+1 <= 3)
        {
            if (currentAnim != null)
            {
                return;
            }
            Level++;
            OnSwitched.Invoke(Id, Level);
            currentAngle = rotateAngle * Level;
            currentAnim = transform
                   .DOLocalRotate(new Vector3(0,0,currentAngle), 0.5f)
                   .SetAutoKill(false)
                   .OnComplete(() => { currentAnim = null; })
                   .Play();
        }
        else 
        {
            Level=0;
            OnSwitched.Invoke(Id, Level);
            currentAngle = rotateAngle * Level;
            currentAnim = transform
                    .DOLocalRotate(new Vector3(0, 0, currentAngle), 0.5f)
                    .SetAutoKill(false)
                    .OnComplete(() => { currentAnim = null; })
                    .Play();
        }
    }
}
