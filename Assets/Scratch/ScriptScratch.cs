using Assets.AssetLoading;
using Assets.Dialogs;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.GameLaunch;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace Assets.Scratch
{
    public class ScriptScratch : MonoBehaviour, IStartupBehavior
    {
        [SerializeField] private TMPro.TMP_Dropdown _dropDown;
        [SerializeField] private PrefabLoader prefab;

        private DropDownManager<Player> _dropDownManager;
        private bool _isInitialized = false;
        public async UniTask Initialize(GameState gameState)
        {
            if (_isInitialized) return;

            //_dropDownManager = new DropDownManager<Player>(gameState.Players, _dropDown);

            //_dropDown.AddDropDownAction(async () =>
            //{
            //    await prefab.CreateInstanceOfAsync<ToggleOption>(this.gameObject);
            //});

            _isInitialized = true;
        }
    }
}
