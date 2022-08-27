using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OtherCharacterBehaviour : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textComp;
    [SerializeField] public Rigidbody2D RigidBody;

    public Vector2 TargetPoint { get; set; }

    void Start()
    {
    }

    void Update()
    {
        SetTextPositionOverPlayer();
    }

    private void FixedUpdate()
    {
        MoveTowardsTargetPoint();
    }

    public void MoveTowardsTargetPoint()
    {
        Vector2 moveAmount = Vector3.MoveTowards(this.RigidBody.position, this.TargetPoint, Time.deltaTime * 10f);
        RigidBody.MovePosition(moveAmount);
    }

    public void SetTextPositionOverPlayer()
    {
        textComp.gameObject.transform.position = this.gameObject.transform.position.ToScreenPoint().WithOffset(0, 8, 0); //  sets the text component to the player gameobject with Y offset
    }
}
