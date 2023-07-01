using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MainMenu.Authentication
{
    public class ObservableBool // not sure if this is actually async // should put in util cause this is fucking great
    {
        private bool _value;

        public delegate UniTask ValueChangedHandler(bool newValue);
        public event ValueChangedHandler OnValueChanged;

        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    bool oldValue = _value;
                    _value = value;
                    OnValueChanged?.Invoke(value);
                }
            }
        }

        public ObservableBool(bool value)
        {
            _value = value;
        }
    }
}
