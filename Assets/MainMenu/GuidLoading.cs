using Assets.AssetLoading;
using Assets.Dialogs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MainMenu
{
    public class GuidLoading : MonoBehaviour
    {
        [SerializeField] private PrefabLoader _prefabLoader;
        [SerializeField] private DialogManager _dialogManager;

        private async UniTask Awake()
        {
            await _prefabLoader.InitializeAsync();
            var inputField = await _dialogManager.CreateDialog<AmountDialogScript>();
            await inputField.Initialize("Enter the GUID");
            await inputField.WaitForResolveCoroutine();

            if (inputField.DialogResult != DialogResult.Ok)
            {
                await inputField.Destroy();
                return;
            }

            TemporaryOptionsScript2.Instance.CurrentPlayerUID = inputField.GetTrimmedMessage();



            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
