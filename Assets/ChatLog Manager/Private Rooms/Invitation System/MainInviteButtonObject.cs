using Assets;
using Assets.ChatLog_Manager;
using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MainInviteButtonObject : GameObjectWrapper, IInitializable, ITickable
{
    private readonly MainInviteButtonObjectScript inviteButtonScript;
    private readonly MainInvitePanelObject _invitePanel;
    private readonly ClientCalls _clientCalls;
    private readonly MainPlayer _mainPlayer;
    private readonly RoomTabBar _roomTabsContainerObject;

    public List<InviteButtonInstance> InviteButtonInstances = new List<InviteButtonInstance>(); // juste appeler invite button et rename inviteButtonWrapper

    public MainInviteButtonObject(MainInviteButtonObjectScript script,
        MainInvitePanelObject invitePanel,
        ClientCalls clientCalls,
        MainPlayer mainPlayer,
        RoomTabBar roomTabsContainerObject) : base(script) // injected ! 
    {
        this.inviteButtonScript = script;
        _invitePanel = invitePanel;
        _clientCalls = clientCalls;
        _mainPlayer = mainPlayer;
        _roomTabsContainerObject = roomTabsContainerObject;
    }

    public void Initialize()
    {
        this.inviteButtonScript.button.AddMethod(CloseInvitePanel);
        this.InviteButtonInstances = BuildButtonsTemp() ;
        AddInvitationCalls(this.InviteButtonInstances);
    }

    public void Tick()
    {

    }

    public void CloseInvitePanel()
    {
        bool isActive = _invitePanel.GameObject.active;
        this._invitePanel.GameObject.SetActive(!isActive);
    }

    public void AddInvitationCalls(List<InviteButtonInstance> inviteButtons)
    {
        foreach (var button in inviteButtons)
        {
            // button.script.Component.onClick.AddListener(delegate { InvitePlayer(button.PrivateInvitation); }); // ajoute dans le bouton la methode 
            //    button.script.Component.onClick.AddListener(() => InvitePlayer(button.PrivateInvitation)); // ajoute dans le bouton la methode 
            button.InstanceBehaviour.Component.onClick.AddListener(() => InvitePlayer(button.Player)); // ajoute dans le bouton la methode 
        }
    }

    public void InvitePlayer(PlayerModel player)
    {
        var invite = new PrivateInvitation()
        {
            Id = Guid.NewGuid(),
            ToPlayerId = player.Id,
            RoomId = _roomTabsContainerObject.CurrentRoomGuid,
            IsAccepted = false,
            RequestFulfilled = false,
            FromPlayerId = _mainPlayer.playerModel.Id,
            FromPlayerName = _mainPlayer.playerModel.Name,
            ToPlayerName = player.Name,
        };
        _clientCalls.InviteToChatRoom(invite);
    }

    public void InvitePlayer(PrivateInvitation invite) // ma question : est-ce que je 
    {
        Debug.Log(invite.ToPlayerName);
        invite.Id = Guid.NewGuid(); // evite de crash le serveur
        invite.RoomId = _roomTabsContainerObject.CurrentRoomGuid;
        _clientCalls.InviteToChatRoom(invite);
    }

    public List<InviteButtonInstance> SpawnInvitesInstances(List<PrivateInvitation> invites)
    {
        List<InviteButtonInstance> buttons = new List<InviteButtonInstance>();

        foreach (var invite in invites)
        {
            var newButton = new InviteButtonInstance(_invitePanel.GameObject, invite, invite.ToPlayerName); // faudrait que le bouton save le invitation
            buttons.Add(newButton);
        }

        return buttons;
    }


    public List<InviteButtonInstance> BuildButtonsTemp()
    {
        List<PlayerModel> players = _clientCalls.GetPlayersCurrentGame(_mainPlayer.playerModel.Id);

        List<InviteButtonInstance> buttons = new List<InviteButtonInstance>();

        foreach (var player in players)
        {
            var newButton = new InviteButtonInstance(_invitePanel.GameObject, player); // faudrait que le bouton save le invitation
            buttons.Add(newButton);
        }
        return buttons;
    }

    public List<PrivateInvitation> BuildPrivateInvitations()
    {
        List<PlayerModel> players = _clientCalls.GetPlayersCurrentGame(_mainPlayer.playerModel.Id);

        List<PrivateInvitation> invitations = new List<PrivateInvitation>();

        foreach (var player in players)
        {
            var invite = new PrivateInvitation()
            {
                Id = Guid.NewGuid(),
                ToPlayerId = player.Id,
                RoomId = _roomTabsContainerObject.CurrentRoomGuid,
                IsAccepted = false,
                RequestFulfilled = false,
                FromPlayerId = _mainPlayer.playerModel.Id,
                FromPlayerName = _mainPlayer.playerModel.Name,
                ToPlayerName = player.Name,
            };
            invitations.Add(invite);
        }
        return invitations;
    }
}

