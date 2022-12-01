
//using Assets.ChatLog_Manager.Private_Rooms.PlayerOptionsPanel;
using Assets.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInRoomEntryUGI : InstanceWrapper<PlayerInRoomEntryAS>, IEntity
{
    public Guid Id => _selectedPlayer.Id;
   // private List<OptionEntryInstance> options = new List<OptionEntryInstance>();
    private Player _selectedPlayer;
    private const string resourceName = "PlayerInviteEntryPrefab";
    //private readonly PlayerInRoomPanelUGF _invitePanel;

    public PlayerInRoomEntryUGI(GameObject parent, Player playerModel) : base(resourceName, parent)
    { 
        _selectedPlayer = playerModel;
        //this.script.ButtonComponent.onClick.AddListener(delegate { GenerateInviteOptions(); });
        InitializeComponents();
    }

    private void GenerateOptions() // sera splittable selon les conditions specifiees p-t par un autre api call
    {
       // AddInviteOption();
    }

    private void InitializeComponents()
    {
        this.AccessScript.ButtonTextComponent.text = _selectedPlayer.Name;

    }

    //private void AddInviteOption()
    //{
    //    if (_invitePanel.OptionsPanelExists)
    //    {
    //        _invitePanel.currentOptionsPanel.UnityInstance.SelfDestroy();
    //    }

    //    _invitePanel.currentOptionsPanel = new PlayerOptionsPanelInstance(this.UnityInstance);

    //    var entryInfo = new InviteEntry(_invitePanel, _selectedPlayer);
    //    var InviteOption = new OptionEntryInstance(_invitePanel.currentOptionsPanel.UnityInstance, entryInfo);
    //    this.options.Add(InviteOption);
    //}
}