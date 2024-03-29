﻿using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Assets.CHATLOG3
{
    public class ChatTextObject : PrefabScriptBase, IEntity
    {
        [SerializeField] private TextMeshProUGUI TextComponent;
        public Guid Id => Message.Id;
        public Message Message { get; private set; }
        public string FullTextLine => $"{Message.SenderName}-{Text}";

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
            this.Message = message;

            Text = $"{Message.SenderName} - {Message.Text}";
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
