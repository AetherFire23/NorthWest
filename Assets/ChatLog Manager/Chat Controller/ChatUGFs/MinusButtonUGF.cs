
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using Zenject;


public class MinusButtonUGF : UGFWrapper, IInitializable
{
    public readonly MinusButtonObjectScript MinusButtonScript;
    private readonly RoomTabBarUGF _roomTabsContainerObject;
    public MinusButtonUGF(MinusButtonObjectScript minusButtonScript,
        RoomTabBarUGF roomTabsContainerObject) : base(minusButtonScript)
    {
        MinusButtonScript = minusButtonScript;
        _roomTabsContainerObject = roomTabsContainerObject;
    }

    public void Initialize()
    {
        this.MinusButtonScript.button.AddMethod(TurnOnOrOff);
    }

    public void TurnOnOrOff()
    {
        //var lastRoomTab = _roomTabsContainerObject.roomTabs.Last();
        //bool currentState = lastRoomTab.UnityInstance.active; // risque de causer problem e
        //lastRoomTab.UnityInstance.SetActive(!currentState); // je peux turn on or off un objet a partir dun zenject
        //lastRoomTab.UnityInstance.SelfDestroy();
        //_roomTabsContainerObject.roomTabs.Remove(lastRoomTab);
        // en fait faudrait remove le selected roomTab
    }
}
