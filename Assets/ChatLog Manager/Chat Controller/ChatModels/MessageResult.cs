using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager.Private_Rooms.ChatModels
{
    public class MessageResult
    {
        public string Message { get; set; }
        public bool IsEmpty { get; set; }

        public MessageResult(string rawMessage)
        {
            string trimmedMessage = TrimHiddenCharacter(rawMessage);
            Message = trimmedMessage;
            IsEmpty = CheckEmptyMessage(trimmedMessage);
        }

        private string TrimHiddenCharacter(string rawMessage)
        {
            return rawMessage.Trim((char)8203);
        }

        public bool CheckEmptyMessage(string message)
        {
            return string.IsNullOrEmpty(message);
        }

        // self.text.Trim((char)8203);
    }
}
