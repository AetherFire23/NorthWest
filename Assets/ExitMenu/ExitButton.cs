using UnityEngine;
using UnityEngine.UI;

namespace Assets.ExitMenu
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private void Awake()
        {
            _button.AddMethod(() => Application.Quit());
        }
    }
}
