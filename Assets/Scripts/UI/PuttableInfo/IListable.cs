using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface IListable
{
    public void PutFood(FoodComponent food);
    ObservableCollection<FoodComponent> Foods { get; set; }
    bool CanPull { get; }
}
