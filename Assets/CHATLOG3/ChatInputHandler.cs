
using Assets.GameLaunch;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CHATLOG3
{
    public class ChatInputHandler : MonoBehaviour, IStartupBehavior
    {
        [SerializeField] private Calls _calls;
        [SerializeField] private ChatLogManager3 _chatLogManager;
        [SerializeField] private TMP_InputField _inputField;

        private GameState _firstGameState;

        public async UniTask Initialize(GameState gameState)
        {
            _firstGameState = gameState;
        }

        bool _isSendingMessage = false;
        private async UniTask Update()
        {
            if (_isSendingMessage) return;
            if (!Input.GetKeyDown(KeyCode.Return)) return;
            _isSendingMessage = true;

            var response = await _calls.PutNewMessageToServer(_firstGameState.PlayerUID,
                _chatLogManager.GetCurrentShownRoom(),
                _inputField.text);// TRIM PLEASE SOON
            _inputField.text = string.Empty;
            _isSendingMessage = false;
        }
    }
}
