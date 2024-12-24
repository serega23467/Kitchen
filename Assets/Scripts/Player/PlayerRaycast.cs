using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float raycastDistance = 2f;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    float showInfoDistance = 1f;
    [SerializeField]
    float minDraggableObjectDistance = 1f;
    [SerializeField]
    float maxDraggableObjectDistance = 1.5f;
    [SerializeField]
    CameraSwitcher cameraSwitcher;
    float draggableObjectDistance = 1.5f;
    DraggableObject currentDraggableObject = null;
    bool isShowInfo = true;
    ShowObjectInfo currentInfoObject = null;
    //bool isInEntiractive = false;

    private void Start()
    {
        //cameraSwitcher = GetComponent<CameraSwitcher>();
    }
    void Update()
    {
        if(isShowInfo)
        {
            RaycastHit infoHit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out infoHit, showInfoDistance, LayerMask.GetMask("DraggableObject") | LayerMask.GetMask("InteractiveObject")))
            {
                if (infoHit.collider.TryGetComponent(out ShowObjectInfo info))
                {
                    if (currentInfoObject == null)
                    {
                        currentInfoObject = info;
                        currentInfoObject.SetOutline(true);
                        currentInfoObject.ShowInfo();

                    }
                    else if (currentInfoObject != info)
                    {
                        currentInfoObject.SetOutline(false);
                        currentInfoObject.HideInfo();
                        currentInfoObject = info;
                        currentInfoObject.SetOutline(true);
                        currentInfoObject.ShowInfo();
                    }
                    else
                    {
                        currentInfoObject.ShowInfo();
                    }
                }
            }
            else if (currentInfoObject != null)
            {
                currentInfoObject.SetOutline(false);
                currentInfoObject.HideInfo();
                currentInfoObject = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isShowInfo = !isShowInfo;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentDraggableObject != null)
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
                    if(draggableObject.CanDrag)
                    {
                        draggableObject.StartFollowingObject();
                        currentDraggableObject = draggableObject;
                    }
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
                    if(currentDraggableObject != null && currentDraggableObject.Type == inclose.DraggableType)
                    {
                        if (!inclose.HasObject)
                        {
                            currentDraggableObject.CanDrag = false;
                            inclose.Put(currentDraggableObject);
                            currentDraggableObject = null;
                        }
                    }
                    else if(inclose.HasObject)
                    {
                        var d = inclose.Pick();
                        d.CanDrag = true;
                        d.StartFollowingObject();
                        currentDraggableObject = d;
                    }
                }
                else if(hit.transform.parent.TryGetComponent(out Inclose inclose2))
                {
                    if (currentDraggableObject != null)
                    {
                        if (!inclose2.HasObject)
                        {
                            currentDraggableObject.CanDrag = false;
                            inclose2.Put(currentDraggableObject);
                            currentDraggableObject = null;
                        }
                    }
                    else if (inclose2.HasObject)
                    {
                        var d = inclose2.Pick();
                        d.CanDrag = true;
                        d.StartFollowingObject();
                        currentDraggableObject = d;
                    }
                }
                else if (hit.collider.TryGetComponent(out Switcher switcher))
                {
                    switcher.AddLevel();
                }
                else if (hit.collider.TryGetComponent(out Tap tap) && currentDraggableObject != null)
                {
                    if(currentDraggableObject.TryGetComponent(out IHeated heated))
                    {
                        if(!heated.HeatedInfo.HasWater)
                        {
                            tap.FillContainer(heated);
                        }
                    }
                }
            }
        }
        if (currentDraggableObject!=null && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            draggableObjectDistance = Mathf.Clamp(draggableObjectDistance+Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime*20f, minDraggableObjectDistance, maxDraggableObjectDistance);
        }
        if (currentDraggableObject != null)
        {
            currentDraggableObject.SetTargetPosition((cam.transform.position + cam.transform.forward * draggableObjectDistance) + offset);
        }
    }
}