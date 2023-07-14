using Assets.AssetLoading;
using Assets.Dialogs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.MainMenu
{
    public class GuidLoading : MonoBehaviour
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private DialogManager _dialogManager;
        [SerializeField] private bool _disable;
        private async UniTask Awake()
        {
            //if (_disable) return;

            //await _prefabLoader.InitializeAsync();
            //var inputField = await _dialogManager.CreateDialog<AmountDialogScript>();
            //await inputField.Initialize("Enter the GUID");
            //await inputField.WaitForResolveCoroutine();

            //if (inputField.DialogResult != DialogResult.Ok)
            //{
            //    await inputField.Destroy();
            //    return;
            //}

            //TemporaryOptionsScript2.Instance.CurrentPlayerUID = inputField.GetTrimmedMessage();



            //SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
