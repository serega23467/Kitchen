using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    PlayerControls playerControls;
    public float WalkingSpeed = 7.5f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;
    public Camera PlayerCamera;
    public float LookSpeed = 2.0f;
    public float LookXLimit = 45.0f;
    public float CrouchHeight = 0.6f;
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
        playerControls = new PlayerControls();
        SettingsInit.InitControls(playerControls);
        LookSpeed = SettingsInit.GetSensetivity();
    }
    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += OnJump;
        playerControls.Player.Seet.performed += OnSeet;
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

        Vector2 axisValues = playerControls.Player.Movement.ReadValue<Vector2>();
        float curSpeedX = canMove ? WalkingSpeed * axisValues.y : 0;
        float curSpeedY = canMove ? WalkingSpeed * axisValues.x : 0;
        if(isCrouching)
        {
            curSpeedX /= 1.5f;
            curSpeedY /= 1.5f;
        }
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;
        if (!characterController.isGrounded)
        {
            moveDirection.y -= Gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -playerControls.Player.Look.ReadValue<Vector2>().y * LookSpeed;
            rotationX = Mathf.Clamp(rotationX, -LookXLimit, LookXLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, playerControls.Player.Look.ReadValue<Vector2>().x * LookSpeed, 0);
        }
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
        playerControls.Player.Disable();
        playerControls.Player.Jump.performed -= OnJump;
        playerControls.Player.Seet.performed -= OnSeet;
    }
}
