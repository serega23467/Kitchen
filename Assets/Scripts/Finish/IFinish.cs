using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface IFinish
{
    public bool HasWater { get; set; }
    public bool CanFinish { get; }
    public Recipe GetRecipe();
    public void SetFinishOutline(bool hasOutline);
}
