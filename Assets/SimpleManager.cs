using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Proyecto26;
using UnityEditor;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;
using Assets.Raycasts;

public class SimpleManager : MonoBehaviour // chu rendu a fixer les instance (pt pas necessaire finalement de tout loader par binding 
{
    private PlayerScript _playerFacade;
    private MainPlayer _mainPlayer;
    private OtherCharactersManager _otherCharactersManager;
    private ClientCalls _clientCalls;
    private DialogManager _dialogManager;

    public bool HasInitializedGame { get; set; } = false;
    SimpleTimer timer = new SimpleTimer(0.2f);
    [SerializeField] Vector3 offsetValue;
    async void Start() 
    {
        Screen.SetResolution(960, 720, false);
    }

    async void Update() 
    {
    }

   
    public async UniTask InitializeLocalPlayer()
    {
        string playerName = "lolzida";

        PlayerModel model = _clientCalls.GetPlayerByName(playerName).AsTask().Result;

        _mainPlayer.playerModel = model;

        _playerFacade.gameObject.transform.position = new Vector3(model.X, model.Y, model.Z);

        _playerFacade.PlayerTextComponent.text = model.Name;

        DebugLocalPlayer(model);
        Debug.Log($"Just finished initializing localPlayer");
    }

    public async UniTask InitializeOtherCharacters()
    {
        _otherCharactersManager.InstantiateOtherCharacters(); // peyt etre async
        Debug.Log($"Finished initializing other characters");
    }

    private void OnGUI()
    {
        List<string> list = new List<string>();

        list.Add($"{this.timer._elapsedTIme}");
        list.Add($"");

        for (int i = 0; i < list.Count; i++)
        {
            GUI.Label(new Rect(50, i * 25, 100, 50), list[i]);
        }
    }

    public void DebugLocalPlayer(PlayerModel model)
    {
        if (model != null)
        {
            Debug.LogError($"I successfully accessed {model.Name} with position of : ({model.X},{model.Y},{model.Z})");
            Debug.Log($"I successfully accessed {model.Name} with position of : ({model.X},{model.Y},{model.Z})");
        }

        else if (model == null)
        {
            Debug.Log("I tried to access the local player but it did not work");
        }
    }

    [Inject]
    public void Construct(PlayerScript playerFacade, MainPlayer mainPlayer, OtherCharactersManager otherCharactersManager, ClientCalls clientCalls, DialogManager dialogManager)
    {
        _playerFacade = playerFacade;
        _mainPlayer = mainPlayer;
        _otherCharactersManager = otherCharactersManager;
        _clientCalls = clientCalls;
        _dialogManager = dialogManager;
    }
}