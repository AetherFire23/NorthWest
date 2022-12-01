using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherCharacterAS : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TextOverHead;
    [SerializeField] public OtherCharacterMovement CharacterMovement;
    [SerializeField] public TextMeshProUGUI ProfessionText;
    [SerializeField] public float professionHeight;
    private Vector2 PlayerScreenPosition => Camera.main.WorldToScreenPoint(this.transform.position);
    void Start()
    {
        
    }

    void Update()
    {
        this.TextOverHead.transform.position = PlayerScreenPosition.WithOffset(0,45,0);
        this.ProfessionText.transform.position = PlayerScreenPosition.WithOffset(0, professionHeight, 0);
    }
}
