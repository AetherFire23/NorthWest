using Assets.Dialogs;
using Assets.Input_Management;
using Assets.InputAwaiter;
using Assets.MainMenu;
using Assets.Raycasts;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuPersistence _mainMenuPersistence;
    private string MainSceneName = "SampleScene";
    private DialogManager _dialogManager;
    private NewInputManager _inputState;
    private InputWaiting _inputWaiting;

    async UniTask Start()
    {
        //_dialogManager.CreateDialog(DialogType.AmountDialog, "Enter your user id please !!!!!!!!!!!!!!");
        //await _inputWaiting.WaitForResult();
        //var dialog = (AmountDialogUGI)_dialogManager.CurrentDialog;
        //Guid receivedId = new Guid(dialog.InputResult);
        //Debug.Log(receivedId);
        //_mainMenuPersistence.receivedId = dialog.InputResult;
        //_mainMenuPersistence.MainPlayerId = receivedId;

        //SceneManager.LoadScene(MainSceneName);
    }
    
    void Update()
    {

        

    }

    [Inject]
    public void InitInjections(DialogManager dialogManager, NewInputManager manager, InputWaiting inputWaiting)
    {
        _dialogManager = dialogManager;
        _inputState = manager;
        _inputWaiting = inputWaiting;
    }
}
