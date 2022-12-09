using Assets.Input_Management;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
public class CameraController : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] public float cameraHeight;

    [SerializeField] public float baseCameraSpeed;
    [SerializeField] public float scrollMoveSpeed;
    [SerializeField] public float clampedScrollMax;
    public float realcameraSpeed => baseCameraSpeed * Camera.main.orthographicSize;


    [Inject] NewInputManager input;
    private bool _mustMove = false;
    private PlayerScript _playerFacade;

    private void Update()
    {
        // move with WASD
        bool controlPressed = Input.GetKey(KeyCode.LeftControl);

        float moveBoost = controlPressed ? 2 : 1;

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position = this.transform.position.WithOffset(0, realcameraSpeed * Time.deltaTime * moveBoost, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position = this.transform.position.WithOffset(0, -realcameraSpeed * Time.deltaTime * moveBoost, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position = this.transform.position.WithOffset(-realcameraSpeed * Time.deltaTime * moveBoost, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position = this.transform.position.WithOffset(realcameraSpeed * Time.deltaTime * moveBoost, 0, 0);
        }

        // MouseScroll
        if (Input.mouseScrollDelta.y != 0f)
        {
            float scrollBoost = controlPressed ? 35 : 1;
            var scrollAmount = (Input.mouseScrollDelta.y * scrollMoveSpeed * Time.deltaTime * scrollBoost) * -1;
            var clampedScrollAmount = Mathf.Clamp(scrollAmount, -clampedScrollMax, clampedScrollMax);
            Camera.main.orthographicSize = Camera.main.orthographicSize + clampedScrollAmount;
        }

        // middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // trouver la DIRECTION inverse.
        }

        // 

        float clampedCameraSize = Mathf.Clamp(Camera.main.orthographicSize, 3, 10);
        Camera.main.orthographicSize = clampedCameraSize;

    }

    private void FixedUpdate()
    {
        //Vector2 cameraPosition = Camera.main.transform.position;
        //Vector2 playerPosition = this._playerFacade.Position;
        //var distanceDifference = Vector2.Distance(playerPosition, cameraPosition);
        //bool playerTooFar = distanceDifference > maxDistance;

        //if (playerTooFar && !_mustMove)
        //{
        //    _mustMove = true;
        //}

        //if (_mustMove)
        //{
        //    var moveAmount = Vector2.MoveTowards(cameraPosition, playerPosition, cameraMoveSpeed * Time.deltaTime);

        //    var nextCamPosition = new Vector3(moveAmount.x, moveAmount.y, -10);
        //    this.transform.position = nextCamPosition;

        //    if (this.transform.position.ToVector2() == this._playerFacade.RigidBody.position)
        //     {
        //        _mustMove = false;
        //    }
        //}

        Vector2 startPosition = new Vector2(50, 50);
        Vector2 endPosition = new Vector2(Screen.width - 50, Screen.height - 50);


        bool isContained = UnityExtensions.IsWithinBounds(startPosition, endPosition, Input.mousePosition);
       // Debug.Log($"contained : {isContained}, start{startPosition}, end {endPosition}, pp {Input.mousePosition}");
        if (isContained)
        {
            
        }




    }



    [Inject]
    public void Construct(PlayerScript playerFacade)
    {
        _playerFacade = playerFacade;
    }
}
