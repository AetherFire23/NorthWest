using Assets.GameLaunch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HttpStuff
{
    public abstract class HttpCallerBase : MonoBehaviour 
    {
        protected DateTime? _timeStamp;
        protected Client HttpClient = new Client();
        private void OnApplicationQuit()
        {
            HttpClient.Dispose();
        }
    }
}
