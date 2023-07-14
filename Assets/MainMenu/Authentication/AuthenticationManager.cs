using Assets;
using System;
using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Assets.MainMenu.Authentication;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour // not stateHolder
{
    [SerializeField] private Canvas _authenticationCanvas;

    [SerializeField] private AuthenticationDialog _authenticationDialog;
    [SerializeField] private RegisterDialog _registerDialog;

    

    [SerializeField] private MainMenuCalls _menuClient;
    [SerializeField] private Button RegisterButton;
    [SerializeField] private TextMeshProUGUI _loginErrorText;
    [SerializeField] private TextMeshProUGUI _registerErrorText;
    [SerializeField] private TextMeshProUGUI _registerSucessText;
    [SerializeField] private Button ConfirmRegisterButton;

    ObservableBool _isRegisteringObservable = new ObservableBool(false);
    ObservableBool _hasLoginErrorObservable = new ObservableBool(false);
    ObservableBool _isAuthenticatedObservable = new ObservableBool(false);
    ObservableBool _hasRegisterError = new ObservableBool(false);

    public async UniTask WaitUntilAuthenticated()
    {
        await _authenticationDialog.Initialize();
        await _registerDialog.Initialize();
        await ConfigureObservables();
        await ConfigureButtonHandlers();
        await RunAuthenticateLoopUntilTokenAndUserIdAreRetrieved();
    }

    private async UniTask RunAuthenticateLoopUntilTokenAndUserIdAreRetrieved()
    {
        while (!_isAuthenticatedObservable.Value)
        {
            await _authenticationDialog.WaitForResolveCoroutine();
            LoginResult loginResult = await TryGetLoginToken(_authenticationDialog.GetLoginCredentials());

            if (!loginResult.IsSuccessful)
            {
                await HandleFailedTokenRequest();
                continue;
            }
            else
            {
                PersistenceModel.Instance.Token = loginResult.Token;
                PlayerInfo.UserId = loginResult.UserId;
                _isAuthenticatedObservable.Value = true; // breaksout
            }
        }

        // cleanup the WindowZ
         _authenticationCanvas.enabled = false;
    }

    private async UniTask OnOkRegisterResolve()
    {
        var clientCallResult = await _menuClient.Register(_registerDialog.GetRegisterRequest());

        if (!clientCallResult.IsSuccessful)
        {
            Debug.LogError($"Error duing authentication on the server-side {clientCallResult.Message}");
            _hasRegisterError.Value = true;
            _registerErrorText.text = clientCallResult.Message;
            _registerErrorText.SetVisibleAlphaLayer(true);
            _registerSucessText.SetVisibleAlphaLayer(false);
            _registerDialog.CleanupFields();
        }

        else
        {
            Debug.LogError("wil ssave to persistence");
            _registerSucessText.SetVisibleAlphaLayer(true);
            _registerErrorText.SetVisibleAlphaLayer(false);
        }
    }

    private async UniTask OnCancelRegisterResolve()
    {
        _registerDialog.ToggleCanvas(false);
        _registerDialog.CleanupFields();
        _authenticationDialog.ToggleCanvas(true);
    }

    private async UniTask<LoginResult> TryGetLoginToken(LoginRequest request)
    {
        var result = await _menuClient.GetLoginToken(request);
        LoginResult loginResult = result.DeserializeContent<LoginResult>();
        return loginResult;
    }

    private async UniTask HandleFailedTokenRequest()
    {
        Debug.Log("wrong creds i think");
        _hasLoginErrorObservable.Value = true;
        _authenticationDialog.Resolved = false;
    }

    private async UniTask ConfigureObservables()
    {
        _hasLoginErrorObservable.OnValueChanged += async (bool hasError) =>
        {
            _loginErrorText.SetVisibleAlphaLayer(hasError);
        };

        // open register canvas
        // might think of refactoring this to all use the same buttons i might have been stupid to try to make different buttons
        // juste make the Email field visible i dont know whatever
        _isRegisteringObservable.OnValueChanged += async (bool isRegistering) => // manages the open-closed states of windows and sets Cancel message to register dialog
        {
            RegisterButton.GetTextMesh().text = isRegistering ? "Back" : "Register";
            if (!isRegistering) // when its closed dont resolve shit ? 
            {
                await _registerDialog.ResolveDialog(DialogResult.Cancel);
            }
            else
            {
                _registerDialog.Resolved = false;
                _registerDialog.ToggleCanvas(true);
                _authenticationDialog.ToggleCanvas(false);
            }
        };

        _registerDialog.OnOkDialogResolve += OnOkRegisterResolve;
        _registerDialog.OnCancelDialogResolve += OnCancelRegisterResolve;
    }

    private async UniTask ConfigureButtonHandlers()
    {
        RegisterButton.AddMethod(() => // flicks the bool
        {
            _isRegisteringObservable.Value = !_isRegisteringObservable.Value;
        });
    }
}
