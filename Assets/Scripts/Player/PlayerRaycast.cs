using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

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
    float draggableObjectDistance = 1.5f;
    public DraggableObject CurrentDraggableObject { get; set; } = null;
    bool isShowInfo = true;
    ShowObjectInfo currentInfoObject = null;
    bool isShowContent = false;
    PlayerController playerController = null;
    bool isShowResult = false;
    bool isRecipeShow = true;
    bool gameIsPaused = false;
    //bool isInEntiractive = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Start()
    {
        //cameraSwitcher = GetComponent<CameraSwitcher>();
        UIElements.GetInstance().HidePanelResult();
        UIElements.GetInstance().HideMenu();
        UIElements.GetInstance().HideSettings();
        UpdateSettings();
    }
    private void OnEnable()
    {
        playerController.PlayerControls.Player.Enable();
        playerController.PlayerControls.Player.Take.performed += TakeItem;
        playerController.PlayerControls.Player.Interect.performed += Interect;
        playerController.PlayerControls.Player.Escape.performed += delegate (CallbackContext context) { Escape(); };
        playerController.PlayerControls.Player.Pour.performed += Pour;
    }
    public void PickFood(IFood ob)
    {
        if (CurrentDraggableObject.TryGetComponent(out Plate plate) && ob!=null)
        {
            if (plate.Food != null)
            {
                if (plate.Food.FoodGameObject != ob.FoodGameObject)
                    plate.AddFood(ob);
            }
            else
            {
                plate.AddFood(ob);
            }
        }
    }
    void Update()
    {
        if(isShowInfo)
        {   
            if(!isRecipeShow)
            {
                UIElements.GetInstance().ShowPanelRecipe();
                isRecipeShow = true;
            }
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
                        if (isShowContent)
                        {
                            var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();
                            if (list != null)
                            {
                                currentInfoObject.ShowContent(list);
                                playerController.canMove = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;

                            }
                        }
                        else
                        {
                            currentInfoObject.HideContent();
                            if (!playerController.canMove)
                            {
                                playerController.canMove = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;
                            }
                        }

                    }
                    else if (currentInfoObject != info)
                    {
                        currentInfoObject.SetOutline(false);
                        currentInfoObject.HideInfo();
                        if (isShowContent)
                        {
                            var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();
                            if (list != null)
                            {
                                currentInfoObject.ShowContent(list);
                                playerController.canMove = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;

                            }
                        }
                        else
                        {
                            currentInfoObject.HideContent();
                            if (!playerController.canMove)
                            {
                                playerController.canMove = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;
                            }
                        }
                        currentInfoObject = info;
                        currentInfoObject.SetOutline(true);
                        currentInfoObject.ShowInfo();
                        if (isShowContent)
                        {
                            var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();

                            if (list != null)
                            {
                                currentInfoObject.ShowContent(list);
                                playerController.canMove = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;
                            }
                        }
                        else
                        {
                            currentInfoObject.HideContent();
                            if (!playerController.canMove)
                            {
                                playerController.canMove = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;
                            }
                        }
                    }
                    else
                    {
                        currentInfoObject.ShowInfo();
                        if (isShowContent)
                        {
                            var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();
                            if (list != null)
                            {
                                if (currentInfoObject.IsContentShowed)
                                {
                                    currentInfoObject.UpdateContent(list);
                                }
                                else
                                {
                                    currentInfoObject.ShowContent(list);
                                    playerController.canMove = false;
                                    Cursor.lockState = CursorLockMode.None;
                                    Cursor.visible = true;
                                }
                            }
                        }
                        else
                        {
                            currentInfoObject.HideContent();
                            if (!playerController.canMove)
                            {
                                playerController.canMove = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;
                            }
                        }
                    }
                }
            }
            else if (currentInfoObject != null)
            {
                currentInfoObject.SetOutline(false);
                currentInfoObject.HideInfo();
                currentInfoObject.HideContent();
                if (!playerController.canMove)
                {
                    playerController.canMove = true;
                }
                currentInfoObject = null;
            }
        }
        else if (isRecipeShow)
        {
            UIElements.GetInstance().HidePanelRecipe();
            isRecipeShow = false;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit hit;
            if (!isShowResult && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
            {
                if (hit.collider.TryGetComponent(out PlateDish dish))
                {
                    if (dish.Compare(out string result))
                    {
                        UIElements.GetInstance().ShowPanelResult("Успешно!", result);
                    }
                    else
                    {
                        UIElements.GetInstance().ShowPanelResult("Неуспешно!", result);
                    }
                    isShowResult = true;
                    playerController.canMove = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else if(isShowResult)
            {
                UIElements.GetInstance().HidePanelResult();
                isShowResult = false;

                playerController.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isShowInfo = !isShowInfo;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
            {
                if(hit.collider.GetComponent<IListable>()!=null || hit.collider.GetComponentInChildren<IListable>() != null)
                    isShowContent = !isShowContent;
            }
        }
        if (CurrentDraggableObject!=null && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            draggableObjectDistance = Mathf.Clamp(draggableObjectDistance+Input.GetAxis("Mouse ScrollWheel")*Time.deltaTime*20f, minDraggableObjectDistance, maxDraggableObjectDistance);
        
        }
        if (CurrentDraggableObject != null)
        {
            CurrentDraggableObject.SetTargetPosition((cam.transform.position + cam.transform.forward * draggableObjectDistance) + offset);
        }
    }
    void TakeItem(CallbackContext context)
    {
        if (CurrentDraggableObject != null)
        {
            CurrentDraggableObject.StopFollowingObject();
            CurrentDraggableObject = null;
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
        {
            if (hit.collider.TryGetComponent(out DraggableObject draggableObject))
            {
                if (draggableObject.CanDrag)
                {
                    draggableObject.StartFollowingObject();
                    CurrentDraggableObject = draggableObject;
                }
            }
        }
    }
    void Interect(CallbackContext context)
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
                if (CurrentDraggableObject != null && CurrentDraggableObject.Type == inclose.DraggableType)
                {
                    if (!inclose.HasObject)
                    {
                        CurrentDraggableObject.CanDrag = false;
                        inclose.Put(CurrentDraggableObject);
                        CurrentDraggableObject.StopFollowingObject();
                        CurrentDraggableObject = null;
                    }
                }
                else if (inclose.HasObject)
                {
                    if (CurrentDraggableObject != null)
                    {
                        CurrentDraggableObject.StopFollowingObject();
                    }
                    var d = inclose.Pick();
                    d.CanDrag = true;
                    d.StartFollowingObject();
                    CurrentDraggableObject = d;
                }
            }
            else if (hit.transform.parent.TryGetComponent(out Inclose inclose2))
            {
                if (CurrentDraggableObject != null && CurrentDraggableObject.Type == inclose2.DraggableType)
                {
                    if (!inclose2.HasObject)
                    {
                        CurrentDraggableObject.CanDrag = false;
                        inclose2.Put(CurrentDraggableObject);
                        CurrentDraggableObject.StopFollowingObject();
                        CurrentDraggableObject = null;
                    }
                }
                else if (inclose2.HasObject)
                {
                    var d = inclose2.Pick();
                    d.CanDrag = true;
                    d.StartFollowingObject();
                    CurrentDraggableObject = d;
                }
            }
            else if (hit.collider.TryGetComponent(out Switcher switcher))
            {
                switcher.AddLevel();
            }
            else if (hit.collider.TryGetComponent(out Tap tap) && CurrentDraggableObject != null)
            {
                if (CurrentDraggableObject.TryGetComponent(out IHeated heated))
                {
                    if (!heated.HeatedInfo.HasWater)
                    {
                        tap.FillContainer(heated);
                    }
                }
            }
            else if (hit.collider.TryGetComponent(out CutBoard board))
            {
                if (CurrentDraggableObject.TryGetComponent(out Plate plate))
                {
                    board.CutObject(plate);
                }
            }
        }
    }
    void Pour(CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
        {
            if (hit.collider.tag == "Puttable" && CurrentDraggableObject != null)
            {
                if (CurrentDraggableObject.TryGetComponent(out IFood food))
                {
                    if (!food.IsPour && hit.collider.TryGetComponent(out Plate plate))
                    {
                        plate.AddFood(food);
                        CurrentDraggableObject.StopFollowingObject();
                        CurrentDraggableObject.transform.localScale = new Vector3(0, 0, 0);
                        CurrentDraggableObject = null;
                    }
                    else if (food.IsPour && hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            pot.PutToWater(food);
                        }
                    }
                    else if (food.IsPour && hit.collider.TryGetComponent(out FryingPan fryingPan))
                    {
                        fryingPan.AddFood(food);
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out Plate plate))
                {
                    if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            IFood food1;
                            plate.RemoveFood(out food1);
                            pot.PutToWater(food1);
                        }
                    }
                    else if (hit.collider.TryGetComponent(out FryingPan fryingPan))
                    {
                        IFood food1;
                        plate.RemoveFood(out food1);
                        fryingPan.AddFood(food1);
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out FryingPan fryingPan))
                {
                    if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            pot.PutToWater(fryingPan.Foods);
                            fryingPan.Foods = new List<IFood>();
                        }
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out PlateDish dish))
                {
                    if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            dish.AddDish(pot.GetFoods());
                        }
                    }
                }
            }

        }
    }
    public void UpdateSettings()
    {
        SettingsInit.InitControls(playerController.PlayerControls);
        playerController.LookSpeed = SettingsInit.GetSensetivity();
    }
    public void Escape()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            UIElements.GetInstance().ShowMenu();
            playerController.canMove = false;
        }
        else
        {
            UIElements.GetInstance().HideSettings();
            UIElements.GetInstance().HideMenu();
            playerController.canMove = true;
        }
    }
    private void OnDisable()
    {
        playerController.PlayerControls.Player.Disable();
        playerController.PlayerControls.Player.Take.performed -= TakeItem;
        playerController.PlayerControls.Player.Interect.performed -= Interect;
        playerController.PlayerControls.Player.Pour.performed -= Pour;
        playerController.PlayerControls.Player.Escape.performed -= delegate (CallbackContext context) { Escape(); };
    }
}