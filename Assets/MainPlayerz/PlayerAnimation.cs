using Assets.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator _animator; // From myPalyer
    [SerializeField] Rigidbody2D _rb; // From myPalyer
    [SerializeField] PlayerMovementScript _movementScript;
    public bool IsMoving => _movementScript.IsMoving;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving)
        {
            _animator.Play("Idle");
            return;
        }

        Direction direction = GetPlayerDirection();

        switch (direction)
        {
            case Direction.East:
                {
                    _animator.Play("WalkEast");
                    break;
                }
            case Direction.West:
                {
                    _animator.Play("WalkWest");
                    break;
                }
            case Direction.South:
                {
                    _animator.Play("WalkSouth");
                    break;
                }
            case Direction.North:
                {
                    _animator.Play("WalkNorth");
                    break;
                }
        }
    }

    public Direction GetPlayerDirection()
    {
        Vector2 playerPosition = this.gameObject.transform.position;
        Vector2 targetPosition = _movementScript.PointerInWorldPosition;
        return UnityExtensions.GetDirectionsBetweenPositions(playerPosition, targetPosition);
    }
}
