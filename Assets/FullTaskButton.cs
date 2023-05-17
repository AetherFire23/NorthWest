using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullTaskButton : PrefabScriptBase
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;
    public GameTaskBase GameTask { get; private set; }
    public async UniTask Initialize(GameTaskBase gameTask, Func<UniTask> task)
    {
        this.GameTask = gameTask;
        _button.AddTaskFunc(task);
        _text.text = GameTask.Code.ToString();
    }
}
