using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class CameraController2 : MonoBehaviour
    {
        [SerializeField] float maxDistance;
        [SerializeField] public float cameraHeight;

        [SerializeField] public float baseCameraSpeed;
        [SerializeField] public float scrollMoveSpeed;
        [SerializeField] public float clampedScrollMax;
        public float realcameraSpeed => baseCameraSpeed * Camera.main.orthographicSize;
        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.position = this.transform.position.WithOffset(0, realcameraSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                this.transform.position = this.transform.position.WithOffset(0, -realcameraSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.A))
            {
                this.transform.position = this.transform.position.WithOffset(-realcameraSpeed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                this.transform.position = this.transform.position.WithOffset(realcameraSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
