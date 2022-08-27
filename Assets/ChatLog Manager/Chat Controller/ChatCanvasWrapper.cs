using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ChatLog_Manager
{
    public class ChatCanvasWrapper : GameObjectWrapper 
    {
        public ChatCanvasScript ChatCanvasScript;
        public ChatCanvasWrapper(ChatCanvasScript chatCanvasScript) : base(chatCanvasScript)
        {
            ChatCanvasScript = chatCanvasScript;
        }
    }
}
