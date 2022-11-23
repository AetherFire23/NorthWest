//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Zenject;
//using UnityEngine;

//namespace Assets.Buttons
//{
//    public class OpenCloseButtonUGF : UGFWrapper, IInitializable
//    {
//        ChatButtonAS _chatButtonScript; // a ne jamais faire, les objets devraient sinitialize a la place
//        public OpenCloseButtonUGF(ChatButtonAS chatScript) : base(chatScript)
//        {
//            _chatButtonScript = chatScript;
//        }

//        public void Initialize()
//        {
//            InitChatLogButton();
//        }

//        public void InitChatLogButton()
//        {
//            Action switchChatButtonCanvas = () => _chatButtonScript.ChatButtonCanvas.enabled = !_chatButtonScript.ChatButtonCanvas.enabled;
//            _chatButtonScript.ChatButtonComponent.onClick.AddListener(delegate { switchChatButtonCanvas(); });
//        }
//    }
//}
