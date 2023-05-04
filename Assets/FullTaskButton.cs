using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FullTaskButton : PrefabScriptBase
{
    [SerializeField] private TextMeshProUGUI _text;
    public GameTaskBase GameTask { get; private set; }
    public async UniTask Initialize(GameTaskBase gameTask)
    {
        this.GameTask = gameTask;
    }
}
