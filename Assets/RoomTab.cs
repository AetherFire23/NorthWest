using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomTab : PrefabScriptBase, IEntity
{
    [SerializeField] public Button Button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    public PrivateChatRoom ChatRoom;
    public string Text
    {
        get { return _buttonText.text; }
        set { _buttonText.text = value; }
    }

    public Guid Id => ChatRoom.Id;

    public async UniTask Initialize(PrivateChatRoom chatRoom, Func<UniTask> roomTabAction)
    {
        Button.AddTaskFunc(roomTabAction);
        Text = chatRoom.ChatRoomName;
        ChatRoom = chatRoom;
        
    }
}
