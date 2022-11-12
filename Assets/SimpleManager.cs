using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEditor;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;
using Assets.Raycasts;

public class SimpleManager : MonoBehaviour 
{
    private PlayerScript _playerFacade;
    private ClientCalls _clientCalls;
    private DialogManager _dialogManager;
    [Inject] ClientCalls clientCalls;
    public bool HasInitializedGame { get; set; } = false;
    SimpleTimer timer = new SimpleTimer(0.2f);
    [SerializeField] Vector3 offsetValue;
    async void Start() 
    {
        Screen.SetResolution(960, 720, false);
        var s = clientCalls.Test();
    }

    async void Update() 
    {
    }



    private void OnGUI()
    {

    }

    public void DebugLocalPlayer(Player model)
    {

    }

    [Inject]
    public void Construct(PlayerScript playerFacade, ClientCalls clientCalls, DialogManager dialogManager)
    {
        _playerFacade = playerFacade;
        _clientCalls = clientCalls;
        _dialogManager = dialogManager;
    }
}