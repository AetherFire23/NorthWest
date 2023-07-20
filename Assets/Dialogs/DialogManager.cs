using Assets.AssetLoading;
using Assets.Dialogs.DIALOGSREFACTOR;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        // lets make it a multi-window ?
        [SerializeField] private RectTransform _dialogCanvas;
        [SerializeField] private PrefabLoader _prefabLoader;

        List<DialogBase> _dialogs = new List<DialogBase>();

        /// <summary> Do not forget to initialize the window after calling this. </summary>
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

        public async UniTask<OptionsDialogScript2> CreateAndInitializeOptionsDialog2(string description, List<object> options, bool allowMultipleChecks, int minimumChecks, int maximumChecks)
        {
            var optionsDialog = await _prefabLoader.CreateInstanceOfAsync<OptionsDialogScript2>(_dialogCanvas.gameObject);
            await optionsDialog.Initialize(description, allowMultipleChecks, minimumChecks, maximumChecks);

            // create the ToggleOptions from outside the optionsDialog because I don't have reference to the prefabloader inside of classes... Maybe I should make it a singleton?
            // SHould verify if this is an async hazard
            foreach (var option in options)
            {
                var toggleOption = await _prefabLoader.CreateInstanceOfAsync<ToggleOption2>(optionsDialog.OptionsDialogObjects.TogglesPanel.gameObject);
                await toggleOption.Initialize(option);
                optionsDialog.InsertToggleBeforeButton(toggleOption);
            }

            return optionsDialog;
        }
    }
}
