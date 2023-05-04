using Shared_Resources.GameTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.GameLaunch;
using Shared_Resources.Models;
using Assets.FACTOR3;
using Cysharp.Threading.Tasks;
using Assets.AssetLoading;

public class FullTasksManager : MonoBehaviour, IRefreshable, IStartupBehavior
{
    [SerializeField] private Calls Calls;
    [SerializeField] private PrefabLoader _prefabLoader;
    [SerializeField] private GameObject _taskScrollViewContent;

    private List<GameTaskBase> _gameTasks = new();
    private GameState _gameState;

    private List<TaskEmitterText> _emittorButtons = new();
    private List<FullTaskButton> _fullTaskButtons = new();
    public async UniTask Initialize(GameState gameState)
    {
        // will not work cos dll uses IGameTask I should not use reflection from the dll hoesntly
        _gameState = gameState;
        _gameTasks = typeof(GameTaskBase).Assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
            && typeof(GameTaskBase).IsAssignableFrom(x))
            .Select(x => (Activator.CreateInstance(x) as GameTaskBase)).ToList();

        var taskProviders = UnityExtensions.GetEnumValues<GameTaskProvider>();

        foreach (GameTaskProvider provider in taskProviders)
        {
            var visible = _gameTasks.Where(x => x.Provider.Equals(provider))
                .Where(x => x.CanShow(_gameState)).ToList();

            // createButton
        }





        // this works
        // var t = _prefabLoader.CreateInstanceOfAsync<FullTaskButton>(_taskScrollViewContent);
        // var z = _prefabLoader.CreateInstanceOfAsync<TaskEmitterText>(_taskScrollViewContent);
    }

    public async UniTask Refresh(GameState gameState)
    {
        _gameState = gameState;
    }


    private async UniTask RefreshAvailableTasks(GameState gameState) // compare with gameTaskCodes
    {
        foreach (var taskProvider in UnityExtensions.GetEnumValues<GameTaskProvider>())
        {
            var getVisibleTasks = await GetVisibleTasksForTaskEmittor(taskProvider);

            var appeared =

        }
    }

    private async UniTask<List<GameTaskBase>> GetAppearedTasks(List<FullTaskButton> oldAvailableTasks, List<GameTaskBase> visibleTasks)
    {
        var currentTasks = oldAvailableTasks.Select(x => x.GameTask).ToList();

        var appearedTasks = visibleTasks.Where(task => !currentTasks.Contains(task)).ToList();
    }

    private async UniTask<List<GameTaskBase>> GetDisappeared()
    {


    }

    public void BuildTaskButtons()
    {
        //  var visibleTasks = _gameTasks.Where(x => x.CanShow()).ToList();
        var allEmittorTypes = UnityExtensions.GetEnumValues<GameTaskProvider>();

        // foreach emittor type, place the appropriate visible tasks
        foreach (GameTaskProvider emittor in allEmittorTypes)
        {
            //var taskEmittorGameObject = new TaskEmitterText(_taskScrollViewContent, emittor.ToSafeString(), "temp");

            //var tasksFromEmittor = visibleTasks.Where(task => task.Provider.Equals(emittor))
            //    .Select(x => new TaskButton(taskEmittorGameObject.UnityInstance, );

            // choisir une playerTarget dans un Window juste après ?

            // faut maintenant associer une fonction a un GameTask. 

            // GameTask avec ou sans Target ?

        }
    }

    public async UniTask<List<GameTaskBase>> GetVisibleTasksForTaskEmittor(GameTaskProvider provider)
    {
        var tasks = _gameTasks.Where(x => x.Provider == provider).ToList();
        var validTasks = _gameTasks.Where(x => x.CanShow(_gameState)).ToList();
        return validTasks;
    }

    public void RefreshAvailableTasks()
    {
        //_emitterTexts.Clear();
        //_taskButtons.Clear();

        //List<EmittorGameTasks> gameTaskInfos = new();
        //gameTaskInfos.Add(GetLocalPlayerTasks());

        //gameTaskInfos.Add(GetRoomTasks());

        //BuildTasks(gameTaskInfos);
    }

    //public void BuildTasks(List<EmittorGameTasks> tasks)
    //{
    //    foreach (EmittorGameTasks info in tasks)
    //    {
    //        _emitterTexts.Add(new TaskEmitterText(this._taskScrollViewContent, info.EmitterType, info.EmitterName));

    //        foreach(GameTaskAction gameTaskAction in info.Actions)
    //        {

    //            _taskButtons.Add(new TaskButton(_taskScrollViewContent, gameTaskAction));
    //        }
    //    }
    //}

    //public EmittorGameTasks GetLocalPlayerTasks()
    //{
    //    var localPlayerTasks = new EmittorGameTasks("Player", _gameState.LocalPlayerDTO.Name);

    //    localPlayerTasks.Actions.Add(new GameTaskAction()
    //    {
    //        taskName = "Sleep",
    //        action = () => Debug.Log("I did not currently program any task executed on self")
    //    });

    //    switch (_gameState.LocalPlayerDTO.Profession)
    //    {
    //        case RoleType.Commander:
    //            {
    //                localPlayerTasks.Actions.Add(new GameTaskAction()
    //                {
    //                    action = () => Debug.Log("This is a profession Task!"),
    //                    taskName = "Commander"
    //                });
    //                break;

    //            }
    //    }

    //    return localPlayerTasks;
    //}

    //public EmittorGameTasks GetRoomTasks()
    //{
    //    string emitterType = "Room";
    //    var roomTasksInfos = new EmittorGameTasks(emitterType, _gameState.Room.Name);
    //    switch (_gameState.Room.Name)
    //    {
    //        case "Kitchen1":
    //            {
    //                GameTaskAction cookTaskInfo = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);
    //                break;
    //            }

    //        case "EntryHall":
    //            {
    //                GameTaskAction cookTaskInfo = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);

    //                GameTaskAction cookTaskInfo2 = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);
    //                break;
    //            }

    //        case "Expedition1":
    //            {
    //                GameTaskAction cookTaskInfo = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);

    //                GameTaskAction cookTaskInfo2 = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);

    //                GameTaskAction cookTaskInfo3 = new GameTaskAction()
    //                {
    //                    action = () => _client.CookTask(_client.PlayerUID, "CookStation1"),
    //                    taskName = "CookTask"
    //                };
    //                roomTasksInfos.Actions.Add(cookTaskInfo);
    //                break;
    //            }

    //        default:
    //            {
    //                throw new Exception("Room name does not exist.");
    //            }
    //    }

    //    return roomTasksInfos;
    //}


}