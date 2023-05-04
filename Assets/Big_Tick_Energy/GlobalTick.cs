//using Assets.GameState_Management;
//using Assets.Interfaces;
//using Cysharp.Threading.Tasks;
//using Shared_Resources.GameTasks;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Zenject;

//namespace Assets.Big_Tick_Energy
//{
//    public class GlobalTick : ITickable
//    {
//        public List<string> SubscribedMembers { get; set; } = new();

//        public delegate void TimerTickedEventHandler(object source, EventArgs args);
//        public event TimerTickedEventHandler TimerTicked;

//        private float _maximumTime = 5f;
//        private float _currentTimeElapsed;
//        private int _tickAmount;

//        [Inject] private DiContainer _container;
//        [Inject] private GameStateManager _gameState;


//        public async void Tick() // From Interface
//        {
//            _currentTimeElapsed += Time.deltaTime;
//            bool thresholdReached = _currentTimeElapsed > _maximumTime;

//            if (thresholdReached)
//            {
//                _currentTimeElapsed = 0;

//                //OnTimerTick(); // MIGHT BREAk IF SO RETURN TO THIS
//                _tickAmount++;
//                await UniTask.RunOnThreadPool(_gameState.GetNextGameState);
//                await ExecuteDelegateOncePerFrame();
//                Debug.Log($"Timer has ticked for {_tickAmount} times. Subscribed members : {String.Join(',', this.SubscribedMembers)}");
//            }

//        }

//        private async Task OnTimerTick()
//        {
//            if (TimerTicked is null) return;

//             TimerTicked(this, EventArgs.Empty);

//            Debug.Log($"Timer has ticked for {_tickAmount} times. Subscribed members : {String.Join(',', this.SubscribedMembers)}");
//            this.SubscribedMembers.Clear();
//        }

//        private async UniTask ExecuteDelegateOncePerFrame()
//        {
//            int count = 0;
//            var invocationList = this.TimerTicked.GetInvocationList();

//            while (count < invocationList.Length)
//            {
//                var subscriber = invocationList[count];
//                subscriber.DynamicInvoke(this, EventArgs.Empty);
//                await UniTask.Yield();
//                count++;
//            }
//        }
//    }
//}
