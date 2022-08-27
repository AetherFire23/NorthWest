using Assets.ChatLog_Manager;
using Assets.ChatLog_Manager.Private_Rooms.InvitePanelObject.PlayerOptionsPanel.OptionEntryInstance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InviteEntry : OptionEntryInfo
{
    public override string Name { get; set; } = "Private";
    private readonly InvitePanel _invitePanel;
    private PlayerModel _invitedPlayer;

    public InviteEntry(InvitePanel invitePanel, PlayerModel invitedPlayer)
    {
        _invitePanel = invitePanel;
        _invitedPlayer = invitedPlayer;
    }

    public override void EntryAction()
    {
        //var invite = GeneratePrivateRoomInvitation();
        //SendInvitation(invite);

        // clear stuff after using it 
        GameObject.Destroy(_invitePanel.currentOptionsPanel.UnityInstance);
        _invitePanel.currentOptionsPanel = null;
    }

    //public PrivateInvitation GeneratePrivateRoomInvitation()
    //{
    //    var privateInvitation = new PrivateInvitation()
    //    {
    //        Id = Guid.NewGuid(),
    //        ToPlayerId = _invitedPlayer.Id,
    //        ToPlayerName = String.Empty,
    //        RequestFulfilled = false,
    //        FromPlayerId = _invitePanel.MainPlayer.playerModel.Id,
    //        FromPlayerName = _invitePanel.MainPlayer.playerModel.Name,
    //        IsAccepted = false,
    //        RoomId = _invitePanel._roomTabsContainerObject.CurrentRoomGuid, 
    //    };

    //    return privateInvitation;
    //}

    //public void SendInvitation(PrivateInvitation invitation)
    //{
    //    _invitePanel.ClientCalls.InviteToChatRoom(invitation);

    //}
}