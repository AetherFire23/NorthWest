using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.ChatLog_Manager.Private_Rooms;
using Assets.ChatLog_Manager;
using Assets;
using WebAPI.Models;
using Assets.GameState_Management;
using Assets.InputAwaiter;

public class PrivateRoomInviteHandler : ITickable, IInitializable
{
    private readonly ClientCalls _clientCalls;
    private readonly DialogManager _dialogManager;

    private SimpleTimer _timer = new SimpleTimer(3);

    private readonly GameStateManager _gameStateManager;
    private readonly InputWaiting _inputWaiting;
    public PrivateRoomInviteHandler(ClientCalls clientCalls,
        DialogManager dialogManager,
        GameStateManager gameStateManager,
        InputWaiting inputWaiting)
    {
        _clientCalls = clientCalls;
        _dialogManager = dialogManager;
        _gameStateManager = gameStateManager;
        _inputWaiting = inputWaiting;
    }

    public void Initialize()
    {

    }

    public async void Tick()
    {
        // tout ca ici ca check linvitation et ca attend la reponse 

        if (_timer.HasTicked) // FAit le client call au lieu de le get dans le gamestate manager.
        {
            if (_dialogManager.IsWaitingForInput()) return; // fait quoi si jenleve ca ?
            if (_gameStateManager.PrivateInvitations.Any())
            {
                var invite = _gameStateManager.PrivateInvitations.First();

                Debug.Log($"La room en question a qui jinvite {invite.RoomId}");

                _dialogManager.CreateDialog(DialogType.YesNoDialog, $"You have been invited by {invite.FromPlayerName}. Do you accept ?");

                await _inputWaiting.WaitForResult();

                bool invitationIsAccepted = _dialogManager.CurrentDialog.DialogResult == DialogResult.Ok; // ici un bug mais ca marche haha

                invite.IsAccepted = true; // depends on paramater
                invite.RequestFulfilled = true; // always true when this is called

                _clientCalls.SendInvitationResponse(invite);

                _dialogManager.CurrentDialog = null;

                _timer.Reset();
            }
        }
        _timer.AddTime(Time.deltaTime);
    }
}