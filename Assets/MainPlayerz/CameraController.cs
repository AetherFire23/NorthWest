using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class CameraController : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] public float cameraHeight;
    private bool _mustMove = false;
    private PlayerScript _playerFacade;


    private void FixedUpdate()
    {
        Vector2 cameraPosition = Camera.main.transform.position;
        Vector2 playerPosition = this._playerFacade.Position;
        var distanceDifference = Vector2.Distance(playerPosition, cameraPosition);
        bool playerTooFar = distanceDifference > maxDistance;

        if (playerTooFar && !_mustMove)
        {
            _mustMove = true;
        }

        if (_mustMove)
        {
            var moveAmount = Vector2.MoveTowards(cameraPosition, playerPosition, cameraMoveSpeed * Time.deltaTime);

            var nextCamPosition = new Vector3(moveAmount.x, moveAmount.y, -10);
            this.transform.position = nextCamPosition;

            if (this.transform.position.ToVector2() == this._playerFacade.RigidBody.position)
             {
                _mustMove = false;
            }
        }
    }

    

    [Inject]
    public void Construct(PlayerScript playerFacade)
    {
        _playerFacade = playerFacade;
    }
}
