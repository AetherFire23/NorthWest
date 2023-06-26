using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameLaunch.BaseLauncherScratch
{
    public class BaseManagerRefreshStopGuards
    {
        public bool IsInitializing { get; set; } = false;
        public bool IsRefreshing { get; set; } = false;

        private float _timeBetweenRefreshes { get; set; } = 3f;
        private float _elapsed { get; set; } = 0f;
        private int _timesTicked { get; set; } = 0;
        public bool MustPreventInitialization()
        {
            bool mustPrevent = IsInitializing;
            return mustPrevent;
        }
        public bool MustPreventRefreshing(float delta)
        {
            bool mustPrevent = IsInitializing || IsRefreshing || !TimeTicked(delta);
            return mustPrevent;
        }



        /// <summary>
        /// Appelle Time.deltaTime faque faut le caller dans Update() ppour que ca procc
        /// </summary>
        private bool TimeTicked(float delta)
        {
            _elapsed += delta;
            if (_elapsed > _timeBetweenRefreshes)
            {
                _elapsed = 0;
                _timesTicked++;
                return true;
            }
            return false;
        }
    }
}
