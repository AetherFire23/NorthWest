using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInRoomButton : PrefabScriptBase, IEntity // pourrais faire un Initialize sur le prefab I guess
{ // et throw si ca marche pas 
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    public Guid Id => _player.Id;
    private Player _player;
    public string Text
    {
        get
        {
            return _buttonText.text;
        }

        set
        {
            _buttonText.text = value;
        }
    }

    public async UniTask Initialize(Player player)
    {
        _player = player;
        Text = _player.Name;
    }
}
