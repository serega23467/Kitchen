using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerControls PlayerControls;
    public float WalkingSpeed = 7.5f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public Camera PlayerCamera;
    public float LookSpeed = 2.0f;
    public float LookXLimit = 45.0f;
    public float CrouchHeight = 0.6f;
    [SerializeField]
    AudioSource playerSource;
    Tween downAnim;
    Tween upAnim;
    Tween currentAnim;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    bool isCrouching = false;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        PlayerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        PlayerControls.Player.Enable();
        PlayerControls.Player.Jump.performed += OnJump;
        PlayerControls.Player.Seet.performed += OnSeet;
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var p = PlayerCamera.transform.localPosition;
        downAnim = PlayerCamera.transform.DOLocalMoveY(p.y - CrouchHeight, 0.5f).SetAutoKill(false).OnComplete(() => { currentAnim = null; });
        upAnim = PlayerCamera.transform.DOLocalMoveY(p.y, 0.5f).SetAutoKill(false).OnComplete(() => { currentAnim = null; });
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 axisValues = PlayerControls.Player.Movement.ReadValue<Vector2>();
        float curSpeedX = canMove ? WalkingSpeed * axisValues.y : 0;
        float curSpeedY = canMove ? WalkingSpeed * axisValues.x : 0;
        if(isCrouching)
        {
            curSpeedX /= 1.5f;
            curSpeedY /= 1.5f;
            playerSource.pitch = 0.8f + Random.Range(-0.1f, 0.1f);
            playerSource.volume = 0.05f;
        }
        else
        {
            playerSource.pitch = 1.0f+Random.Range(-0.1f, 0.1f);
            playerSource.volume = 0.1f;
        }
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;
        if (!characterController.isGrounded)
        {
            moveDirection.y -= Gravity * Time.deltaTime;
        }
        if (canMove)
        {
            characterController.Move(moveDirection * Time.deltaTime);
            if (new Vector3(moveDirection.x,0,moveDirection.z).magnitude > 0.1f && characterController.isGrounded)
            {
                if (playerSource != null && !playerSource.isPlaying)
                {
                    playerSource.Play();
                }
            }
            rotationX += -PlayerControls.Player.Look.ReadValue<Vector2>().y * LookSpeed;
            rotationX = Mathf.Clamp(rotationX, -LookXLimit, LookXLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, PlayerControls.Player.Look.ReadValue<Vector2>().x * LookSpeed, 0);
        }
    }
    public void OnMenuMode()
    {
        canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OffMenuMode()
    {
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnJump(CallbackContext context)
    {
        if (canMove && characterController.isGrounded)
        {
            moveDirection.y = JumpSpeed;
        }
    }
    void OnSeet(CallbackContext context)
    {
        if(canMove)
        {
            if (currentAnim != null)
                return;
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                downAnim.Restart();
            }
            else
            {
                upAnim.Restart();
            }
        }
    }
    private void OnDisable()
    {
        PlayerControls.Player.Disable();
        PlayerControls.Player.Jump.performed -= OnJump;
        PlayerControls.Player.Seet.performed -= OnSeet;
    }
}
