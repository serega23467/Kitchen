using UnityEngine;

[RequireComponent(typeof(ShakingAnim))]
public class SpiceComponent : MonoBehaviour
{
    [SerializeField]
    string spiceName;
    [SerializeField]
    float spiceDozenGramsWeight;
    ShakingAnim anim;
    void Start()
    {
        anim = GetComponent<ShakingAnim>();
        GetComponent<ShowObjectInfo>().ObjectData = $"{spiceDozenGramsWeight} ã";
    }
    public void AddSpiceTo(FoodComponent food)
    {
        food.TryAddParameterValue(spiceName, spiceDozenGramsWeight);
        anim?.Shake();
    }
}
