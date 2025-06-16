using UnityEngine;

[RequireComponent(typeof(ShakingAnim))]
public class SpiceComponent : MonoBehaviour
{
    [SerializeField]
    string spiceName;
    [SerializeField]
    float spiceDozenGramsWeight;
    ShakingAnim anim;
    ShowObjectInfo info;
    void Awake()
    {
        info = GetComponent<ShowObjectInfo>();
        anim = GetComponent<ShakingAnim>();
        info.ObjectInfo = "Нажмите 'Переместить в ёмкость' на посуду или продукт чтобы добавить";
        info.ObjectData = $"{spiceDozenGramsWeight} г за раз";
    }
    public void AddSpiceTo(FoodComponent food)
    {
        food.TryAddParameterValue(spiceName, spiceDozenGramsWeight);
        anim?.Shake();
    }
}
