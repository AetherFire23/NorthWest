using Cysharp.Threading.Tasks;
using Shared_Resources.GameTasks;
using Shared_Resources.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Dialogs.DIALOGSREFACTOR
{
    public class OptionsDialogScript2 : DialogBase
    {
        protected override string Name => nameof(OptionsDialogScript2);

        [SerializeField]
        public OptionsDialogObjects OptionsDialogObjects;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        private List<ToggleOption2> ToggleOptions { get; set; } = new();
        public List<ToggleOption2> ToggledOptions => ToggleOptions.Where(x => x.Toggled).ToList();
        private bool _allowMultipleChecks { get; set; }
        private int _minimumChecks { get; set; }
        private int _maximumChecks { get; set; }
        private Button _resolveButton => this.OptionsDialogObjects.ResolveButton;


        // initialized through dialogManager
        public async UniTask Initialize(string description, bool allowMultipleChecks, int minimumChecks, int maximumChecks) // must add minimum validation
        {
            _descriptionText.text = description;
            _resolveButton.AddTaskFunc(async () => await this.ValidateThenResolveDialog());
            _allowMultipleChecks = allowMultipleChecks;

            _minimumChecks = minimumChecks;
            _maximumChecks = maximumChecks;
        }

        // Update to prevent multiple checkcs from being made if multiple targets are allowed
        private void Update()
        {
            // janky behaviour. Will have to incorporate OnToggle or something and have to initialize the toggleOptions in the DIalog manager
            // The toggle method will save the current toggled option and if allowMUltiple = false, erase this last one to check the current checked one
            if (!_allowMultipleChecks && ToggledOptions.Count > 1)
            {
                ToggledOptions.First().Toggled = false;
            }

            if (_allowMultipleChecks && ToggledOptions.Count > _maximumChecks)
            {
                ToggledOptions.First().Toggled = false;
            }
        }

        public List<T> GetSelections<T>()
        {
            var selections = ToggleOptions.Where(x => x.Toggled).Select(x => (T)x.Option).ToList();
            return selections;
        }

        public List<object> GetRawSelections()
        {
            var selections = ToggledOptions.Select(x => x.Option).ToList();
            return selections;
        }

        public TaskParameters GetToggledAsParameters()
        {
            bool incorrectType = this.ToggledOptions.Any(x => x.Option is not ITaskParameter);
            if (incorrectType)
            {
                Debug.LogError("YOu tried to convert toggled options to parameters when the options do not inherit from ITaskParameter.");
                return new TaskParameters();
            }

            var valuePairs = ToggledOptions.Select(x => x.TaskOption.TaskParam).ToList();
            return new TaskParameters(valuePairs);
        }

        // replace button action with this
        private async UniTask ValidateThenResolveDialog()
        {
            bool isValid = ToggledOptions.Count >= _minimumChecks;
            if (!isValid) return; // could show a messageBox or what the behaviour is when not enough inputs have been entered

            await this.ResolveDialog(DialogResult.Ok);
        }
        // So I have to place the text panle, toggle, button in same vertical layout group for it to work.
        // If I just spawn a toggle inside the group, itll spawn AFTER the button, so it wont do
        // so I need to make every toggle spawn at buttonSibligIndex -1 
        // also not really before button because buttonIndex is moved when inited
        public void InsertToggleBeforeButton(ToggleOption2 optio)
        {
            this.ToggleOptions.Add(optio);

            int buttonIndex = OptionsDialogObjects.ResolveButton.gameObject.transform.GetSiblingIndex();
            optio.transform.SetSiblingIndex(buttonIndex);
        }
    }
}