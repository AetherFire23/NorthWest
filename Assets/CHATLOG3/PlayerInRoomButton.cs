using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInRoomButton : PrefabScriptBase, IEntity // pourrais faire un Initialize sur le prefab I guess
{ 
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI _buttonTextMesh;
    public string RoomText
    {
        get { return _buttonTextMesh.text; }
        set { _buttonTextMesh.text = value; }
    }
    public Guid Id => _player.Id;
    private Player _player;

    public async UniTask Initialize(Player player)
    {
        _player = player;
        RoomText = _player.Name;
    }
}
