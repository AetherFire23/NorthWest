//using Assets.Dialogs;
//using Assets.InputAwaiter;
//using Assets.MainMenu.UGIs;
//using Assets.Utils;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Unity.VisualScripting;
//using UnityEngine;
//using Zenject;

//public class FriendsManager : MonoBehaviour
//{
//    [SerializeField] public BasicButtonScript FriendButton;
//    [SerializeField] public GameObject Content;
//    [SerializeField] public BasicButtonScript AddButton;
//    [SerializeField] public BasicButtonScript RemoveButton;
//    [SerializeField] public Canvas FriendsCanvas;
//    [SerializeField] public Canvas InvitePanel;
//    private DialogManager _dialogManager;
//    private InputWaiting _inputWaiting;

//    async void Start()
//    {
//        FriendButton.ButtonComponent.AddMethod(() => this.FriendsCanvas.enabled = !this.FriendsCanvas.enabled);
//        AddButton.ButtonComponent.AddMethod(async () => await AskForFriend());

//    }

//    public void CreateOrDeleteFriendButton()
//    {

//    }

//    public async Task AskForFriend()
//    {
//        if (_dialogManager.IsWaitingForInput()) return;

//        _dialogManager.CreateDialog(DialogType.AmountDialog, "Give me a UserName to add.");
//        await _inputWaiting.WaitForResult();
//        var amoutdiag = _dialogManager.CurrentDialog as AmountDialogUGI;
//        string userName = amoutdiag.InputResult;

//        if (string.IsNullOrEmpty(userName))
//        {
//            _dialogManager.CreateDialog(DialogType.YesNoDialog, "Not a valid userID.");
//            await _inputWaiting.WaitForResult();
//        }
//        Debug.Log(userName);
//    }

//    [Inject]
//    public void Construct(DialogManager dialogManager,
//        InputWaiting inputWaiting)
//    {
//        _dialogManager = dialogManager;
//        _inputWaiting = inputWaiting;
//    }
//}
