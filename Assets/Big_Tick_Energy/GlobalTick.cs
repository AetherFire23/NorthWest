using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Big_Tick_Energy
{
    public class GlobalTick : ITickable
    {
        public List<string> SubscribedMembers { get; set; } = new();

        public delegate void TimerTickedEventHandler(object source, EventArgs args);
        public event TimerTickedEventHandler TimerTicked;

        private float _maximumTime = 3f;
        private float _currentTimeElapsed;
        private int _tickAmount;

        public void Tick() // From Interface
        {
            try
            {
                _currentTimeElapsed += Time.deltaTime;
                bool thresholdReached = _currentTimeElapsed > _maximumTime;

                if (thresholdReached)
                {
                    _currentTimeElapsed = 0;
                    //await UniTask.RunOnThreadPool(OnTimerTick);
                    //UniTask.RunOnThreadPool(OnTimerTick);
                    OnTimerTick();
                    _tickAmount++;
                }
            }
            catch(Exception e)
            {
                Debug.Log("An error related to the tick occurred. Probably caused by async stuff");
            }

        }

        private void OnTimerTick()
        {
            if (TimerTicked is null) return;

            TimerTicked(this, EventArgs.Empty);

            Debug.Log($"Timer has ticked for {_tickAmount} times. Subscribed members : {String.Join(',', this.SubscribedMembers)}");
            this.SubscribedMembers.Clear();
        }
    }
}
