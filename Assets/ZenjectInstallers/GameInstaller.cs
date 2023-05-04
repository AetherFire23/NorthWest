//using Assets;
//using Assets.Big_Tick_Energy;
////using Assets.Buttons;
//using Assets.ChatLog_Manager;
////using Assets.ChatLog_Manager.Chat_Controller;
//using Assets.ChatLog_Manager.Private_Rooms;
//using Assets.ChatLog_Manager.Private_Rooms.NewInviteSystem;
//using Assets.Dialogs;
//using Assets.GameState_Management;
//using Assets.Input_Management;
//using Assets.InputAwaiter;
//using Assets.Inventory;
//using Assets.Inventory.ItemsUGF;
//using Assets.Raycasts.NewRaycasts;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Zenject;

//public class GameInstaller : MonoInstaller
//{
//    public override void InstallBindings()
//    {
//        // forcing the program to receive gamestate since virtually everything depends on that. Is launched non-lazily from the zen installer.

//        //Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
//        Container.Bind<ClientCalls>().AsSingle();
//        Container.Bind<GameStateFetcher>().AsSingle();



//        //Container.Bind<InputWaiting>().AsSingle();
//        //Container.Bind<DialogManager>().AsSingle();
//        //Container.BindInterfacesAndSelfTo<NewInputManager>().AsSingle();
//        //Container.BindInterfacesAndSelfTo<NewRayCaster>().AsSingle();

//    }


//    private static void RegisterTaskTypes()
//    {

//    }
//}