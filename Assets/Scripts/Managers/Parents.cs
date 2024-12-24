using Unity.VisualScripting;
using UnityEngine;

public class Parents
{
    public GameObject FoodParent { get; private set; }
    public GameObject ThingParent { get; private set; }

    private static Parents instance;
    private Parents() { }
    public static Parents GetInstance()
    {
        if (instance == null)
        {
            instance = new Parents();
            instance.FoodParent = GameObject.Find("Food");
            instance.ThingParent = GameObject.Find("Thing");
        }
        return instance;
    }
}
