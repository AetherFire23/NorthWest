using UnityEngine;

namespace Assets
{
    public class CameraController2 : MonoBehaviour
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] public float CameraHeight;

        [SerializeField] public float BaseCameraSpeed;
        [SerializeField] public float ScrollMoveSpeed;
        [SerializeField] public float ClampedScrollMax;

        public float RealCameraSpeed => BaseCameraSpeed * Camera.main.orthographicSize;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = transform.position.WithOffset(0, RealCameraSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position = transform.position.WithOffset(0, -RealCameraSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.position = transform.position.WithOffset(-RealCameraSpeed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.position = transform.position.WithOffset(RealCameraSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
