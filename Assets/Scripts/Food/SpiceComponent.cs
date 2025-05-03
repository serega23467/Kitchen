using UnityEngine;

public class SpiceComponent : MonoBehaviour
{
    [SerializeField]
    string spiceName;
    [SerializeField]
    float spiceDozenGramsWeight;
    void Start()
    {
        GetComponent<ShowObjectInfo>().ObjectData = $"{spiceDozenGramsWeight} �";
    }
    public void AddSpiceTo(FoodComponent food)
    {
        food.TryAddParameterValue(spiceName, spiceDozenGramsWeight);
    }
}
