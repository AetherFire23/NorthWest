//using Assets.Big_Tick_Energy;
//using Assets.Dialogs;
//using Assets.Enums;
//using Assets.GameState_Management;
//using Shared_Resources.Enums;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Zenject;

//public class CycleChangeEnqueuer : MonoBehaviour
//{
//    [Inject] GlobalTick _tick;
//    [Inject] DialogManager _dialogs;
//    [Inject] GameStateManager _gameState;

//    void Start()
//    {
//        QueueCycleChanges();
//        _tick.TimerTicked += OnTimerTick;
//    }

//    private void OnTimerTick(object source, EventArgs e)
//    {
//        QueueCycleChanges();
//    }

//    private void QueueCycleChanges()
//    {
//        var triggers = _gameState.Notifications.Where(x => x.NotificationType == NotificationType.CycleChanged).ToList();

//        foreach (var trigger in triggers)
//        {
//            var da = new DialogAndAction()
//            {
//                TriggerNotification = trigger,  
//                ActionType = () => Debug.Log("Just informed someone!"),
//                DialogType = DialogType.MessageBox,
//                Message = "I am here to inform you that a cycle has changed.",
//            };



//                _dialogs.DialogQueue.Enqueue(da);

//            // add fenetre
//        }
//    }
//}
