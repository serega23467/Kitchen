using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 targetPosition;
    bool follow;
    public bool canPick = true;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.maxAngularVelocity = 1;
    }

    void FixedUpdate()
    {
        if(canPick)
        {
            if (!follow)
                return;
            Vector3 moveDirection = targetPosition - _rb.position;
            _rb.linearVelocity = moveDirection * 15f;
        }
    }
    public void StartFollowingObject()
    {
        if (canPick)
            follow = true;
    }
    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        if (canPick)
            targetPosition = newTargetPosition;
    }
    public void StopFollowingObject()
    {
        if (canPick)
        {
            follow = false;
            _rb.linearVelocity = Vector3.zero;
        }
    }
}
