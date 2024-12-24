using UnityEngine;

public interface IHeated
{
    public float Temperature {  get; set; }
    public byte MassKG{ get; set; }
    public void StopHeating();
    public void StartHeating();
}
