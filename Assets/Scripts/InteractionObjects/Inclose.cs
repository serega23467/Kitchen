using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Inclose : MonoBehaviour
{
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [Range(0, 3)]
    public byte Id { get; set; }    
    public DraggableType DraggableType = DraggableType.Food;
    public DraggableObject PickedObject { get; private set; }
    public UnityEvent<byte, bool> OnPickPut { get; private set; } = new UnityEvent<byte, bool>();
    public bool HasObject { get; private set; }
    public void Put(DraggableObject ob)
    {
        if (DraggableType == ob.Type)
        {
            if (ob.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                ob.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
            }
            PickedObject = ob;
            PickedObject.transform.parent = transform;
            if (ob.gameObject.TryGetComponent(out FryingPan pan))
            {
                PickedObject.transform.position = transform.position + new Vector3(0, 0.05f, 0);
            }
            else
            {
               PickedObject.transform.position = transform.position + offset;
            }
            PickedObject.GetComponent<Rigidbody>().isKinematic = true;
            HasObject = true;
            OnPickPut.Invoke(Id, HasObject);
        }
    }
    public DraggableObject Pick()
    {
        PickedObject.transform.parent = Parents.GetInstance().FoodParent.transform;
        PickedObject.GetComponent<Rigidbody>().isKinematic = false;
        DraggableObject pickedDO = PickedObject;
        HasObject = false;
        OnPickPut.Invoke(Id, HasObject);
        PickedObject = null;
        return pickedDO;
    }
}
