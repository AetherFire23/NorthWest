using Assets;
using Assets.AssetLoading;
using Assets.GameLaunch;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;


// Fydar 
// RuntimeeInitializationOnLoadMethod


// scriptable object builderIHost thingy
// DisableEverything
// BootScene,
// 

// Application.quitting += stuff

// click button run
// scenes to create instances of game (Fydar added)
// 

[DefaultExecutionOrder(-10)]
public class GameLauncherAndRefresher : MonoBehaviour
{
    [SerializeField] bool _allowRefresh;
    [SerializeField] private bool _timeBasedRefresh;

    [SerializeField] private TemporaryOptionsScript _temporaryOptionsScript;
    [SerializeField] private GameCalls _client;
    [SerializeField] private GameStateFetcher _gameStateFetcher;
    [SerializeField] private PrefabLoader _prefabLoader;
    [SerializeField] private LocalPLayerManager _localPLayerManager;

    private List<IStartupBehavior> _managers = new List<IStartupBehavior>();
    private List<IRefreshable> _refreshables = new List<IRefreshable>();

    private bool _isInitializing = false;
    async UniTask Awake() // EFFECTIVE ENTRY POINT
    {

    }

    async UniTask Start()
    {
        if (_isInitializing) return;
        _isInitializing = true;

        Debug.Log("Loading");
        await LaunchLogger.WarnForMissingSerializables();
        await _prefabLoader.InitializeAsync();  // TODO check for semi-injection of unique manager ? 

        PlayerInfo.UID = _temporaryOptionsScript.CurrentPlayerID; // for optionsScript in scene


        //Guid id = TemporaryOptionsScript2.Instance is null
        //    ? new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F")
        //    : new Guid(TemporaryOptionsScript2.Instance.CurrentPlayerUID);
        //PlayerInfo.UID = id; // for persistent options script

        DiscoverManagers();

        Stopwatch stop = new Stopwatch();
        stop.Start();
        await InitializeManagersAsync();
        stop.Stop();
        _isInitializing = false;
        Debug.Log($"Finished loading. with loading time of : {stop.ElapsedMilliseconds} milliseconds");
    }

    private float _maximumTime = 3f;
    private float _currentTimeElapsed = 0;
    private int _tickAmount = 0;
    private bool _isRefreshing = false;

    // Handles refresh ticks, only if it has initialized
    async UniTask Update() // Ticks 
    {
        if (_isRefreshing || _isInitializing) return;
        _isRefreshing = true;
        _currentTimeElapsed += Time.deltaTime;

        if (_currentTimeElapsed > _maximumTime) // watch out ehre
        {
            if (!_allowRefresh) return;

            // Here is right before refreshing managers, so I could update position I guess
            await _client.UpdatePosition(PlayerInfo.UID, _localPLayerManager.PlayerPosition.x, _localPLayerManager.PlayerPosition.y);
            await RefreshManagersAsync();
            Debug.Log($"has ticked for {_tickAmount}");
            _tickAmount++;
            _currentTimeElapsed = 0;

        }
        _isRefreshing = false;
    }

    private async UniTask InitializeManagersAsync() // enforce order ?, LOADING SCREEN ! Rien mettre dinteraction ici
    {
        var gameStateWatch = new Stopwatch();
        gameStateWatch.Start();
        var firstGameState = await _gameStateFetcher.FetchFirstGameStateAsync(PlayerInfo.UID);
        gameStateWatch.Stop();
        Debug.Log($"Loaded the gameState in {gameStateWatch.ElapsedMilliseconds}");

        var managersWatch = new Stopwatch();
        managersWatch.Start();
        foreach (var manager in _managers)
        {
            await manager.Initialize(firstGameState);
        }
        managersWatch.Stop();
        Debug.Log($"Initialized maangers in {managersWatch.ElapsedMilliseconds}");

        //var tasks = _managers.Select(x => x.Initialize(firstGameState));
        //await UniTask.WhenAll(tasks);
    }

    // on peut pas await un dialog dans le Initialize pour dire que Cest loaded. 
    // ca voudrait dire que faut peser sur des boutons pendant le loading screen.
    private async UniTask RefreshManagersAsync()
    {
        GameState gameState = await _gameStateFetcher.FetchNextGameStateAsync(PlayerInfo.UID);
        var tasks = _refreshables.Select(x => x.Refresh(gameState));
        await UniTask.WhenAll(tasks);
    }

    public async UniTask ForceRefreshManagers()
    {
        await WaitUntilRefreshEndsCoroutine();
        await RefreshManagersAsync();
        _currentTimeElapsed = 0;
    }

    private async UniTask WaitUntilRefreshEndsCoroutine()
    {
        while (_isRefreshing)
        {
            await UniTask.Yield();
        }
    }

    private void DiscoverManagers()
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
