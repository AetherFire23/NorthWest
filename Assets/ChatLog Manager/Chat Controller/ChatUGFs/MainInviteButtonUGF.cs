//using Assets;
//using Assets.ChatLog_Manager;
//using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
//using Assets.GameState_Management;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using WebAPI.Models;
//using Zenject;

//public class MainInviteButtonUGF : UGFWrapper, IInitializable, ITickable
//{
//    private readonly MainInviteButtonObjectScript inviteButtonScript;
//    private readonly MainInvitePanelUGF _invitePanel;
//    private readonly ClientCalls _clientCalls;
//    private readonly RoomTabBarUGF _roomTabsContainerObject;
//    private readonly GameStateManager _gameState;

//    public List<InviteButtonUGI> InviteButtonInstances = new List<InviteButtonUGI>(); // juste appeler invite button et rename inviteButtonWrapper

//    public MainInviteButtonUGF(MainInviteButtonObjectScript script,
//        MainInvitePanelUGF invitePanel,
//        ClientCalls clientCalls,
//        RoomTabBarUGF roomTabsContainerObject,
//        GameStateManager gameState) : base(script) // injected ! 
//    {
//        this.inviteButtonScript = script;
//        _invitePanel = invitePanel;
//        _clientCalls = clientCalls;
//        _roomTabsContainerObject = roomTabsContainerObject;
//        _gameState = gameState;
//    }

//    public void Initialize()
//    {
//        this.inviteButtonScript.button.AddMethod(CloseInvitePanel);
//        this.InviteButtonInstances = BuildButtonsTemp() ;
//        AddInvitationCalls(this.InviteButtonInstances);
//    }

//    public void Tick()
//    {

//    }

//    public void CloseInvitePanel()
//    {
//        bool isActive = _invitePanel.GameObject.active;
//        this._invitePanel.GameObject.SetActive(!isActive);
//    }

//    public void AddInvitationCalls(List<InviteButtonUGI> inviteButtons)
//    {
//        foreach (var button in inviteButtons)
//        {
//            // button.script.Component.onClick.AddListener(delegate { InvitePlayer(button.PrivateInvitation); }); // ajoute dans le bouton la methode 
//            //    button.script.Component.onClick.AddListener(() => InvitePlayer(button.PrivateInvitation)); // ajoute dans le bouton la methode 
//            button.AccessScript.Component.onClick.AddListener(() => InvitePlayer(button.Player)); // ajoute dans le bouton la methode 
//        }
//    }

//    public void InvitePlayer(Player player)
//    {
//        var invite = new PrivateInvitation()
//        {
//            Id = Guid.NewGuid(),
//            ToPlayerId = player.Id,
//            RoomId = _roomTabsContainerObject.CurrentRoomGuid,
//            IsAccepted = false,
//            RequestFulfilled = false,
//            FromPlayerId = _gameState.PlayerUID,
//            FromPlayerName = _gameState.LocalPlayerDTO.Name,
//            ToPlayerName = player.Name,
//        };
//        _clientCalls.InviteToChatRoom(invite);
//    }

//    public void InvitePlayer(PrivateInvitation invite) // ma question : est-ce que je 
//    {
//        Debug.Log($"Following player is invited : {invite.ToPlayerName}");
//        invite.Id = Guid.NewGuid(); // evite de crash le serveur
//        invite.RoomId = _roomTabsContainerObject.CurrentRoomGuid;
//        _clientCalls.InviteToChatRoom(invite);
//    }

//    public List<InviteButtonUGI> SpawnInvitesInstances(List<PrivateInvitation> invites)
//    {
//        List<InviteButtonUGI> buttons = new List<InviteButtonUGI>();

//        foreach (var invite in invites)
//        {
//            var newButton = new InviteButtonUGI(_invitePanel.GameObject, invite, invite.ToPlayerName); // faudrait que le bouton save le invitation
//            buttons.Add(newButton);
//        }

//        return buttons;
//    }


//    public List<InviteButtonUGI> BuildButtonsTemp() // actual good one
//    {
//        //   List<Player> players = _clientCalls.GetPlayersCurrentGame(_mainPlayer.playerModel.Id);
//        List<Player> players = _gameState.Players;

//        List<InviteButtonUGI> buttons = new List<InviteButtonUGI>();

//        foreach (var player in players)
//        {
//            var newButton = new InviteButtonUGI(_invitePanel.GameObject, player); // faudrait que le bouton save le invitation
//            buttons.Add(newButton);
//        }
//        return buttons;
//    }
//}

