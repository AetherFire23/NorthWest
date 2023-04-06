using Assets.Utils;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.ChatLog_Manager
{
    public class TextObjectUGI : InstanceWrapper<TextObjectScript>, IEntity 
    {
        public Guid Id => MessageModel.Id;
        public readonly Message MessageModel;
        private const string _resourceName = "ChatTextPrefab";
        private GameObject _parent;
        private string _text;
        private string _sender;// put the specific components here 

        public TextObjectUGI(GameObject parent, Message messageModel) : base(_resourceName, parent)
        {
            this.MessageModel = messageModel;
            _parent = parent;
            _text = MessageModel.Text;
            _sender = MessageModel.Name;
            this.AccessScript.TextComponent.text = GetMessageLine();
        }

        public string GetMessageLine()
        {
            string message = $"{_sender} : {_text}";
            return message;
        }
    }
}