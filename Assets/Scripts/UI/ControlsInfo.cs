using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ControlsInfo
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }
        public UnityEvent<int> OnChangeKey { get; private set; } = new UnityEvent<int>();

        public override bool Equals(object obj)
        {
            ControlsInfo other = obj as ControlsInfo;
            if (other == null)
            {
                return false;
            }
            return (Id == other.Id && Action==other.Action && Key == other.Key);
        }
    }
}
