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
        info.ObjectInfo = "������� '����������� � �������' �� ������ ��� ������� ����� ��������";
        info.ObjectData = $"{spiceDozenGramsWeight} � �� ���";
    }
    public void AddSpiceTo(FoodComponent food)
    {
        food.TryAddParameterValue(spiceName, spiceDozenGramsWeight);
        anim?.Shake();
    }
}
