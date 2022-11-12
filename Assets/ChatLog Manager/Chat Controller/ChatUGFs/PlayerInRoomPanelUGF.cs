using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using Assets.GameState_Management;

public class PlayerInRoomPanelUGF : UGFWrapper, IInitializable, ITickable
{
    private readonly GameStateManager _gameStateManager;


    public InvitePanelObjectScript Behaviour;
    public List<PlayerInRoomEntryUGI> playerEntries = new List<PlayerInRoomEntryUGI>();

    public PlayerInRoomPanelUGF(InvitePanelObjectScript invitePanelObjectScript,
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

    public void InitializePlayerEntries(List<Player> playersInChatRoom) 
    {// si probleme dans linitialization, remettre ca 
       // this.ClearPlayerEntries();
        RefreshPlayerEntries(playersInChatRoom);
    }

    public void RefreshPlayerEntries(List<Player> playersInChatRoom)
    {
        var playerEntries = CreatePanelEntries(playersInChatRoom);
        this.playerEntries.AddRange(playerEntries);
    }

    public void ClearPlayerEntries()
    {
        playerEntries.ForEach(player => player.UnityInstance.SelfDestroy());
        playerEntries.Clear();
    }
    private List<PlayerInRoomEntryUGI> CreatePanelEntries(List<Player> players)
    {
        List<PlayerInRoomEntryUGI> entries = players.Select(
            player => CreatePanelEntry(player)).ToList();
        return entries;
    }
    private PlayerInRoomEntryUGI CreatePanelEntry(Player model)
    {
        PlayerInRoomEntryUGI playerInviteEntryInstance = new PlayerInRoomEntryUGI(this, model);
        return playerInviteEntryInstance;
    }
   // public bool OptionsPanelExists => currentOptionsPanel != null;
}