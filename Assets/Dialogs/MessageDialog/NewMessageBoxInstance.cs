﻿using Assets.Dialogs.New_System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class NewMessageBoxInstance : InstanceWrapper<NewMessageBoxScript>, IDialog
{
    public bool IsShowing { get; set; }
    public DialogResult DialogResult { get; set; }

    public const string resourceName = "MessageBox";

    public NewMessageBoxInstance(GameObject parent, string message) : base(resourceName, parent)
    {
        //must redo boy
        this.IsShowing = true;
        AddButtonAction(this.InstanceBehaviour.MessageBoxButtonComponent, MessageBoxClickConfirmation);
        this.InstanceBehaviour.PanelTextComponent.text = message;
    }

    public void MessageBoxClickConfirmation()
    {
        Resolve();
    }

    public void Resolve()
    {
        this.DialogResult = DialogResult.Ok;
        GameObject.Destroy(this.UnityInstance);
        this.IsShowing = false;
    }

    public void AddButtonAction(Button button, Action action)
    {
        button.onClick.AddListener(delegate { action(); });
    }
}