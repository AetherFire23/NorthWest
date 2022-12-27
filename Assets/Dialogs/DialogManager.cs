using Assets.Big_Tick_Energy;
using Assets.Dialogs;
using Assets.Dialogs.New_System;
using Assets.InputAwaiter;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DialogManager
{
    // pour faire une window 
    private readonly GameObject _messageCanvas;
    public IDialog CurrentDialog { get; set; }
    public bool InstanceExists => this.CurrentDialog != null;
    public bool IsOkResult => this.CurrentDialog.DialogResult == DialogResult.Ok;
    public DialogResult DialogResult => this.CurrentDialog.DialogResult;
    public Queue<DialogAndAction> DialogQueue = new(); // queue de unitask a place ? 

    private readonly GlobalTick _tick;
    bool QueueIsRunning = false;
    //private readonly InputWaiting _waiter;
    public DialogManager(MessagesCanvasScript messageCanvas, GlobalTick tick)
    {
        _messageCanvas = messageCanvas.gameObject;
        _tick = tick;
        _tick.TimerTicked += OnTimerTick;
        //_waiter = waiter;
        //DialogQueue.Enqueue(TestQueue());
        //DialogQueue.Enqueue(TestQueue());
        //DialogQueue.Enqueue(TestQueue());

    }

    private DialogAndAction TestQueue()
    {
        DialogAndAction action = new()
        {
            ActionType = () =>
            {
                Debug.Log("SDSDdddddddddddddddddd");
                CurrentDialog = null;

            }
            ,
            DialogType = DialogType.MessageBox,
            Message = $"yeah !{this.DialogQueue.Count + 1}"
        };

        return action;

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

    private async void OnTimerTick(object source, EventArgs e)
    {


        if (CurrentDialog is not null) return;
        if (QueueIsRunning) return;
        if (DialogQueue.Count == 0) return;

        await StartDequeuingNotifications();
    }

    private async UniTask StartDequeuingNotifications()
    {
        // roule les dialogs 1 par un, ne permet pas de faire des nodes de dialog par contre. 

        QueueIsRunning = true;
        while (DialogQueue.Count >= 1)
        {
            DialogAndAction dialogActionPair = DialogQueue.Dequeue();
            CreateDialog(dialogActionPair.DialogType, dialogActionPair.Message);
            while (CurrentDialog.IsShowing)
            {
                await UniTask.Yield();
            }
            dialogActionPair.ActionType();
        }
        QueueIsRunning = false;
        CurrentDialog = null;
    }
}