using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.PostProcessing;
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
    ShowObjectInfo currentInfoObject = null;
    PlayerController playerController = null;
    Knife knife;
    bool isShowInfo = true;
    bool isShowContent = false;
    bool isShowResult = false;
    bool isRecipeShow = true;
    bool gameIsPaused = false;
    bool knifeInHand = false;
    //bool isInEntiractive = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        knife = cam.GetComponentInChildren<Knife>();
        knife.gameObject.SetActive(false);
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
        playerController.PlayerControls.Player.TakeKnife.performed += TakeKnife;
        playerController.PlayerControls.Player.Cut.performed += Cut;
        playerController.PlayerControls.Player.ShowInfo.performed += ChangeInfoVisibility;
    }
    public void PickFood(FoodComponent food)
    {
        if (CurrentDraggableObject == null) return;
        if (CurrentDraggableObject.TryGetComponent(out Plate plate) && food != null)
        {
            if(plate.TryAddFood(food))
            {
                food.gameObject.transform.localScale = Vector3.one;
                var dgo = food.GetComponent<DraggableObject>();
                if(dgo!=null)
                {
                    dgo.OffRigidbody();
                    dgo.CanDrag = false;
                }
            }
        }
    }
    void Update()
    {
        if(isShowInfo)
        {
            RaycastHit infoHit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out infoHit, showInfoDistance, LayerMask.GetMask("DraggableObject") | LayerMask.GetMask("InteractiveObject"))
            && infoHit.collider.TryGetComponent(out ShowObjectInfo info))
            {
                if (currentInfoObject == null)
                {
                    currentInfoObject = info;
                    currentInfoObject.SetOutline(true);
                    currentInfoObject.ShowInfo();
                }
                else if (currentInfoObject != info)
                {
                    currentInfoObject.HideInfo();
                    currentInfoObject.SetOutline(false);

                    currentInfoObject = info;
                    currentInfoObject.SetOutline(true);
                    currentInfoObject.ShowInfo();
                }
                else
                {
                    currentInfoObject.ShowInfo();
                }
            }
            else if (currentInfoObject != null)
            {
                currentInfoObject.HideInfo();
                currentInfoObject.SetOutline(false);
                currentInfoObject = null;
            }
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
                        UIElements.GetInstance().ShowPanelResult("”ÒÔÂ¯ÌÓ!", result);
                    }
                    else
                    {
                        UIElements.GetInstance().ShowPanelResult("ÕÂÛÒÔÂ¯ÌÓ!", result);
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
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    isShowInfo = !isShowInfo;
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
        //    {
        //        if (hit.collider.GetComponent<IListable>() != null || hit.collider.GetComponentInChildren<IListable>() != null)
        //            isShowContent = !isShowContent;
        //    }
        //}
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
            //else if (hit.collider.TryGetComponent(out CutBoard board))
            //{
            //    if (CurrentDraggableObject.TryGetComponent(out Plate plate))
            //    {
            //        board.CutObject(plate);
            //    }
            //}
        }
    }
    void Pour(CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
        {
            if (hit.collider.tag == "Puttable" && CurrentDraggableObject != null)
            {
                if (CurrentDraggableObject.TryGetComponent(out FoodComponent food))
                {
                    if(food.FoodInfo.IsPour)
                    {
                        if(hit.collider.TryGetComponent(out Plate plate))
                        {
                            if(food.GetPour(out FoodComponent pour))
                            {
                                if (!plate.TryAddFood(pour))
                                {
                                    Destroy(pour.gameObject);
                                }
                            }
                        }
                        else if (hit.collider.TryGetComponent(out Pot pot))
                        {
                            if (pot.HeatedInfo.HasWater)
                            {
                                if (food.GetPour(out FoodComponent pour))
                                {
                                    pot.PutToWater(pour);
                                    pour.OnPull.RemoveAllListeners();
                                    pour.OnPull.AddListener(PickFood);
                                    pour.OnPull.AddListener(pot.OnRemoveFromWater.Invoke);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (hit.collider.TryGetComponent(out Plate plate))
                        {
                            if (plate.TryAddFood(food))
                            {
                                CurrentDraggableObject.StopFollowingObject();
                                CurrentDraggableObject.OffRigidbody();
                                CurrentDraggableObject.CanDrag = false;
                                CurrentDraggableObject = null;
                            }
                        }
                        else if (hit.collider.TryGetComponent(out Pot pot))
                        {
                            if (pot.HeatedInfo.HasWater)
                            {
                                pot.PutToWater(food);
                                food.OnPull.RemoveAllListeners();
                                food.OnPull.AddListener(PickFood);
                                food.OnPull.AddListener(pot.OnRemoveFromWater.Invoke);
                                CurrentDraggableObject.StopFollowingObject();
                                CurrentDraggableObject = null;
                            }
                        }
                    }                 
                }
                else if (CurrentDraggableObject.TryGetComponent(out SpiceComponent spice))
                {
                    if (hit.collider.TryGetComponent(out Plate plate))
                    {
                        plate.SpiceFood(spice);
                    }
                    else if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        pot.SpiceFood(spice);
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out Plate plate))
                {
                    if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            var foods = plate.MoveAllFood();
                            foreach(var f in foods)
                            {
                                f.OnPull.RemoveAllListeners();
                                f.OnPull.AddListener(PickFood);
                                f.OnPull.AddListener(pot.OnRemoveFromWater.Invoke);
                            }
                            pot.PutToWater(foods);
                        }
                    }
                    else if (hit.collider.TryGetComponent(out Plate plate2))
                    {
                        var foodList = new List<FoodComponent>(plate.GetFoodList());
                        foreach (var f in foodList)
                        {
                            plate2.TryAddFood(f);
                        }    
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out PlateDish dish))
                {
                    if (hit.collider.TryGetComponent(out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            //dish.AddDish(pot.GetFoods());
                        }
                    }
                }
            }
            else if (CurrentDraggableObject == null && hit.collider.TryGetComponent(out FoodComponent food))
            {
                if (food.plate != null)
                {
                    if (!food.FoodInfo.IsPour)
                    {
                        var plate = food.plate;
                        plate.RemoveFood(food);

                        food.transform.parent = Parents.GetInstance().FoodParent.transform;

                        var dgo = food.GetComponent<DraggableObject>();
                        dgo.CanDrag = true;
                        dgo.gameObject.transform.position += new Vector3(0, 0.05f, 0);
                        dgo.OnRigidbody();
                        dgo.StartFollowingObject();
                        CurrentDraggableObject = dgo;
                    }
                    else
                    {
                        //Ã≈Õﬁ ¬€—€œ¿Õ»ﬂ À»ÿÕ≈√Œ
                    }
                }

            }

        }
    }
    public void UpdateSettings()
    {
        SettingsInit.InitControls(playerController.PlayerControls);
        playerController.LookSpeed = SettingsInit.GetSensetivity();
        SettingsInit.UpdateVirtualSecond();
        SettingsInit.InitVideo();
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
    void TakeKnife(CallbackContext context)
    {
        if(knifeInHand)
        {
            knife.gameObject.SetActive(false);
            knifeInHand = false;
        }
        else
        {
            knife.gameObject.SetActive(true);
            knifeInHand = true;
        }
    }
    void Cut(CallbackContext context)
    {
        if (knifeInHand)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDistance, LayerMask.GetMask("DraggableObject")))
            {
                if (hit.collider.TryGetComponent(out FoodComponent food))
                {
                    if (CurrentDraggableObject != null)
                    {
                        if(CurrentDraggableObject.gameObject == hit.collider.gameObject)
                        {
                            CurrentDraggableObject.StopFollowingObject();
                            CurrentDraggableObject = null;
                        }
                    }
                    knife.Cut(food);
                }
            }
        }
    }
    public void ChangeInfoVisibility(CallbackContext context)
    {
        if (currentInfoObject == null) return;
        if(!isShowContent)
        {
            var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();
            if (list != null && list.Foods.Count>0)
            {
                isShowContent = true;
                bool hasPlate = CurrentDraggableObject == null ? false : CurrentDraggableObject.TryGetComponent(out Plate plate);
                currentInfoObject.ShowContent(list, hasPlate);
                playerController.OnMenuMode();
            }
        }
        else
        {
            isShowContent = false;
            currentInfoObject.HideContent();
            playerController.OffMenuMode();
        }
    }
    private void OnDisable()
    {
        playerController.PlayerControls.Player.Disable();
        playerController.PlayerControls.Player.Take.performed -= TakeItem;
        playerController.PlayerControls.Player.Interect.performed -= Interect;
        playerController.PlayerControls.Player.Pour.performed -= Pour;
        playerController.PlayerControls.Player.Escape.performed -= delegate (CallbackContext context) { Escape(); };
        playerController.PlayerControls.Player.TakeKnife.performed -= TakeKnife;
        playerController.PlayerControls.Player.Cut.performed -= Cut;
        playerController.PlayerControls.Player.ShowInfo.performed -= ChangeInfoVisibility;
    }
}