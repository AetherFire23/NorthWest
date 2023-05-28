using Assets.Dialogs;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using Shared_Resources.Models;
using Shared_Resources.Scratches;
using System.Collections.Generic;
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

    public async UniTask SelectTargetsThenExecuteGameTask(GameState gameState, GameTaskBase gameTask)
    {
        if (_optionsDialog is not null) return;

        var allParameters = new TaskParameters();
        var checkLists = gameTask.GetCheckLists(gameState);

      
        foreach (var prompt in checkLists)
        {
            var options = prompt.GetPromptsAsObjects();
            _optionsDialog = await _dialogManager.CreateAndInitializeOptionsDialog2(prompt.Description, options, prompt.IsMultipleChecks, prompt.MinimumChecks, prompt.MaximumChecks);
            await _optionsDialog.WaitForResolveCoroutine();

            var parameters = _optionsDialog.GetToggledAsParameters();
            allParameters.AddRange(parameters);

            await _optionsDialog.Destroy();
        }

        // validate here
        var gameTaskContext = new GameTaskContext()
        {
            GameState = gameState,
            Parameters = allParameters
        };
        var validationREsult = gameTask.Validate(gameTaskContext);

        if (!validationREsult.IsValid)
        {
            Debug.LogError($"The task{gameTask.Code} could not be executed.");
            return;
        }

        var callResult = await _calls.TryExecuteGameTask(gameState.PlayerDTO.Id, gameTask.Code, allParameters);

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
