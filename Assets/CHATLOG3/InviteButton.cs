using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CHATLOG3
{
    public class InviteButton : PrefabScriptBase
    {
        [SerializeField] public Button Button;
        [SerializeField] private TextMeshProUGUI _buttonText;

        public string Text
        {
            get { return _buttonText.text; }
            set { _buttonText.text = value; }
        }

        //public async Task<ActionResult<ClientCallResult>> InviteToRoom(Guid fromId, Guid targetPlayer, Guid targetRoomId)

        public async UniTask Initialize(string playerName, Func<UniTask> inviteFunction)
        {
            _buttonText.text = playerName;
            Button.AddTaskFunc(inviteFunction);
        }
    }
}
