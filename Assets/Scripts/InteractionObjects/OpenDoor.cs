using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(ShowObjectInfo))]
public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    OpenType type = OpenType.Opening;
    [SerializeField]
    Vector3 angle = new Vector3(0, 90, 0);
    [SerializeField]
    float zDelta = 1;
    public bool IsOpen { get; private set; }
    Tween openAnim;
    Tween closeAnim;

    Tween currentAnim;

    private void Awake()
    {
        if (type == OpenType.Opening)
        {
            openAnim = transform
                .DOLocalRotate(angle, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currentAnim = null; IsOpen = true; });
            closeAnim = transform
                .DOLocalRotate(transform.localEulerAngles, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currentAnim = null; IsOpen = false; });
        }
        else if (type == OpenType.Retractable)
        {
            openAnim = transform
                .DOLocalMoveZ(transform.localPosition.z + zDelta, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currentAnim = null; IsOpen = true; }); ;
            closeAnim = transform
                .DOLocalMoveZ(transform.localPosition.z, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currentAnim = null; IsOpen = false; }); ;
        }
    }
    public void Open()
    {   
        if(currentAnim != null)
        {
            return;
        }
        currentAnim = openAnim;
        openAnim.Restart();
    }
    public void Close()
    {
        if (currentAnim != null)
        {
            return;
        }
        currentAnim = closeAnim;
        closeAnim.Restart();
    }
}
