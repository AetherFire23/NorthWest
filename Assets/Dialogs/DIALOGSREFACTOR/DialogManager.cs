using Assets.AssetLoading;
using Assets.Dialogs.New_System;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public class DialogManager : MonoBehaviour
    {
        // lets make it a multi-window ?
        [SerializeField] RectTransform _dialogCanvas;
        [SerializeField] PrefabLoader _prefabLoader;

        List<DialogBase> _dialogs = new List<DialogBase>();

        /// <summary>
        /// Do not forget to initialize the window after calling this.
        /// </summary>
        public async UniTask<T> CreateDialog<T>() where T : DialogBase
        {
            var dialog = await _prefabLoader.CreateInstanceOfAsync<T>(_dialogCanvas.gameObject);
            _dialogs.Add(dialog);
            return dialog;
        }

        public async UniTask DeleteDialog(DialogBase dialog)
        {
            _dialogs.Remove(dialog);
            GameObject.Destroy(dialog.gameObject);
        }

        // could maybe use reflection to force special implementation through this method
        // this dialog needs to spawn some more Toggles but it does load the prefab loader...
        // maybe I could have the selectionDialog be always in the 
        /// <summary>
        /// Dont forget to override ToString();
        /// </summary>
        public async UniTask<OptionsDialogScript> CreateAndInitializeOptionsDialog(List<object> options, bool allowMultipleChecks, int minimumChecks, int maximumChecks)
        {
            var optionsDialog = await _prefabLoader.CreateInstanceOfAsync<OptionsDialogScript>(_dialogCanvas.gameObject);
            await optionsDialog.Initialize(allowMultipleChecks, minimumChecks, maximumChecks);

            // create the ToggleOptions from outside the optionsDialog because I don't have reference to the prefabloader inside of classes... Maybe I should make it a singleton?
            // SHould verify if this is an async hazard
            foreach (var option in options)
            {
                var toggleOption = await _prefabLoader.CreateInstanceOfAsync<ToggleOption>(optionsDialog.OptionsDialogObjects.TogglesPanel.gameObject);
                await toggleOption.Initialize(option);
                optionsDialog.ToggleOptions.Add(toggleOption);
            }

            return optionsDialog;
        }
    }
}
