using UnityEngine;

public class Inclose : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    GameObject pickedObject;
    void Start()
    {
        offset = Vector3.zero;
    }
    public void Put(GameObject ob)
    {
        pickedObject = ob;
        pickedObject.transform.parent = transform;
        pickedObject.transform.position = transform.position + offset;
        pickedObject.GetComponent<Rigidbody>().isKinematic = true;
        pickedObject.transform.parent = transform;
    }
    public DraggableObject Pick()
    {
        if(pickedObject == null)
            return null;
        pickedObject.transform.parent = Parents.GetInstance().FoodParent.transform;
        pickedObject.GetComponent<Rigidbody>().isKinematic = false;
        DraggableObject pickedDO = pickedObject.GetComponent<DraggableObject>();
        pickedObject = null;
        return pickedDO;
    }
}
