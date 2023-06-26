using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using TMPro;
using UnityEngine;

namespace Assets.FullTasksPanel
{
    public class TaskEmitterText : PrefabScriptBase
    {
        [SerializeField] private TextMeshProUGUI _text;
        public GameTaskCategory Category { get; private set; }
        public async UniTask Initialize(GameTaskCategory provider)
        {
            this.Category = provider;
            _text.text = provider.ToString();
        }
    }
}