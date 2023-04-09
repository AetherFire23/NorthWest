using Assets.Big_Tick_Energy;
using Assets.FullTasksPanel;
using Assets.GameState_Management;
using Assets.Utils;
using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Shared_Resources.GameTasks.Implementations_Unity;
using Assets.Game_Tasks;
using System.Reflection;
using Assets.Scratch;

public class FullTasksManager : MonoBehaviour
{
    [Inject] GameStateManager _gameStateManager;
    [Inject] GlobalTick _tick;
    [Inject] ClientCalls _client;

    [SerializeField] private GameObject _taskScrollViewContent;

    // Includes both emitter text separation and 
    // task buttons. I will probably delete and refresh everything every Tick since
    // tehre wont be many tasks and the code for task comparison will probably be tedious.

    private UGICollection<TaskEmitterText, BasicTextScript> _emitterTexts = new(); // is this bugging ?
    private UGICollection<TaskButton, BasicButtonScript> _taskButtons = new();
    private List<IGameTask> _gameTasks;

    private void Start()
    {
        StaticScratch.Lolzida();

        var s = typeof(IGameTask).Assembly.GetTypes();
        var z = s.Where(x => typeof(IGameTask).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToList();

        var activated = Activator.CreateInstance(z.First());

        _tick.TimerTicked += this.OnTimerTick;


        try
        {
            var ttt = GameTaskReflection.GetGameTaskTypes();

        }
        catch (TypeLoadException ex)
        {
            Debug.LogError(ex.Message);
        }


        var zzz = GameTaskReflection.CreateGameTaskInstances();
        //_emitterTexts = new();
        //_taskButtons = new();
        //RefreshAvailableTasks();
    }

    public void BuildTaskButtons()
    {
        var visibleTasks = _gameTasks.Where(x => x.CanShow(_gameStateManager.CurrentGameState)).ToList();

        // foreach emittor type, place the appropriate visible tasks
        foreach (var gameTaskEmittor in (GameTaskEmittorTypes[])Enum.GetValues(typeof(GameTaskEmittorTypes)))
        {
            var tasks = new TaskEmitterText(_taskScrollViewContent, gameTaskEmittor.ToSafeString(), "temp");

            var tasksFromEmittor = visibleTasks.Where(task => task.EmittorType.Equals(gameTaskEmittor));

            //var emulation = tasksFromEmittor. 

        }
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

    private void OnTimerTick(object source, EventArgs e)
    {
        //RefreshAvailableTasks();
    }
}