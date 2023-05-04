using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public class MessageBox : DialogBase
    {
        [SerializeField] public TextMeshProUGUI Text;
        [SerializeField] public Button OkButton;

        private void Start()
        {
        }

        public async UniTask Initialize(string text)
        {
            OkButton.AddTaskFunc(async () => await this.ResolveDialog(DialogResult.Ok));

            Text.text = text;
        }
    }
}
