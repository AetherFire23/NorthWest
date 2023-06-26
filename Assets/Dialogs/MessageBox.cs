using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public class MessageBox : DialogBase
    {
        protected override string Name => nameof(MessageBox);

        [SerializeField] public TextMeshProUGUI Text;
        [SerializeField] public Button OkButton;


        public async UniTask Initialize(string text)
        {
            OkButton.AddTaskFunc(async () => await this.ResolveDialog(DialogResult.Ok));

            Text.text = text;
        }
    }
}
