using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ShowObjectInfo))]
public class DraggableObject : MonoBehaviour
{
    Rigidbody rb;
    Vector3 targetPosition;
    AudioSource audioSource;
    bool follow;

    [SerializeField]
    float minCollisionForce = 10f;
    [SerializeField]
    float maxCollisionForce = 60f;
    [SerializeField]
    float maxVolume = 1f;

    public DraggableType Type = DraggableType.Food;
    public bool CanDrag = true;
    public Vector3 PlaceOffset = Vector3.zero;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
            rb.linearVelocity = moveDirection * 15f;
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
        follow = false;
        rb.linearVelocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("DraggableObject");
        }
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
        this.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.relativeVelocity.magnitude;

        float volume = Mathf.Clamp((collisionForce - minCollisionForce) / (maxCollisionForce - minCollisionForce), 0, 1) * maxVolume;

        if (volume > 0)
        {
            audioSource.volume = volume;
            audioSource.pitch = Random.Range(audioSource.pitch - 0.5f, audioSource.pitch + 0.5f);
            audioSource.Play();
        }
    }
}
