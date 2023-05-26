using Assets.Dialogs.DIALOGSREFACTOR;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YesNoDialog : DialogBase
{
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public Button OkButton;
    [SerializeField] public Button CancelButton;

    public async UniTask Initialize(string message)
    {
        OkButton.AddTaskFunc(async () => await this.ResolveDialog(DialogResult.Ok));
        CancelButton.AddTaskFunc(async () => await this.ResolveDialog(DialogResult.Cancel));

        Text.text = message;
    }
}
