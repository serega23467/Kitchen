using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SettingValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int BindingIndex { get; set; }
        public UnityEvent<int> OnValueChange { get; private set; } = new UnityEvent<int>();

        public override bool Equals(object obj)
        {
            SettingValue other = obj as SettingValue;
            if (other == null)
            {
                return false;
            }
            return (Id == other.Id && Name==other.Name && Value == other.Value);
        }
    }
}
