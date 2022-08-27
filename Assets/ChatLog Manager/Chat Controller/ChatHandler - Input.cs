using Assets;
using Assets.ChatLog_Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using Cysharp.Threading.Tasks;
using Assets.ChatLog_Manager.Chat_Controller;
using Assets.ChatLog_Manager.Private_Rooms.ChatModels;
using Assets.GameState_Management.Models;
using Assets.GameState_Management;

public partial class ChatHandler : ITickable, IInitializable
{
    private MessageResult GetInputMessageResult()
    {
        string rawInputFieldText = _ChatObject.GetMessageFromInputField();
        return new MessageResult(rawInputFieldText);
    }
}