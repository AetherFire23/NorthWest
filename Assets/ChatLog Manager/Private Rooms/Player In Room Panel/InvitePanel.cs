using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets;
using Assets.ChatLog_Manager.Private_Rooms.PlayerOptionsPanel;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using Assets.GameState_Management;

public class InvitePanel : GameObjectWrapper, IInitializable, ITickable
{
    public InvitePanelObjectScript Behaviour;
    public PlayerOptionsPanelInstance currentOptionsPanel;

    private readonly GameStateManager _gameStateManager;

    //  private readonly ChatHandler _chatHandler;
    public List<PlayerInviteEntryInstance> playerEntries = new List<PlayerInviteEntryInstance>();

    public InvitePanel(InvitePanelObjectScript invitePanelObjectScript,
        GameStateManager gameStateManager) : base(invitePanelObjectScript)
    {
        Behaviour = invitePanelObjectScript;
        _gameStateManager = gameStateManager;
    }

    public void Initialize()
    {
        //  Initialized dans le chatHandler
    }

    public void Tick()
    {
    }

    public void InitializePlayerEntries(List<PlayerModel> playersInChatRoom) 
    {
        this.ClearPlayerEntries();
        RefreshPlayerEntries(playersInChatRoom);
    }

    public void RefreshPlayerEntries(List<PlayerModel> playersInChatRoom)
    {
        var playerEntries = CreatePanelEntries(playersInChatRoom);
        this.playerEntries.AddRange(playerEntries);
    }

    private void ClearPlayerEntries()
    {
        playerEntries.ForEach(player => player.UnityInstance.SelfDestroy());
        playerEntries.Clear();
    }
    private List<PlayerInviteEntryInstance> CreatePanelEntries(List<PlayerModel> players)
    {
        List<PlayerInviteEntryInstance> entries = players.Select(
            player => CreatePanelEntry(player)).ToList();
        return entries;
    }
    private PlayerInviteEntryInstance CreatePanelEntry(PlayerModel model)
    {
        PlayerInviteEntryInstance playerInviteEntryInstance = new PlayerInviteEntryInstance(this, model);
        return playerInviteEntryInstance;
    }
    public bool OptionsPanelExists => currentOptionsPanel != null;
}