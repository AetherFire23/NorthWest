using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.ChatLog_Manager
{
    public class TextObjectInstance : InstanceWrapper<TextObjectScript> // Eventually make instancehelper a private class maybe ? 
    {
        public readonly MessageModel MessageModel;
        private const string _resourceName = "ChatTextPrefab";
        private GameObject _parent;
        private string _text;
        private string _sender;// put the specific components here 

        public TextObjectInstance(GameObject parent, MessageModel messageModel) : base(_resourceName, parent)
        {
            this.MessageModel = messageModel;
            _parent = parent;
            _text = MessageModel.Text;
            _sender = MessageModel.Name;
            this.InstanceBehaviour.TextComponent.text = GetMessageLine();
        }

        public string GetMessageLine()
        {
            string message = $"{_sender} : {_text}";
            return message;
        }
    }
}