﻿using Assets.AssetLoading;
using Assets.Dialogs;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scratch
{
    public class ScriptScratch : MonoBehaviour, IStartupBehavior
    {
        [SerializeField] private Transform _dialogCanvast;
        [SerializeField] private TMPro.TMP_Dropdown _dropDown;
        [SerializeField] private PrefabLoader prefab;
        [SerializeField] private DialogManager _dialogManager;

        private bool _isInitialized = false;
        public async UniTask Initialize(GameState gameState)
        {
            if (_isInitialized) return;

            string jsoNData = JsonConvert.SerializeObject(gameState);
            JObject jobj = JsonConvert.DeserializeObject<JObject>(jsoNData);

            string type = jobj.GetType().Name;
            string typ2 = jobj.Type.ToString();
            Debug.Log(type);



            PersistenceAccess.SaveData(gameState);

            var data = PersistenceAccess.LoadPersistentData<List<Type>>();
            PersistenceAccess.LoadPersistentData<GameState>();
        }
    }
}
