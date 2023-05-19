//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Cysharp.Threading.Tasks;
//using UnityEngine.UI;
//using static UnityEngine.UI.Button;
//using Assets.Utils;
//using Shared_Resources.Interfaces;

//public class RoomTabUGI : InstanceWrapper<RoomTabInstanceScript>, IEntity
//{
//    public Guid Id => RoomId;
//    public Button ButtonComponent => this.AccessScript.button;
//    public ButtonClickedEvent OnClick => this.ButtonComponent.onClick;
//    public Guid RoomId { get; set; } = Guid.Empty;

//    private const string resourceName = "RoomTabPrefab";

//    public RoomTabUGI(GameObject parent, Guid privateRoomId, int amount) : base(resourceName, parent)
//    {
//        this.RoomId = privateRoomId;
//        this.AccessScript.buttonText.text = $"Room {amount}";
//    }
//}