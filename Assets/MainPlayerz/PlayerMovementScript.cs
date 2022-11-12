using Assets.Input_Management;
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

    public Vector2 PointerInWorldPosition => Camera.main.ScreenToWorldPoint(_newInputManager.PointerPosition);
    void Start()
    {

    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        bool pressedOrHeld = _newInputManager.Pressed || _newInputManager.Held;
        if (!pressedOrHeld)
        {
            this.IsMoving = false;
            return;
        }

        this.IsMoving = true;

        Vector2 currentPosition = this.rb.position;
        float maxDistance = _moveSpeed * Time.deltaTime;
        Vector2 moveAmount = Vector3.MoveTowards(currentPosition, PointerInWorldPosition, maxDistance);
        this.rb.MovePosition(moveAmount);
    }

    [Inject]
    public void Construct(NewInputManager newInputManager)
    {
        _newInputManager = newInputManager;
    }
}
