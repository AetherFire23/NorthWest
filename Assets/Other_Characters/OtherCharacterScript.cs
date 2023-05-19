using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherCharacterScript : PrefabScriptBase, IEntity
{
    [SerializeField] public TextMeshProUGUI _textOverHead;
    [SerializeField] public TextMeshProUGUI _professionText;
    public Guid Id => _player.Id;
    private Player _player;
    // Start is called before the first frame update

    public async UniTask Initialize(Player player)
    {
        _player = player;
        _textOverHead.text = player.Name;
        _professionText.name = player.Profession.ToString();
    }
}
