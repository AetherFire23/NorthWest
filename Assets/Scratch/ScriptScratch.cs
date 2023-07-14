using Assets.AssetLoading;
using Assets.Dialogs;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
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



        }
    }
}
