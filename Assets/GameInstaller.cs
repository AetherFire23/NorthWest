using Assets;
using Assets.Buttons;
using Assets.ChatLog_Manager;
using Assets.ChatLog_Manager.Chat_Controller;
using Assets.ChatLog_Manager.Private_Rooms;
using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
using Assets.Dialogs;
using Assets.GameState_Management;
using Assets.InputAwaiter;
using Assets.Inventory;
using Assets.Inventory.ItemsUGF;
using Assets.OtherPlayers;
using Assets.RoomTransitioning;
using Assets.RoomTransitioning.Room_Instances;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<OtherCharactersManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<MainPlayer>().AsSingle();
        Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
        Container.BindInterfacesTo<PlayerMovement>().AsSingle();
        Container.Bind<PlayerInputState>().AsSingle();
        Container.Bind<InputWaiting>().AsSingle();

        Container.BindInterfacesAndSelfTo<InventoryManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryInputHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoomSwitchHandler>().AsSingle();

        

        Container.BindInterfacesTo<GameController>().AsSingle();


        Container.Bind<DialogManager>().AsSingle();

        Container.Bind<ClientCalls>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChatHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerPositionTick>().AsSingle();
        Container.BindInterfacesAndSelfTo<OpenCloseButtonUGF>().AsSingle();


        Container.BindInterfacesAndSelfTo<FirstRoomObject>().AsSingle();
        Container.BindInterfacesAndSelfTo<SecondRoomObject>().AsSingle();

        Container.BindInterfacesAndSelfTo<RoomHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PrivateRoomInviteHandler>().AsSingle();


        Container.BindInterfacesAndSelfTo<MessageService>().AsSingle();



        // Classes wrapping a Zenject binding
        Container.Bind<OtherCharactersObjectContainer>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoomTabBarUGF>().AsSingle();

        Container.Bind<ChatUGF>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInRoomPanelUGF>().AsSingle();
        Container.Bind<MessageCanvasUGF>().AsSingle();
        Container.Bind<ChatCanvasUGF>().AsSingle();

        Container.BindInterfacesAndSelfTo<MinusButtonUGF>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlusButtonUGF>().AsSingle();


        Container.BindInterfacesAndSelfTo<MainInviteButtonUGF>().AsSingle();
        Container.BindInterfacesAndSelfTo<MainInvitePanelUGF>().AsSingle();

        Container.BindInterfacesAndSelfTo<GlobalButtonUGF>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryPanelUGF>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoomInventoryUGF>().AsSingle();
    }
}