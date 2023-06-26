using Assets.MainMenu.Authentication;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    [SerializeField]
    private AuthenticationDialog _authenticationDialog;

    [SerializeField]
    private RegisterDialog _registerDialog;

    public bool IsUserLoggedIn { get; set; } = false;

    async UniTask Start()
    {
        await _authenticationDialog.Initialize();
        
    }

    private void Update()
    {
        if (IsUserLoggedIn)
        {
            _authenticationDialog.ToggleCanvas(false);
            _registerDialog.ToggleCanvas(false);
        }
    }
}
