using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.CHATLOG3
{
    public class ChatTextObject : PrefabScriptBase, IEntity
    {
        [SerializeField] private TextMeshProUGUI TextComponent;
        public Guid Id => MessageEntity.Id;
        public Message MessageEntity;
        public string FullTextLine => $"{MessageEntity.Name}-{Text}";

        public string Text
        {
            get { return this.TextComponent.text; }
            set { this.TextComponent.text = value; }
        }

        public async UniTask Initialize(Message message)
        {
            await WarnNull();
            if (message is null)
            {
                string mess = "dont forget to initialize";
                throw new NotImplementedException(mess);
            }
            this.MessageEntity = message;




            this.Text = message.Text;
        }

        public async UniTask WarnNull()
        {
            var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<SerializeField>() != null)
                .Where(x => x.GetValue(this) is null);

            foreach (var field in fields)
            {
                Debug.LogError($"Forgot to match prefabs serializes ${field.Name} in ${this.name}");
            }
        }
    }
}
