using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager.Chat_Controller
{
    public class ChatUGF : UGFWrapper
    {
        public ChatScript ChatBehaviour;
        public ChatUGF(ChatScript chatScript) : base(chatScript) // devrons rendre private I guess, remet en question la pertinence du unityobject
        {
            ChatBehaviour = chatScript;
        }
        
        public string GetMessageFromInputField()
        {
            string inputText = this.ChatBehaviour.InputFieldTextComponent.GetTextWithoutHiddenCharacters();
            return inputText;
        }

        public void ClearInputField()
        {
            this.ChatBehaviour.InputFieldTextComponent.text = String.Empty;
        }
    }
}
