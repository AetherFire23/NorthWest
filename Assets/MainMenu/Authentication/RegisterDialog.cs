using Assets.Dialogs.DIALOGSREFACTOR;
using Cysharp.Threading.Tasks;
using Shared_Resources.Models.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MainMenu.Authentication
{
    public class RegisterDialog : DialogBase // not implement yet
    {
        protected override string Name => nameof(RegisterDialog);

        [SerializeField] private TMP_InputField UserNameInputField;
        [SerializeField] private TMP_InputField PasswordInputField;
        [SerializeField] private TMP_InputField EmailField;

        [SerializeField] private Button AcceptCredentialsButton;
        //[SerializeField] MainMenuCalls _calls;

        public string UserName { get => UserNameInputField.GetTextWithoutHiddenCharacters(); set => UserNameInputField.text = value; }
        public string Password { get => PasswordInputField.GetTextWithoutHiddenCharacters(); set => PasswordInputField.text = value; }
        public string Email { get => EmailField.GetTextWithoutHiddenCharacters(); set => EmailField.text = value; }

        public async UniTask Initialize()
        {
            base.DialogResult = DialogResult.No;
            AcceptCredentialsButton.AddTaskFunc(async () => await ValidateInputThenResolve());
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

        public RegisterRequest GetRegisterRequest()
        {
            var loginRequest = new RegisterRequest()
            {
                UserName = this.UserName,
                Password = this.Password,
                Email = this.Email,
            };

            return loginRequest;
        }
        public void CleanupFields()
        {
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }
    }
}
