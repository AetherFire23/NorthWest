using Assets.HttpStuff;
using Assets.MainMenu.Authentication;
using Assets.Scratch;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.Requests;
using TMPro;
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
    [SerializeField] private Button OpenRegisterButton;
    [SerializeField] private TextMeshProUGUI _errorText;

    ObservableBool _isRegisteringObservable = new ObservableBool(false);
    ObservableBool _hasLoginErrorObservable = new ObservableBool(false);
    ObservableBool _isAuthenticatedObservable = new ObservableBool(false);

    public async UniTask WaitUntilAuthenticated()
    {
        await _authenticationDialog.Initialize();
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
                _isAuthenticatedObservable.Value = true;
            }
        }
    }

    private async UniTask<(bool Success, string Token)> TryGetLoginToken(LoginRequest request)
    {
        var result = await _menuClient.GetLoginToken(request);
        var sucessToken = (result.IsSuccessful, result.Content as string ?? string.Empty);
        return sucessToken;
    }

    private async UniTask HandleFailedTokenRequest()
    {
        Debug.Log("wrong password i think");
        _hasLoginErrorObservable.Value = true;
        _authenticationDialog.Resolved = false;
    }

    private async UniTask ConfigureObservables()
    {
        _hasLoginErrorObservable.OnValueChanged += async (bool hasError) =>
        {
            _errorText.IsVisibleAlphaLayer(hasError);
        };

        // open register canvas
        _isRegisteringObservable.OnValueChanged += async (bool isRegistering) =>
        {
            _registerDialog.ToggleCanvas(isRegistering); // show when registering
            _authenticationDialog.ToggleCanvas(!isRegistering); // hide when registering
            OpenRegisterButton.GetTextMesh().text = isRegistering ? "Back" : "Register";
        };
    }

    private async UniTask ConfigureButtonHandlers()
    {
        OpenRegisterButton.AddMethod(() => // flicks the bool
        {
            _isRegisteringObservable.Value = !_isRegisteringObservable.Value;
        });
    }
}
