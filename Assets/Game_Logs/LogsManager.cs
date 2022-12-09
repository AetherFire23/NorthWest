using Assets.Big_Tick_Energy;
using Assets.Game_Logs;
using Assets.GameState_Management;
using Assets.HttpClient.Shared_Entities;
using Assets.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LogsManager : MonoBehaviour
{
    // Les Private Logs devraient etre grises et prefixed au lieu de faire 2 prefab differents
    [SerializeField] private Button _openLogButton;
    [SerializeField] private Button _viewAllButton;
    [SerializeField] private Canvas _logCanvas;

    [SerializeField] private RectTransform _logContentTransform;
    [SerializeField] private RectTransform _playerFilterContentTransform;
    [SerializeField] private RectTransform _roomFilterContentTransform;

    [Inject] GameStateManager _gameState;
    [Inject] GlobalTick _tick;
    private List<Log> LogBank = new();

    private UGICollectionEntity<LogMessageUGI, TextObjectScript, Log> Logs { get; set; }

    private UGICollection<NormalButtonUGI, BasicButtonScript> PlayerFilterButtons = new UGICollection<NormalButtonUGI, BasicButtonScript>(); // Player entity
    private UGICollection<NormalButtonUGI, BasicButtonScript> RoomFilterButtons = new UGICollection<NormalButtonUGI, BasicButtonScript>();// les RoomId

    private Action _filter;

    void Start()
    {
        this.Logs = new UGICollectionEntity<LogMessageUGI, TextObjectScript, Log>((log) => new LogMessageUGI(this._logContentTransform.gameObject, log)); ;

        // sub to tick
        _tick.TimerTicked += this.OnTimerTick;

        // init buttons
        _openLogButton.AddMethod(() => _logCanvas.enabled = !_logCanvas.enabled);
        this._viewAllButton.AddMethod(ShowAllLogs);


        // init logs
        this.LogBank.AddRange(_gameState.Logs);
        ShowAllLogs();

        //

        InitializePlayerFilter();
        InitializeRoomFilter();
    }

    public void InitializePlayerFilter()
    {
        foreach (var player in _gameState.Players)
        {
            var playerButton = this.PlayerFilterButtons.Add(new NormalButtonUGI(_playerFilterContentTransform.gameObject, player.Name));
            playerButton.AccessScript.ButtonComponent.AddMethod(() =>
            {
                _filter = () => RefreshByPlayerId(player.Id);
                _filter();
            }
            );
        }
    }

    public void RefreshByPlayerId(Guid playerId)
    {
        var playerLogs = this.LogBank.Where(x => x.TriggeringPlayerId == playerId).ToList();

        Logs.RefreshFromDbModels(playerLogs);
    }

    public void RefreshByRoomId(Guid roomId)
    {
        var roomLogs = this.LogBank.Where(x => x.RoomId == roomId).ToList();

        Logs.RefreshFromDbModels(roomLogs);
    }

    public void InitializeRoomFilter()
    {
        foreach (var room in _gameState.Rooms)
        {
            var roomButton = this.RoomFilterButtons.Add(new NormalButtonUGI(_roomFilterContentTransform.gameObject, room.Name));
            roomButton.AccessScript.ButtonComponent.AddMethod(() =>
            {
                _filter = () =>RefreshByRoomId(room.Id);
                _filter();
            }
            );
        }
    }

    public void ShowAllLogs()
    {
        _filter = () => this.Logs.RefreshFromDbModels(this.LogBank);
        _filter();
    }

    private void OnTimerTick(object source, EventArgs e)
    {
        this.LogBank.AddRange(_gameState.Logs);
        _filter();
    }
}