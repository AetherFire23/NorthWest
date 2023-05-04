using Assets.Input_Management;
using Assets.Raycasts.NewRaycasts;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementScript : MonoBehaviour
{
    public bool IsMoving = false;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float _moveSpeed = 55;

    // private NewInputManager _newInputManager;
    // [Inject] private NewRayCaster _raycasts;

    private Vector2 PointerInWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public Vector2 _targetPosition = Vector2.zero;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsMoving = true;
            _targetPosition = PointerInWorldPosition;
        }

        bool isPlayerAtTargetPosition = rb.position.Equals(_targetPosition);
        if (IsMoving && isPlayerAtTargetPosition)
        {
            IsMoving = false;
            Debug.Log("Reached destination");
        }
    }


    private async void FixedUpdate()
    {
        if (!IsMoving) return;



        Vector2 currentPosition = this.rb.position;
        float maxMoveDistance = _moveSpeed * Time.deltaTime;
        Vector2 moveAmount = Vector3.MoveTowards(currentPosition, _targetPosition, maxMoveDistance);
        this.rb.MovePosition(moveAmount);

    }

    public async UniTask MoveCharacterCoroutine()
    {
        while (rb.position != _targetPosition)
        {
            Vector2 currentPosition = this.rb.position;
            float maxMoveDistance = _moveSpeed * Time.deltaTime;
            Vector2 moveAmount = Vector3.MoveTowards(currentPosition, _targetPosition, maxMoveDistance);
            this.rb.MovePosition(moveAmount);
            await UniTask.WaitForFixedUpdate();

        }

    }

    //public bool MustBlockMovement()
    //{
    //    var raycasts = _raycasts.PointerRayAll();
    //    return raycasts.HasFoundHit;
    //}
}
