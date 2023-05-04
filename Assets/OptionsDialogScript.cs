using Assets.Dialogs.DIALOGSREFACTOR;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsDialogScript : DialogBase
{
    [SerializeField] public OptionsDialogObjects OptionsDialogObjects;

    public List<ToggleOption> ToggleOptions { get; set; } = new();
    public List<ToggleOption> ToggledOptions => ToggleOptions.Where(x => x.Toggled).ToList();
    private bool AllowMultipleChecks;
    private int _maximumChecks;
    // initialized through dialogManager
    public async UniTask Initialize(bool allowMultipleChecks = false, int maximumChecks = 99)
    {
        OptionsDialogObjects.ResolveButton.AddTaskFunc(async () => await this.ResolveDialog(DialogResult.Ok));
        this.AllowMultipleChecks = allowMultipleChecks;
        _maximumChecks = maximumChecks;
    }

    // Update to prevent multiple checkcs from being made if multiple targets are allowed

    private void Update()
    {
        // janky behaviour. Will have to incorporate OnToggle or something and have to initialize the toggleOptions in the DIalog manager
        // The toggle method will save the current toggled option and if allowMUltiple = false, erase this last one to check the current checked one
        if (!AllowMultipleChecks && ToggledOptions.Count > 1)
        {
            ToggledOptions.First().Toggled = false;
        }

        if (AllowMultipleChecks && ToggledOptions.Count > _maximumChecks)
        {
            ToggledOptions.First().Toggled = false;
        }
    }

    public List<T> GetSelections<T>()
    {
        var selections = ToggleOptions.Where(x => x.Toggled).Select(x => (T)x.Option).ToList();
        return selections;
    }

    public List<object> GetRawSelections()
    {
        var selections = ToggledOptions.Select(x => x.Option).ToList();
        return selections;
    }
}
