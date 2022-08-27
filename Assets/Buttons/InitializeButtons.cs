using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;

namespace Assets.Buttons
{
    public class InitializeButtons : IInitializable
    {
        ChatButtonScript _chatButtonScript;
        public InitializeButtons(ChatButtonScript chatScript)
        {
            _chatButtonScript = chatScript;

        }

        public void Initialize()
        {
            _chatButtonScript.ChatButtonComponent.onClick.AddListener(SwitchChatButtonCanvas);
        }

        public void SwitchChatButtonCanvas()
        {
            Debug.Log("Chat Opened or closed");
            _chatButtonScript.ChatButtonCanvas.enabled = !_chatButtonScript.ChatButtonCanvas.enabled;
        }
    }
}
