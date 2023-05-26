using Assets.Enums;
using UnityEngine;

public class OtherCharacterAnimation : MonoBehaviour
{
    [SerializeField] private OtherCharacterMovement _characterMovement;
    [SerializeField] private Animator _animator;
    public Vector2 TargetPosition => _characterMovement.TargetPosition;

    void Update()
    {
        bool IsMoving = this.TargetPosition != this.transform.position.ToVector2();

        if (!IsMoving)
        {
            _animator.Play("Idle");
            return;
        }
        var direction = UnityExtensions.GetDirectionsBetweenPositions(this.transform.position, TargetPosition);

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
}
