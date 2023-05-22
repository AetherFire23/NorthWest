using Cysharp.Threading.Tasks;
using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogTextObject : PrefabScriptBase, IEntity
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    public Guid Id => LogEntity.Id;
    public Log LogEntity;
    public string FullTextLine => $"{LogEntity}-{Text}";

    public string Text
    {
        get { return this.TextComponent.text; }
        set { this.TextComponent.text = value; }
    }

    public async UniTask Initialize(Log log)
    {
        this.LogEntity = log;
        if (log is null)
        {
            string mess = "dont forget to initialize";
            throw new NotImplementedException(mess);
        }

        this.Text = log.EventText;
    }
}
