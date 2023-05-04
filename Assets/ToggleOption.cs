using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOption : PrefabScriptBase
{
    [SerializeField] private UnityEngine.UI.Toggle _toggle;
    [SerializeField] private Text _text;
    public bool Toggled
    {
        get { return _toggle.isOn; }
        set { _toggle.isOn = value; }
    }

    public object Option { get; private set; }
    public async UniTask Initialize(object option)
    {
        Option = option;
        _text.text = Option.ToString();
    }
}
