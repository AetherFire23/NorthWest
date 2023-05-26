using UnityEngine;

public class OtherCharacterMovement : MonoBehaviour
{
    public Vector2 TargetPosition;
    [SerializeField] public float MovementSpeed = 0.000003f;
    void Update()
    {

    }

    private void FixedUpdate()
    {
        var moveAmount = Vector2.MoveTowards(this.transform.position, this.TargetPosition, MovementSpeed * Time.deltaTime);
        this.transform.position = moveAmount;
    }
}
