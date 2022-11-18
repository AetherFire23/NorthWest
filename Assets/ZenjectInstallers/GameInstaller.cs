using Assets;
using Assets.Big_Tick_Energy;
using Assets.Buttons;
using Assets.ChatLog_Manager;
using Assets.ChatLog_Manager.Chat_Controller;
using Assets.ChatLog_Manager.Private_Rooms;
using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
using Assets.Dialogs;
using Assets.GameState_Management;
using Assets.Input_Management;
using Assets.InputAwaiter;
using Assets.Inventory;
using Assets.Inventory.ItemsUGF;
using Assets.Raycasts.NewRaycasts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();

        //Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
        //Container.BindInterfacesTo<PlayerMovement>().AsSingle();
        //Container.Bind<PlayerInputState>().AsSingle();
        Container.Bind<InputWaiting>().AsSingle();

        //Container.BindInterfacesAndSelfTo<InventoryManager>().AsSingle();
       // Container.BindInterfacesAndSelfTo<InventoryInputHandler>().AsSingle();

        

        Container.BindInterfacesTo<GameController>().AsSingle();


        Container.Bind<DialogManager>().AsSingle();

        Container.Bind<ClientCalls>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChatHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<OpenCloseButtonUGF>().AsSingle();
        //Container.BindInterfacesAndSelfTo<PrivateRoomInviteHandler>().AsSingle();


        Container.BindInterfacesAndSelfTo<MessageService>().AsSingle();



        // Classes wrapping a Zenject binding
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
        //Container.BindInterfacesAndSelfTo<InventoryPanelUGF>().AsSingle();
        //Container.BindInterfacesAndSelfTo<RoomInventoryUGF>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<NewInputManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<NewRayCaster>().AsSingle();


        Container.BindInterfacesAndSelfTo<GlobalTick>().AsSingle();
    }
}