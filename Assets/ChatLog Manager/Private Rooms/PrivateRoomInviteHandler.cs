using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.ChatLog_Manager.Private_Rooms;
using Assets.ChatLog_Manager.Private_Rooms.PlayerOptionsPanel;
using Assets.ChatLog_Manager;
using Assets;

public class PrivateRoomInviteHandler : ITickable, IInitializable
{
    public List<PlayerModel> currentPlayers;
    public List<PlayerInviteEntryInstance> playerEntries = new List<PlayerInviteEntryInstance>();
    private readonly ClientCalls _clientCalls;
    private readonly DialogManager _dialogManager;
    private readonly MainPlayer _mainPlayer;

    private SimpleTimer _timer = new SimpleTimer(3);

    

    public PrivateRoomInviteHandler(ClientCalls clientCalls,
        DialogManager dialogManager,
        RoomTabBar roomTabsContainer,
        InvitePanel invitePanel,
        ChatCanvasWrapper chatCanvasObject,
        MainPlayer mainPlayer)
    {
        _clientCalls = clientCalls;
        _dialogManager = dialogManager;
        _mainPlayer = mainPlayer;
    }

    public void Initialize()
    {
        Debug.Log("I initialized requestController");
    }

    public async void Tick()
    {
        // tout ca ici ca check linvitation et ca attend la reponse 

        if (_timer.HasTicked) 
        {
            if (_dialogManager.IsWaitingForInput()) return; 

            PrivateInvitation invite = _clientCalls.GetPendingInvitation(_mainPlayer.playerModel.Id).AsTask().Result;
            InviteResult result = new InviteResult(invite); // pourra etre delete a casue du null dans le web api, si present == invited on le sait 
            Debug.Log(result.IsInvited);

            if (result.IsInvited) // if invite != null 
            {
                _dialogManager.CreateDialog(DialogType.YesNoDialog, $"You have been invited by {result.InviterName}. Do you accept ?");

                await IsWaitingForDialogResult();

                bool invitationIsAccepted = _dialogManager.CurrentDialog.DialogResult == DialogResult.Ok;

                PrivateInvitation responseInvitation = result.FulfillRequest(invitationIsAccepted);

                _clientCalls.SendInvitationResponse(responseInvitation);

                _dialogManager.CurrentDialog = null;
            }
            _timer.Reset();
        }
        _timer.AddTime(Time.deltaTime);
    }


    public InviteResult GetInviteResult() // considerer faire un InvitationService?
    {
        PrivateInvitation invite = _clientCalls.GetPendingInvitation(_mainPlayer.playerModel.Id).AsTask().Result;
        InviteResult result = new InviteResult(invite); // logiuqe a linterieur du constructeur de invite 
        return result;
    }

    public async UniTask WaitForKeyPress(KeyCode keycode) // comment faire ca clean ?
    {
        bool hasPressedKey = Input.GetKey(keycode);

        Func<KeyCode, bool> input = Input.GetKey; // why ?
        while (input(keycode) == false)
        {
            Debug.Log($"{nameof(input)}");
            await UniTask.Yield();
        }
    }
 // await _inputManager.WaitForDialogInput()
    public async UniTask IsWaitingForDialogResult() // comment faire ca clean ? // faire un singleton InputManager
    {
        bool isWaiting = true;
        while (isWaiting)
        {
            isWaiting = _dialogManager.IsWaitingForInput();

            Debug.Log($"State of waiting for input : {isWaiting}");
            //await UniTask.Delay(100); // faudrait tester avec du delay 
            await UniTask.Yield();
        }
    }
}

