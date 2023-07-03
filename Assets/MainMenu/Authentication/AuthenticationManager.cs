using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Assets.MainMenu.Authentication;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.DTOs;
using Shared_Resources.Models;
using Shared_Resources.Models.Requests;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour // not stateHolder
{
    // player can register during authentication loop. 
    // logic 
    [SerializeField] private MainMenuCalls _menuClient;
    // views
    [SerializeField] private AuthenticationDialog _authenticationDialog;
    [SerializeField] private RegisterDialog _registerDialog;
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
        await AuthenticateLoop();
    }

    private async UniTask AuthenticateLoop()
    {
        while (!_isAuthenticatedObservable.Value)
        {
            await _authenticationDialog.WaitForResolveCoroutine();
            (bool Success, string Token) successToken = await TryGetLoginToken(_authenticationDialog.GetLoginCredentials());

            if (!successToken.Success)
            {
                await HandleFailedTokenRequest();
                continue;
            }
            else
            {
                PersistenceModel.Instance.Token = successToken.Token;
                _isAuthenticatedObservable.Value = true; // breaksout
            }
        }
    }

    private async UniTask OnOkRegisterResolve()
    {
        // try login
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

    private async UniTask<(bool Success, string Token)> TryGetLoginToken(LoginRequest request)
    {
        var result = await _menuClient.GetLoginToken(request);
        var sucessToken = (result.IsSuccessful, result.Content as string ?? string.Empty);
        return sucessToken;
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
