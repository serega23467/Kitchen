using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public Camera cam;
    public float raycastDistance = 2f;
    public float draggableObjectDistance = 2f;
    public DraggableObject currentDraggableObject = null;
    public CameraSwitcher cameraSwitcher;
    //bool isInEntiractive = false;

    private void Start()
    {
        //cameraSwitcher = GetComponent<CameraSwitcher>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentDraggableObject != null)
            {
                currentDraggableObject.StopFollowingObject();
                currentDraggableObject = null;
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
            {
                if (hit.collider.TryGetComponent(out DraggableObject draggableObject))
                {
                    draggableObject.StartFollowingObject();
                    currentDraggableObject = draggableObject;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("InteractiveObject")))
            {
                if (hit.collider.TryGetComponent(out OpenDoor door))
                {
                    if (door.IsOpen)
                    {
                        door.Close();
                    }
                    else
                    {
                        door.Open();
                    }
                }
                else if (hit.collider.TryGetComponent(out Inclose inclose))
                {
                    if(currentDraggableObject != null)
                    {
                        currentDraggableObject.StopFollowingObject();
                        currentDraggableObject.canPick = false;
                        inclose.Put(currentDraggableObject.gameObject);
                        currentDraggableObject = null;
                       
                    }
                    else
                    {
                        var c = currentDraggableObject = inclose.Pick();
                        if(c != null)
                        {
                            c.canPick = true;
                            c.StartFollowingObject();
                            currentDraggableObject = c;
                        }
                    }
                }
            }
        }
        if (currentDraggableObject != null)
        {
            currentDraggableObject.SetTargetPosition(cam.transform.position + cam.transform.forward * draggableObjectDistance);
        }
    }
}