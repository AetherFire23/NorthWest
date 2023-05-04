using Assets.AssetLoading;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.FACTOR3;
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
using Unity.VisualScripting;
using UnityEngine;

public class TaskBui : MonoBehaviour
{
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private Calls _calls;
    // Should somewhat map the taskCodes and the gameTasks
    // NOTES : 
    // 1. Handle player target
    // 2. room targets
    // 3. special dialogs
    // 4. common dialogs

    public async UniTask ExecuteTask(GameState gameState, GameTaskBase gameTask)
    {
        var targetPrompts = gameTask.GetValidTargetPrompts(gameState);
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        // does not handle ExactAmountChecks
        // let it break in task validation for now
        foreach (var prompt in targetPrompts.CheckLists)
        {
            var toListOfObjects = prompt.GetPromptsAsObjects();
            OptionsDialogScript dialog;
            if (prompt.IsMultipleChecks)
            {
                dialog = await _dialogManager.CreateAndInitializeOptionsDialog(toListOfObjects, true, prompt.MaximumChecks);
            }
            else
            {
                dialog = await _dialogManager.CreateAndInitializeOptionsDialog(toListOfObjects);
            }
            await dialog.WaitForResolveCoroutine();

            int i = 0;
            // build parameters dictionary...
            var parameterizedTargets = dialog.ToggledOptions.Select(x => x.Option as ITaskParameter).ToList();
            var parametesrs = parameterizedTargets.Select((x, i) => x.GetKeyValuePairParameter(i)).ToList();
            // avertir ici si il manque un TaskParameterization
            parametesrs.ForEach(x => parameters.Add(x.Key, x.Value));

            await dialog.Destroy();
        }


        // validate here


        var callResult = await _calls.TryExecuteGameTask(gameState.PlayerDTO.Id, gameTask.Code, parameters);
    }
}
