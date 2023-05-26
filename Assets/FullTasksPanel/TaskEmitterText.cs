using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using TMPro;
using UnityEngine;

public class TaskEmitterText : PrefabScriptBase
{
    [SerializeField] private TextMeshProUGUI _text;
    public GameTaskProvider Provider { get; private set; }
    public async UniTask Initialize(GameTaskProvider provider)
    {
        this.Provider = provider;
        _text.text = provider.ToString();
    }
}
