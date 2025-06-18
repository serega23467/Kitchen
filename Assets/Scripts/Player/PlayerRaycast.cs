using System.Collections.Generic;
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
    ShowObjectInfo currentInfoObject = null;
    ShowObjectInfo contentInfoObject = null;

    PlayerController playerController = null;
    Knife knife;
    bool isShowInfo = true;
    bool isShowContent = false;
    bool isShowResult = false;
    bool isRecipeShow = false;
    bool gameIsPaused = false;
    bool knifeInHand = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        knife = cam.GetComponentInChildren<Knife>();
        knife.gameObject.SetActive(false);
    }
    private void Start()
    {
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
        playerController.PlayerControls.Player.ShowRecipe.performed += ShowRecipe;
    }
    public bool PickFood(FoodComponent food)
    {
        if (CurrentDraggableObject == null) return false;
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
                return true;
            }
        }
        return false;
    }
    void Update()
    {
        if(isShowInfo)
        {
            RaycastHit infoHit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out infoHit, showInfoDistance, LayerMask.GetMask("DraggableObject") | LayerMask.GetMask("InteractiveObject"))
            &&  ComponentGetter.TryGetComponent(infoHit.collider, out ShowObjectInfo info))
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
            if (ComponentGetter.TryGetComponent(hit.collider, out DraggableObject draggableObject))
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
                if (CurrentDraggableObject != null)
                {
                    if (CurrentDraggableObject.Type == inclose.DraggableType && !inclose.HasObject)
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
                    else
                    {
                        UIElements.ShowToast("В кастрюле уже есть вода!");
                    }
                }
                else
                {
                    UIElements.ShowToast("Возьмите в руку кастрюлю для набора воды!");
                }
            }
            else if (hit.collider.TryGetComponent(out BellFinish bell))
            {
                bell.DingBell(Escape);
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
                if (CurrentDraggableObject.TryGetComponent(out FoodComponent food))
                {
                    if(food.FoodInfo.IsPour)
                    {
                        if(ComponentGetter.TryGetComponent(hit.collider, out Plate plate))
                        {
                            if(food.GetPour(out FoodComponent pour))
                            {
                                if (!plate.TryAddFood(pour))
                                {
                                    Destroy(pour.gameObject);
                                }
                            }
                        }
                        else if (ComponentGetter.TryGetComponent(hit.collider, out Pot pot))
                        {
                            if (pot.HeatedInfo.HasWater)
                            {
                                if (food.GetPour(out FoodComponent pour))
                                {
                                    pot.AddFood(pour);
                                }
                            }
                            else
                            {
                                UIElements.ShowToast("Наполните кастрюлю водой чтобы положить в неё что либо!");
                            }
                        }
                    }
                    else
                    {
                        if (ComponentGetter.TryGetComponent(hit.collider, out Plate plate))
                        {
                            if (plate.TryAddFood(food))
                            {
                                CurrentDraggableObject.StopFollowingObject();
                                CurrentDraggableObject.OffRigidbody();
                                CurrentDraggableObject.CanDrag = false;
                                CurrentDraggableObject = null;
                            }
                        }
                        else if (ComponentGetter.TryGetComponent(hit.collider, out Pot pot))
                        {
                            if (pot.HeatedInfo.HasWater)
                            {
                                pot.AddFood(food);
                                CurrentDraggableObject.StopFollowingObject();
                                CurrentDraggableObject = null;
                            }
                            else
                            {
                                UIElements.ShowToast("Наполните кастрюлю водой чтобы положить в неё что либо!");
                            }
                        }
                    }                 
                }
                else if (CurrentDraggableObject.TryGetComponent(out SpiceComponent spice))
                {
                    if (ComponentGetter.TryGetComponent(hit.collider, out Plate plate))
                    {
                        plate.SpiceFood(spice);
                    }
                    else if (ComponentGetter.TryGetComponent(hit.collider, out Pot pot))
                    {
                        pot.SpiceFood(spice);
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out Plate plate))
                {
                    if (ComponentGetter.TryGetComponent(hit.collider, out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            var foods = plate.MoveAllFood();
                            pot.AddFood(foods);
                        }
                        else
                        {
                            UIElements.ShowToast("Наполните кастрюлю водой чтобы положить в неё что либо!");
                        }
                    }
                    else if (ComponentGetter.TryGetComponent(hit.collider, out Plate plate2))
                    {
                        var foodList = new List<FoodComponent>(plate.Foods);
                        foreach (var f in foodList)
                        {
                            plate2.TryAddFood(f);
                        }    
                    }
                }
                else if (CurrentDraggableObject.TryGetComponent(out PlateDish dish))
                {
                    if (ComponentGetter.TryGetComponent(hit.collider, out Pot pot))
                    {
                        if (pot.HeatedInfo.HasWater)
                        {
                            var foods = pot.GetFoodsClone();
                            if(foods.Count > 0)
                                dish.AddDish(foods, pot.HeatedInfo.HasWater);
                        }
                        else
                        {
                            UIElements.ShowToast("В кастрюле нет воды, наполните!");
                        }
                    }
                }
            }
            else if (ComponentGetter.TryGetComponent(hit.collider, out FoodComponent food))
            {
                if(CurrentDraggableObject == null)
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
                            var plate = food.plate;
                            System.Action<bool> action =
                            result =>
                            {
                                if (result)
                                {
                                    plate.RemoveFood(food);
                                    Destroy(food.gameObject);
                                }
                                playerController.OffMenuMode();
                            };
                            playerController.OnMenuMode();
                            UIElements.GetInstance().OpenPanelConfirm($"Удаление продукта: {food.FoodInfo.FoodName}", action);
                        }
                    }
                }
                else if(CurrentDraggableObject.TryGetComponent(out SpiceComponent spice))
                {
                    spice.AddSpiceTo(food);
                }
            }

        }
    }
    public void UpdateSettings()
    {
        SettingsInit.InitControls(playerController.PlayerControls);
        playerController.LookSpeed = DB.GetSensetivity();
    }
    public void Escape()
    {
        if (SceneLoader.Instance.gameObject.activeSelf) return;

        if(UIElements.GetInstance().TryEscape())
        {
            gameIsPaused = !gameIsPaused;
            if (gameIsPaused)
            {
                UIElements.GetInstance().ShowMenu();
                playerController.OnMenuMode();
            }
            else
            {
                UIElements.GetInstance().HideSettings();
                UIElements.GetInstance().HideMenu();
                playerController.OffMenuMode();
            }
        }
        else if (!gameIsPaused)
        {
            playerController.OffMenuMode();
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
            knife.TakeKnife();
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
                if (ComponentGetter.TryGetComponent(hit.collider, out FoodComponent food))
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
        if(!UIElements.GetInstance().IsContentShowed())
        {
            contentInfoObject = null;
            isShowContent = false;
        }
        if (contentInfoObject!=null)
        {
            isShowContent = false;
            contentInfoObject.HideContent();
            contentInfoObject = null;
            playerController.OffMenuMode();
        }
        else
        {
            if (currentInfoObject == null || !currentInfoObject.CanShowContent) return;
            if (!isShowContent)
            {
                var list = currentInfoObject.GetComponentInChildren<IListable>() ?? currentInfoObject.transform.GetComponent<IListable>();
                if (list != null && list.Foods.Count > 0)
                {
                    isShowContent = true;
                    bool hasPlate = CurrentDraggableObject == null ? false : CurrentDraggableObject.TryGetComponent(out Plate plate);
                    currentInfoObject.ShowContent(list, hasPlate);
                    contentInfoObject = currentInfoObject;
                    playerController.OnMenuMode();
                    if (!hasPlate && list.CanPull) UIElements.ShowToast("Чтобы вытащить продукт в руках должна быть тарелка!");
                }
            }
            else
            {
                isShowContent = false;
                currentInfoObject.HideContent();
                contentInfoObject = null;
                playerController.OffMenuMode();
            }
        }

    }
    public void ShowRecipe(CallbackContext context)
    {
        isRecipeShow = !isRecipeShow;
        if(isRecipeShow)
        {
            UIElements.GetInstance().ShowRecipePanel();
        }
        else
        {
            UIElements.GetInstance().HideRecipePanel();
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
        playerController.PlayerControls.Player.ShowRecipe.performed -= ShowRecipe;
    }
}