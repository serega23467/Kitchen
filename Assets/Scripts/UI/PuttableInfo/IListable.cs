using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface IListable
{
    ObservableCollection<FoodComponent> Foods { get; set; }
    bool CanPull { get; }
    public void PutFood(FoodComponent food);
}
