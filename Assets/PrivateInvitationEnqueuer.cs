using Assets.Big_Tick_Energy;
using Assets.Dialogs;
using Assets.Enums;
using Assets.GameState_Management;
using Assets.HttpClient.Shared_API_Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PrivateInvitationEnqueuer : MonoBehaviour
{
    [Inject] GlobalTick _tick;
    [Inject] DialogManager _dialogs;
    [Inject] GameStateManager _gameState;
    [Inject] ClientCalls _client;

    private NotificationType _notificationType = NotificationType.PrivInv;

    void Start()
    {
        QueuePrivateInvitations();
        _tick.TimerTicked += OnTimerTick;
    }

    private void OnTimerTick(object source, EventArgs e)
    {
        QueuePrivateInvitations();
    }

    private void QueuePrivateInvitations()
    {
        var triggers = _gameState.GetNotifications(_notificationType);
        if (triggers.Count == 0) return;

        int i = 0;

        foreach (var trigger in triggers)
        {
            //var sex = (Ptrigger.ExtraProperties;
           var invite = (PrivateInvitationNotification)trigger.ExtraProperties;// bug sur ste ligne 

            var da = new DialogAndAction()
            {
                Message = $"You have been invited by {invite.FromPlayerName}",
                DialogType = DialogType.YesNoDialog,
                ActionType = () =>
                {
                    Debug.Log("Sent invitation");
                    if (!_dialogs.IsOkResult) return;

                    _client.Chat.SendInvitationResponse(trigger.Id, true);
                },
            };
        }
    }
}
