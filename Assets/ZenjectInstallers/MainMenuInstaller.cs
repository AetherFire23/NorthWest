using Assets.Input_Management;
using Assets.InputAwaiter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NewInputManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<ClientCalls>().AsSingle();
        Container.BindInterfacesAndSelfTo<DialogManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputWaiting>().AsSingle();
    }
}
