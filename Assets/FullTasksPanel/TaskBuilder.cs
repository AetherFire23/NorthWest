using Assets.AssetLoading;
using Assets.Dialogs;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Constants;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
using Shared_Resources.GameTasks.Implementations_Unity;
using Shared_Resources.Interfaces;
using Shared_Resources.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskBuilder : MonoBehaviour
{
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private Calls _calls;
    [SerializeField] private GameLauncherAndRefresher _gameLauncherAndRefresher;
    // Should somewhat map the taskCodes and the gameTasks
    // NOTES : 
    // 1. Handle player target
    // 2. room targets
    // 3. special dialogs
    // 4. common dialogs

    private OptionsDialogScript2 _optionsDialog;

    public async UniTask SendGameTaskAfterTargetSelections(GameState gameState, GameTaskBase gameTask)
    {
        if (_optionsDialog is not null) return;

        var targetPrompts = gameTask.GetValidTargetPrompts(gameState);
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        // does not handle ExactAmountChecks
        // let it break in task validation for now
        foreach (var prompt in targetPrompts.CheckLists)
        {
            var options = prompt.GetPromptsAsObjects();
            _optionsDialog = await _dialogManager.CreateAndInitializeOptionsDialog2(prompt.Description, options, prompt.IsMultipleChecks, prompt.MinimumChecks, prompt.MaximumChecks);
            await _optionsDialog.WaitForResolveCoroutine();

            var parameterizedTargets = _optionsDialog.GetSelectionsAsDialogParameters();
            parameterizedTargets.ForEach(x => parameters.Add(x.Key, x.Value));

            await _optionsDialog.Destroy();
        }

        // validate here
        var gameTaskContext = new GameTaskContext()
        {
            GameState = gameState,
            Parameters = parameters
        };
        var validationREsult = gameTask.Validate(gameTaskContext);

        if (!validationREsult.IsValid)
        {
            Debug.LogError($"The task{gameTask.Code} could not be executed.");
            return;
        }

        var callResult = await _calls.TryExecuteGameTask(gameState.PlayerDTO.Id, gameTask.Code, parameters);

        if (callResult.IsSuccessful)
        {
            await _gameLauncherAndRefresher.ForceRefreshManagers();
            Debug.Log("Task executed gracefully!");
        }
        else
        {
            Debug.Log("Task went wrong in webapi boi");
        }

        _optionsDialog = null;
    }
}
