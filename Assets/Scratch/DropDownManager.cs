using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using static TMPro.TMP_Dropdown;

namespace Assets.Scratch
{
    public class DropDownManager<T> where T : class, IFormattable // should implement this for ToggleOptions honestly
    {
        private Dictionary<OptionData, T> _options = new Dictionary<OptionData, T>();
        private TMP_Dropdown _dropdown;

        public List<T> Options => _options.Values.ToList();
        public DropDownManager(List<T> options, TMP_Dropdown down)
        {
            _dropdown = down;
            MapOptionsToOptionData(options);
        }

        private void MapOptionsToOptionData(List<T> options)
        {
            if(options.Any(x=> x is null)) throw new ArgumentNullException("at least 1 argument was null inside options");
            foreach (var option in options)
            {
                var optionData = new OptionData(option.ToString());
                _options.Add(optionData, option);
            }

            _dropdown.AddOptions(_options.Keys.ToList());
        }

        public T GetValue(OptionData option)
        {
            if (!_options.ContainsKey(option))
            {
                throw new ArgumentNullException($"The following key must be implement in the dropdown: {option.text}");
            }

            var value = _options[option];
            return value;
        }

        public T GetSelectedValue()
        {
            int selectedIndex = _dropdown.value;
            var selectedOption = _dropdown.options[selectedIndex];
            var selectedObject = this.GetValue(selectedOption);
            return selectedObject;
        }
    }
}
