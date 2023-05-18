using Assets;
using Assets.AssetLoading;
using Assets.FACTOR3;
using Assets.GameLaunch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameLauncherAndRefresher : MonoBehaviour
{
    [SerializeField] bool _allowRefresh;



    [SerializeField] private TemporaryOptionsScript _temporaryOptionsScript;
    [SerializeField] private Calls _client;
    [SerializeField] private GameStateFetcher _gameStateFetcher;
    [SerializeField] private PrefabLoader _prefabLoader;
    private Guid _playerUID => _temporaryOptionsScript.CurrentPlayerID;

    
    private List<IStartupBehavior> _managers = new List<IStartupBehavior>();
    private List<IRefreshable> _refreshables = new List<IRefreshable>();


    private bool _isInitializing = false;
    async UniTask Awake() // EFFECTIVE ENTRY POINT
    {
        if (_isInitializing) return;
        _isInitializing = true;

        Debug.Log("Loading");
        await LaunchLogger.WarnForMissingSerializables();
        await _prefabLoader.InitializeAsync();  // TODO check for semi-injection of unique manager ? 
        PlayerInfo.UID = _temporaryOptionsScript.CurrentPlayerID;
        FindManagers();

        await InitializeManagersAsync();
        _isInitializing = false;
        Debug.Log("Finished loading.");
    }

    private float _maximumTime = 3f;
    private float _currentTimeElapsed = 0;
    private int _tickAmount = 0;


    private bool _isRefreshing = false;
    // Handles refresh ticks
    async UniTask Update() // Ticks 
    {
        if (_isRefreshing || _isInitializing) return;
        _isRefreshing = true;
        _currentTimeElapsed += Time.deltaTime;

        if (_currentTimeElapsed > _maximumTime)
        {
            if (!_allowRefresh) return;

            await RefreshManagersAsync();
            Debug.Log($"has ticked for {_tickAmount}");
            _tickAmount++;
            _currentTimeElapsed = 0;

        }
        _isRefreshing = false;
    }


    private async UniTask InitializeManagersAsync() // enforce order ?, LOADING SCREEN ! Rien mettre dinteraction ici
    {
        var firstGameState = await _gameStateFetcher.FetchFirstGameStateAsync(_playerUID);

        var tasks = _managers.Select(x => x.Initialize(firstGameState));
        await UniTask.WhenAll(tasks);
    }

    // on peut pas await un dialog dans le Initialize pour dire que Cest loaded. 
    // ca voudrait dire que faut peser sur des boutons pendant le loading screen.
    //private async UniTask PostInitializeAwaiters

    private async UniTask RefreshManagersAsync()
    {
        GameState gameState = await _gameStateFetcher.FetchNextGameStateAsync(_playerUID);
        var tasks = _refreshables.Select(x => x.Refresh(gameState));
        await UniTask.WhenAll(tasks);
    }
    private void FindManagers()
    {
        MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();
        _managers = monoBehaviours
            .Where(x => typeof(IStartupBehavior).IsAssignableFrom(x.GetType()))
            .Select(x => x as IStartupBehavior).ToList();

        _refreshables = monoBehaviours
            .Where(x => typeof(IRefreshable).IsAssignableFrom(x.GetType()))
            .Select(x => x as IRefreshable).ToList();
    }
}
