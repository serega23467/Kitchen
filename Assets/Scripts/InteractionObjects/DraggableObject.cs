using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowObjectInfo))]
public class DraggableObject : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 targetPosition;
    bool follow;
    public DraggableType Type = DraggableType.Food;
    public bool CanDrag = true;
    public Vector3 PlaceOffset = Vector3.zero;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = 1f;
        _rb.maxLinearVelocity = 15f;
    }

    void FixedUpdate()
    {
        if(CanDrag)
        {
            if (!follow)
                return;
            Vector3 moveDirection = targetPosition - _rb.position;
            _rb.linearVelocity = moveDirection * 15f;
        }
    }
    public void StartFollowingObject()
    {
        if (CanDrag && !follow)
        {
            follow = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        if (CanDrag)
            targetPosition = newTargetPosition;
    }
    public void StopFollowingObject()
    {
        if (CanDrag)
        {
            follow = false;
            _rb.linearVelocity = Vector3.zero;
        }
        gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        }
    }
    public void OffRigidbody()
    {
        Destroy(_rb);
        _rb = null;
    }
    public void OnRigidbody()
    {
        var rb = gameObject.AddComponent<Rigidbody>();
        _rb = rb;
        _rb.maxAngularVelocity = 1f;
        _rb.maxLinearVelocity = 15f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
