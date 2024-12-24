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
            PickedObject = ob;
            PickedObject.transform.parent = transform;
            PickedObject.transform.position = transform.position + offset;
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
