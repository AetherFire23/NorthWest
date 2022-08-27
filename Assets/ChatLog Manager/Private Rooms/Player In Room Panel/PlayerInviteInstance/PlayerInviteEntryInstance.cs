
using Assets.ChatLog_Manager.Private_Rooms.PlayerOptionsPanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInviteEntryInstance : InstanceWrapper<PlayerInviteEntryInstanceScript>
{
    private List<OptionEntryInstance> options = new List<OptionEntryInstance>();
    private PlayerModel _selectedPlayer;
    private const string resourceName = "PlayerInviteEntryPrefab";
    private readonly InvitePanel _invitePanel;

    public PlayerInviteEntryInstance(InvitePanel panel, PlayerModel playerModel) : base(resourceName, panel.GameObject)
    { 
        _selectedPlayer = playerModel;
        _invitePanel = panel;
        //this.script.ButtonComponent.onClick.AddListener(delegate { GenerateInviteOptions(); });
        InitializeComponents();
    }

    private void GenerateOptions() // sera splittable selon les conditions specifiees p-t par un autre api call
    {
        AddInviteOption();
    }

    private void InitializeComponents()
    {
        this.InstanceBehaviour.ButtonTextComponent.text = _selectedPlayer.Name;

    }

    private void AddInviteOption()
    {
        if (_invitePanel.OptionsPanelExists)
        {
            _invitePanel.currentOptionsPanel.UnityInstance.SelfDestroy();
        }

        _invitePanel.currentOptionsPanel = new PlayerOptionsPanelInstance(this.UnityInstance);

        var entryInfo = new InviteEntry(_invitePanel, _selectedPlayer);
        var InviteOption = new OptionEntryInstance(_invitePanel.currentOptionsPanel.UnityInstance, entryInfo);
        this.options.Add(InviteOption);
    }
}