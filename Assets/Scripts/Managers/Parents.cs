using Unity.VisualScripting;
using UnityEngine;

public class Parents : MonoBehaviour
{
    public GameObject FoodParent { get; private set; }
    public GameObject ThingParent { get; private set; }
    public GameObject Player { get; private set; }

    private static Parents instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public static Parents GetInstance()
    {
        if(instance.Player == null || instance.FoodParent == null || instance.ThingParent == null)
        {
            instance.FoodParent = GameObject.Find("Food");
            instance.ThingParent = GameObject.Find("Thing");
            instance.Player = GameObject.Find("Player");
        }
        return instance;
    }
}
