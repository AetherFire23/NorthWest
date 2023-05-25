using Shared_Resources.GameTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.GameLaunch;
using Shared_Resources.Models;
using Cysharp.Threading.Tasks;
using Assets.AssetLoading;
using Assets.HttpStuff;

public class FullTasksManager : MonoBehaviour, IRefreshable, IStartupBehavior
{
    [SerializeField] private Calls Calls;
    [SerializeField] private PrefabLoader _prefabLoader;
    [SerializeField] private GameObject _taskScrollViewContent;
    [SerializeField] private TaskBuilder _taskBuilder;

    // models
    private List<GameTaskBase> _gameTasks = new();
    private GameState _gameState;

    //prefabs
    private List<TaskEmitterText> _emittorTexts = new();
    private List<FullTaskButton> _fullTaskButtons = new();
    private List<GameTaskBase> _currentDisplayedGameTasks => _fullTaskButtons.Select(x => x.GameTask).ToList();

    public async UniTask Initialize(GameState gameState)
    {
        // will not work cos dll uses IGameTask I should not use reflection from the dll hoesntly
        _gameState = gameState;
        _gameTasks = typeof(GameTaskBase).Assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
            && typeof(GameTaskBase).IsAssignableFrom(x))
            .Select(x => (Activator.CreateInstance(x) as GameTaskBase)).ToList();

        var taskProviders = UnityExtensions.GetEnumValues<GameTaskProvider>();

        await RefreshAvailableTasks();
    }

    private bool _isRefreshing = false;
    public async UniTask Refresh(GameState gameState)
    {
        if (_isRefreshing)
        {
            Debug.Log("Cancelled refreshing fulltasksManager");
        }

        _isRefreshing = true;
        _gameState = gameState;
        await RefreshAvailableTasks();
        _isRefreshing = false;
    }

    private async UniTask RefreshAvailableTasks() // compare with gameTaskCodes
    {
        foreach (var appearedTask in await GetAppearedTasks())
        {
            FullTaskButton button = await _prefabLoader.CreateInstanceOfAsync<FullTaskButton>(_taskScrollViewContent);

            // CreateButtons?
            Func<UniTask> createPromptsThenSendTask = async () =>
            {
                await _taskBuilder.SendGameTaskAfterTargetSelections(_gameState, appearedTask);

            };

            await button.Initialize(appearedTask, createPromptsThenSendTask);
            _fullTaskButtons.Add(button);
            await PlaceTaskAfterEmittorIndexAndCreateOneIfItDoesntExist(button);
        }

        foreach (var disappearedTask in await GetDisappearedTasks())
        {
            FullTaskButton buttonToDelete = FindButtonFromGameTask(disappearedTask);
            _fullTaskButtons.Remove(buttonToDelete);
            buttonToDelete.gameObject.SelfDestroy();

            DeleteEmittorTextOrDoNothing(disappearedTask.Provider);
        }
    }

    private FullTaskButton FindButtonFromGameTask(GameTaskBase taskBase)
    {
        var taskButton = _fullTaskButtons.FirstOrDefault(x => x.GameTask.Code.Equals(taskBase.Code));

        if (taskButton is null) throw new Exception($"Tried to find a button with a gameTask that did not exist : {taskBase.Code}");

        return taskButton;
    }

    private void DeleteEmittorTextOrDoNothing(GameTaskProvider taskProvider)
    {
        bool hasEmittorRemainingTasks = _currentDisplayedGameTasks.Where(x => x.Provider == taskProvider)
            .ToList().Count != 0;

        if (hasEmittorRemainingTasks) return;

        var emittorToDelete = _emittorTexts.FirstOrDefault(x => x.Provider == taskProvider);

        // already deleted
        if (emittorToDelete is null) return;

        _emittorTexts.Remove(emittorToDelete);
        GameObject.Destroy(emittorToDelete.gameObject);
    }

    private async UniTask<List<GameTaskBase>> GetAppearedTasks()
    {
        List<GameTaskBase> newCalculatedTasks = _gameTasks.Where(x => x.Requires(_gameState)).ToList();
        var appearedTasks = newCalculatedTasks.Where(task => !_currentDisplayedGameTasks.Contains(task)).ToList();

        return appearedTasks;
    }

    private async UniTask<List<GameTaskBase>> GetDisappearedTasks()
    {
        List<GameTaskBase> newCalculatedTasks = _gameTasks.Where(x => x.Requires(_gameState)).ToList();
        var disappearedTasks = _currentDisplayedGameTasks.Where(x => !newCalculatedTasks.Contains(x)).ToList();

        return disappearedTasks;
    }

    private async UniTask PlaceTaskAfterEmittorIndexAndCreateOneIfItDoesntExist(FullTaskButton fullTaskButton)
    {
        var emittorText = _emittorTexts.FirstOrDefault(x => x.Provider == fullTaskButton.GameTask.Provider);
        if (emittorText is null)
        {
            emittorText = await _prefabLoader.CreateInstanceOfAsync<TaskEmitterText>(_taskScrollViewContent.gameObject);
            await emittorText.Initialize(fullTaskButton.GameTask.Provider);
            _emittorTexts.Add(emittorText);
        }

        int emittorTextIndex = emittorText.transform.GetSiblingIndex();
        fullTaskButton.transform.SetSiblingIndex(emittorTextIndex + 1);
    }
}
