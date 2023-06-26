using UnityEngine;

namespace Assets.ExitMenu
{
    public class OpenScapeMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;


        private bool _isEscapePressed => Input.GetKeyDown(KeyCode.Escape);
        private void Update()
        {
            if (!_isEscapePressed) return;

            _canvas.enabled = !_canvas.enabled;
        }
    }
}