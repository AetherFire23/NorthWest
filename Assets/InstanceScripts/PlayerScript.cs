using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerScript : MonoBehaviour 
{
    [SerializeField] public Transform Transform;
    [SerializeField] public Rigidbody2D RigidBody;
    [SerializeField] public TextMeshProUGUI PlayerTextComponent;
    public Vector3 Position => this.transform.position;
    public Vector3 PlayerScreenPosition => Camera.main.WorldToScreenPoint(Position);

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        SetTextPositionOverHead();
    }

    public void SetTextPositionOverHead()
    {
        PlayerTextComponent.transform.position = PlayerScreenPosition.WithOffset(0, 8, 0); ;

    }

}
