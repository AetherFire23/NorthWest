using Assets.Dialogs.DIALOGSREFACTOR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.MainMenu.Authentication
{
    public class RegisterDialog : DialogBase
    {
        protected override string Name => nameof(RegisterDialog);

        [SerializeField] private TMP_InputField UserNameInputField { get; set; }
        [SerializeField] private TMP_InputField PasswordInputField { get; set; }

    }
}
