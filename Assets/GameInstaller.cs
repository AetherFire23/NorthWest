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

        Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle();

        Container.Bind<DialogManager>().AsSingle();

        Container.Bind<ClientCalls>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChatHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerPositionTick>().AsSingle();
        Container.BindInterfacesAndSelfTo<InitializeButtons>().AsSingle();


        Container.BindInterfacesAndSelfTo<FirstRoomObject>().AsSingle();
        Container.BindInterfacesAndSelfTo<SecondRoomObject>().AsSingle();

        Container.BindInterfacesAndSelfTo<RoomHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PrivateRoomInviteHandler>().AsSingle();


        Container.BindInterfacesAndSelfTo<MessageService>().AsSingle();



        // Classes wrapping a Zenject binding
        Container.Bind<OtherCharactersObjectContainer>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoomTabBar>().AsSingle();

        Container.Bind<ChatWrapper>().AsSingle();
        Container.BindInterfacesAndSelfTo<InvitePanel>().AsSingle();
        Container.Bind<MessageCanvasObject>().AsSingle();
        Container.Bind<ChatCanvasWrapper>().AsSingle();

        Container.BindInterfacesAndSelfTo<MinusButtonObject>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlusButtonObject>().AsSingle();


        Container.BindInterfacesAndSelfTo<MainInviteButtonObject>().AsSingle();
        Container.BindInterfacesAndSelfTo<MainInvitePanelObject>().AsSingle();

        Container.BindInterfacesAndSelfTo<GlobalButtonObject>().AsSingle();

    }
}