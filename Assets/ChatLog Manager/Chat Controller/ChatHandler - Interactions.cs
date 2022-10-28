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
using Assets.GameState_Management;
using WebAPI.Models;

// Interactions veut dire que c'est les fonctions accédées par d'autres classes.
public partial class ChatHandler : ITickable, IInitializable 
{
    public void ChangeChatRoom(Guid roomId)  
    {
        //Clear
        this.CurrentRoomId = roomId;
        this.ClearChatlogC(); // messages
        _invitePanel.ClearPlayerEntries();

        //Post 
        this.PostMessagesInRoom(roomId);
        var playersInChangedRoom = GetPlayersInChatRoom(roomId);
        _invitePanel.InitializePlayerEntries(playersInChangedRoom);
    }

    public List<Player> GetPlayersInChatRoom(Guid roomId)
    {
        bool isGlobalId = _gameStateManager.LocalPlayerDTO.GameId == roomId;
        if(isGlobalId) return _gameStateManager.Players;
        
        // le WEb Api renvoie tous les PRivateRoomParticipants qui partagent une room avec le joueur. Mais là je filtre pour une room spécifique
        List<PrivateChatRoomParticipant> ParticipantsInSameRoom = _gameStateManager.ParticipantsInPlayerRooms.Where(room => room.RoomId == roomId).ToList(); //refaire avec where et select
        List<Guid> playersIdsInSelectedRoom = ParticipantsInSameRoom.Select(room => room.ParticipantId).ToList();
        List<Player> playerInSelectedRoom = _gameStateManager.Players.Where(player => playersIdsInSelectedRoom.Contains(player.Id)).ToList(); // nexclut pas le main player

        return playerInSelectedRoom;
    }
}