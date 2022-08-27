using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class RoomTabInstance : InstanceWrapper<RoomTabInstanceScript>
{
    public Button ButtonComponent => this.InstanceBehaviour.button;
    public ButtonClickedEvent OnClick => this.ButtonComponent.onClick;
    public Guid RoomId { get; set; } = Guid.Empty;

    private const string resourceName = "RoomTabPrefab";

    public RoomTabInstance(GameObject parent, Guid privateRoomId, int amount) : base(resourceName, parent)
    {
        this.RoomId = privateRoomId;
        this.InstanceBehaviour.buttonText.text = $"Room {amount}";
    }
}