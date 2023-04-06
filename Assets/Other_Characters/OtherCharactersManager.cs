using Assets.Big_Tick_Energy;
using Assets.GameState_Management;
using Assets.Input_Management;
using Assets.InputAwaiter;
using Assets.OtherCharacters;
using Assets.Raycasts.NewRaycasts;
using Assets.Utils;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class OtherCharactersManager : MonoBehaviour
{
    private GlobalTick _globalTick;
    private PlayerScript _playerScript;
    private DialogManager _dialogManager;
    private InputWaiting _inputWaiter;
    private GameStateManager _gameStateManager;
    private ClientCalls _clientCalls;
    private NewInputManager _newInputManager;
    private NewRayCaster _newRayCaster;
    private GameObject _roomManagerParent => this.gameObject;
    //private List<OtherCharacterUGI> _otherCharacters = new();
    private List<Player> _playersInRoom => _gameStateManager.PlayersInRoom;
    private UGICollectionEntity<OtherCharacterUGI, OtherCharacterAS, Player> _otherPlayersEditor { get; set; }
    private ReadOnlyCollection<OtherCharacterUGI> _otherPlayersUGIs => _otherPlayersEditor.UGIs;

    private List<Player> _otherPlayers => _gameStateManager.Players.Where(x=> x.Id != _gameStateManager.PlayerUID).ToList();

    void Start()
    {
        _otherPlayersEditor = new(BuildOtherCharacter);
        // _otherPlayersEditor.RefreshFromDbModels(_playersInRoom);
        _otherPlayersEditor.RefreshFromDbModels(_otherPlayers);
        _globalTick.TimerTicked += this.OnTimerTick;
    }

    void Update()
    {
        // Verifier si ya pas des joueurs qui ont apparu/ disparu (Ou faire un event ) quand on change de porte
        //faire move les joueurs vers leur target
    }

    private void UpdatePlayerTargetPosition() // Ontick
    {
        foreach (Player player in _playersInRoom)
        {
            var playerUGI = _otherPlayersUGIs.First(x => x.DbModel.Id == player.Id);
            playerUGI.AccessScript.CharacterMovement.TargetPosition = player.GetPosition();
        }
    }

    private OtherCharacterUGI BuildOtherCharacter(Player p)
    {
        var newUGI = new OtherCharacterUGI(_roomManagerParent, p);
        newUGI.UnityInstance.transform.position = p.GetPosition();
        newUGI.AccessScript.CharacterMovement.TargetPosition = p.GetPosition();
        return newUGI;
    }

    private void OnTimerTick(object source, EventArgs e)
    {
        _globalTick.SubscribedMembers.Add(this.GetType().Name);
        _otherPlayersEditor.RefreshFromDbModels(_otherPlayers);
        UpdatePlayerTargetPosition();
    }


    [Inject]
    public void Construct(NewRayCaster newRayCaster,
       NewInputManager newInputManager,
       ClientCalls clientCalls,
       GameStateManager gameStateManager,
       InputWaiting inputWaiter,
       DialogManager dialogManager,
       PlayerScript playerScript,
       GlobalTick globalTick)
    {
        _globalTick = globalTick;
        _playerScript = playerScript;
        _dialogManager = dialogManager;
        _inputWaiter = inputWaiter;
        _gameStateManager = gameStateManager;
        _clientCalls = clientCalls;
        _newInputManager = newInputManager;
        _newRayCaster = newRayCaster;
    }
}
