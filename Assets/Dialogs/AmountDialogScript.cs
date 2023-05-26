using Assets.Dialogs.DIALOGSREFACTOR;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmountDialogScript : DialogBase
{
    [SerializeField] public Button OkButton;
    [SerializeField] public Button CancelButton;
    [SerializeField] public TextMeshProUGUI InputFieldTextComponent;
    [SerializeField] public TextMeshProUGUI PanelTextComponent;

    public async UniTask Initialize(string message)
    {
        await ConfigureResolveButton(OkButton, DialogResult.Ok);
        await ConfigureResolveButton(CancelButton, DialogResult.Cancel);
        this.PanelTextComponent.text = message;
    }

    public string GetTrimmedMessage()
    {
        return this.InputFieldTextComponent.text.Trim((char)8203);
    }
}
