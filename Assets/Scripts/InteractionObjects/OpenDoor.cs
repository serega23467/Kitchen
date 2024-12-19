using DG.Tweening;
using UnityEngine;

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

    Tween currenAnim;

    private void Awake()
    {
        if (type == OpenType.Opening)
        {
            openAnim = transform
                .DOLocalRotate(angle, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currenAnim = null; IsOpen = true; });
            closeAnim = transform
                .DOLocalRotate(transform.localEulerAngles, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currenAnim = null; IsOpen = false; });
        }
        else if (type == OpenType.Retractable)
        {
            openAnim = transform
                .DOMoveZ(transform.position.z + zDelta, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currenAnim = null; IsOpen = true; }); ;
            closeAnim = transform
                .DOMoveZ(transform.position.z, 1.5f)
                .SetAutoKill(false)
                .OnComplete(() => { currenAnim = null; IsOpen = false; }); ;
        }
    }
    public void Open()
    {   
        if(currenAnim != null)
        {
            return;
        }
        currenAnim = openAnim;
        openAnim.Restart();
    }
    public void Close()
    {
        if (currenAnim != null)
        {
            return;
        }
        currenAnim = closeAnim;
        closeAnim.Restart();
    }
}
