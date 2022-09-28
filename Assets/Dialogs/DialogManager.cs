using Assets.Dialogs;
using Assets.Dialogs.New_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DialogManager
{
    private readonly GameObject _messageCanvas;
    public IDialog CurrentDialog { get; set; }
    public bool InstanceExists => this.CurrentDialog != null;
    public bool IsOkResult => this.CurrentDialog.DialogResult == DialogResult.Ok;
    public DialogResult DialogResult => this.CurrentDialog.DialogResult;


    public DialogManager(MessagesCanvasScript messageCanvas)
    {
        _messageCanvas = messageCanvas.gameObject;
    }

    public bool CanInvokeNewInstance()
    {
        return !InstanceExists;
    }

    public void CreateDialog(DialogType dialogType, string message) // adder les diags 
    {
        CurrentDialog = null; // Ne voulons pas ecraser le dialog au clic du bouton pour maintenir lacces au result

        switch (dialogType)
        {
            case DialogType.MessageBox:
                {
                    this.CurrentDialog = new NewMessageBoxUGI(_messageCanvas, message);
                    break;
                }
            case DialogType.YesNoDialog:
                {
                    this.CurrentDialog = new YesNoDialogUGI(_messageCanvas, message);
                    break;
                }
            case DialogType.AmountDialog:
                {
                    this.CurrentDialog = new AmountDialogUGI(_messageCanvas, message);
                    break;
                }
        }
    }

    public bool IsWaitingForInput()
    {
        if (CurrentDialog == null) return false;

        return CurrentDialog.IsShowing;

        throw new NotImplementedException("IsWaitingForInput() should always return a value before reaching the end of method.");
    }
}