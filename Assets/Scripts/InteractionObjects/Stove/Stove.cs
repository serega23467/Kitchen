using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField]
    Switcher[] switchers;
    [SerializeField]
    Inclose[] places;
    
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            switchers[i].Id = (byte)i;
            places[i].Id = (byte)i;
            switchers[i].OnSwitched.AddListener(EditFire);
            places[i].OnPickPut.AddListener(EditIncome);
        }
    }
    void EditIncome(byte id, bool hasObject)
    {
        if (switchers[id].Level == 0)
        {
            return;
        }
        if (hasObject)
        {
            StoveFire fire = places[id].gameObject.AddComponent<StoveFire>();
            fire.StartFire(places[id].PickedObject.GetComponent<IHeated>(), switchers[id].Level);
            places[id].GetComponent<FireControl>().ChangeFire(0);
        }
        else
        {
            places[id].GetComponent<FireControl>().ChangeFire(switchers[id].Level);
            if (places[id].gameObject.TryGetComponent<StoveFire>(out StoveFire fire))
            {
                Destroy(fire);
                places[id].PickedObject.GetComponent<IHeated>().StopHeating();
            }
        }
    }
    void EditFire(byte id, byte level)
    {
        if (places[id].gameObject.TryGetComponent<StoveFire>(out StoveFire fire))
        {
            Destroy(fire);
            places[id].PickedObject.GetComponent<IHeated>().StopHeating();
        }
        if (switchers[id].Level != 0)
        {
            places[id].GetComponent<FireControl>().ChangeFire(level);
            if (places[id].HasObject)
            {
                StoveFire newFire = places[id].gameObject.AddComponent<StoveFire>();
                newFire.StartFire(places[id].PickedObject.GetComponent<IHeated>(), switchers[id].Level);
                places[id].GetComponent<FireControl>().ChangeFire(0);
            }
        }
        else
        {
            places[id].GetComponent<FireControl>().ChangeFire(0);
        }
        
    }
}
