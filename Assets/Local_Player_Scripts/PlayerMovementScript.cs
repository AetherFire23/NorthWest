using Assets.Input_Management;
using Assets.Raycasts.NewRaycasts;
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

    private NewInputManager _newInputManager;
    [Inject] private NewRayCaster _raycasts;

    public Vector2 PointerInWorldPosition => Camera.main.ScreenToWorldPoint(_newInputManager.PointerPosition);
    void Start()
    {

    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        // block le movement ici


        bool pressedOrHeld = _newInputManager.Pressed || _newInputManager.Held;
        if (!pressedOrHeld)
        {
            this.IsMoving = false;
            return;
        }

        if (MustBlockMovement()) return;


        this.IsMoving = true;

        Vector2 currentPosition = this.rb.position;
        float maxDistance = _moveSpeed * Time.deltaTime;
        Vector2 moveAmount = Vector3.MoveTowards(currentPosition, PointerInWorldPosition, maxDistance);
        this.rb.MovePosition(moveAmount);
    }

    public bool MustBlockMovement()
    {
        var raycasts = _raycasts.PointerRayAll();
        return raycasts.HasFoundHit;
    }

    [Inject]
    public void Construct(NewInputManager newInputManager)
    {
        _newInputManager = newInputManager;
    }
}
