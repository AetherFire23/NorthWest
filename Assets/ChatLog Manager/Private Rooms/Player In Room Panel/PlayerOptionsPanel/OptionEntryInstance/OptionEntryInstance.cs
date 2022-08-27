using Assets.ChatLog_Manager.Private_Rooms.InvitePanelObject.PlayerOptionsPanel.OptionEntryInstance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OptionEntryInstance : InstanceWrapper<OptionEntryInstanceScript> 
{
    private const string resourceName = "OptionEntryPrefab";
    public OptionEntryInstance(GameObject parent, OptionEntryInfo entryAction ) : base(resourceName, parent)
    {
        this.InstanceBehaviour.ButtonComponent.onClick.AddListener(delegate { entryAction.EntryAction(); }); // placeholder pour laction
        this.InstanceBehaviour.ButtonComponent.onClick.AddListener(delegate { DeleteSelf(); }); // placeholder pour laction
        this.InstanceBehaviour.TextButtonComponent.text = entryAction.Name;
        // dois rajouter un bouton pour inviter
        // dois rajouter un moyen pour deleter le panel 
        // delete for now
    }

    public void DeleteSelf()
    {
        GameObject.Destroy(this.UnityInstance);
    }
}