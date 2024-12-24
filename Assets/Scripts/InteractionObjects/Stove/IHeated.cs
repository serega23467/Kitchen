using UnityEngine;

public interface IHeated
{
    public HeatedInfo HeatedInfo { get; set; }
    public void AddWater();
    public void OnBoiling(byte level);
    public void StopHeating();
    public void StartHeating();
}
