using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MainMenu.Authentication
{
    public class AuthenticationDialog : DialogBase
    {
        protected override string Name => nameof(AuthenticationDialog);

        [SerializeField] private TMP_InputField UserNameInputField;
        [SerializeField] TMP_InputField PasswordInputField;
        [SerializeField] Button LoginButton;
        [SerializeField] Button RegisterButton;

        // non-standalone calls
        [SerializeField] MainMenuCalls _calls;

        public string UserName { get => UserNameInputField.GetTextWithoutHiddenCharacters(); set => UserNameInputField.text = value; }
        public string Password { get => PasswordInputField.GetTextWithoutHiddenCharacters(); set => PasswordInputField.text = value; }

        public async UniTask Initialize()
        {
            LoginButton.AddTaskFunc(async () => await ValidateInputThenResolve());
        }

        public async UniTask ValidateInputThenResolve()
        {
            if (UserName.Equals(string.Empty) || Password.Equals(string.Empty))
            {
                Debug.Log("empty input not allowed");
                return;
            }
            await ResolveDialog(DialogResult.Ok); // Error maybe ^ 
        }

        public LoginRequest GetLoginCredentials()
        {
            var loginReq = new LoginRequest()
            {
                UserName = UserName,
                PasswordAttempt = Password,
            };
            return loginReq;
        }

        public void ResetInputFieldTexts()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
