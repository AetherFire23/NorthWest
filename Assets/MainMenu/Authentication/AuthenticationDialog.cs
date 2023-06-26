using Assets.Dialogs.DIALOGSREFACTOR;
using Assets.HttpStuff;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SerializeField] Calls _calls;

        public async UniTask Initialize()
        {
            LoginButton.AddMethod(TryLogin);
        }

        public async void TryLogin()
        {
            Debug.Log(UserNameInputField.GetTextWithoutHiddenCharacters());
            Debug.Log(PasswordInputField.GetTextWithoutHiddenCharacters());
            await ResolveDialog(DialogResult.Ok); // Error maybe ^ 
        }
    }
}
