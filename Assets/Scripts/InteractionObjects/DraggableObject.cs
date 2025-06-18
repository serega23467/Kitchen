using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(ShowObjectInfo))]
public class DraggableObject : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    float minCollisionForce = 10f;
    [SerializeField]
    float maxCollisionForce = 60f;
    [SerializeField]
    float maxVolume = 0.6f;

    Rigidbody rb;
    bool follow;
    Vector3 targetPosition;
    List<Collider> colliders;

    public DraggableType Type = DraggableType.Food;
    public bool CanDrag = true;
    public Vector3 PlaceOffset = Vector3.zero;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 1f;
        rb.maxLinearVelocity = 15f;
    }

    void FixedUpdate()
    {
        if(CanDrag)
        {
            if (!follow)
                return;
            Vector3 moveDirection = targetPosition - rb.position;
            rb.linearVelocity = moveDirection * 5f;
        }
    }
    public void StartFollowingObject()
    {
        if (CanDrag && !follow)
        {
            follow = true;
            gameObject.layer = LayerMask.NameToLayer("OnHands");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("OnHands");
            }
            rb.gameObject.transform.localRotation = Quaternion.identity;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        if (CanDrag)
            targetPosition = newTargetPosition;
    }
    public void StopFollowingObject()
    {
        follow = false;
        //rb.linearVelocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        }
        rb.constraints = RigidbodyConstraints.None;      
    }
    public void OffRigidbody()
    {
        Destroy(rb);
        rb = null;
    }
    public void OnRigidbody()
    {
        var rb = gameObject.AddComponent<Rigidbody>();
        this.rb = rb;
        this.rb.maxAngularVelocity = 1f;
        this.rb.maxLinearVelocity = 15f;     
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource == null) return;
        float collisionForce = collision.relativeVelocity.magnitude;

        float volume = Mathf.Clamp((collisionForce - minCollisionForce) / (maxCollisionForce - minCollisionForce), 0, 1) * maxVolume;

        if (volume > 0)
        {
            audioSource.volume = volume;

            audioSource.pitch = 1.0f + Random.Range(0f, 0.2f); 
            audioSource.Play();
        }
    }
}
