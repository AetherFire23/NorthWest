using Assets.Big_Tick_Energy;
using Assets.Dialogs;
using Assets.Enums;
using Assets.GameState_Management;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Newtonsoft.Json;

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
            var obj = trigger.ExtraProperties.ToString();
            var invite = JsonConvert.DeserializeObject<PrivateInvitationProperties>(obj);

            var da = new DialogAndAction()
            {
                Message = $"You have been invited by {invite.FromPlayerName}",
                DialogType = DialogType.YesNoDialog,
                ActionType = () =>
                {
                    Debug.Log("Sent invitation");

                    bool response = _dialogs.IsOkResult;

                    _client.Chat.SendInvitationResponse(trigger.Id, response);

                },
            };

            // dan le fond ca spawn twice. Je pense yavait le same probleme avant...
            // jai limpression que le gamestate est update apres le tick et que ca fait en sorte que la boucle run 2 fois dans les memes notifications.
            // au pire je deleterais la notification dans le gamestate ,ou je forcerais le tick 
           
            if (_dialogs.DialogQueue.Select(x => x.TriggerNotification).Contains(da.TriggerNotification))
            {
                return;
            }

            _dialogs.DialogQueue.Enqueue(da);
        }

    }
}
