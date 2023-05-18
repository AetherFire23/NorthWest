using Assets.OtherCharacters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherCharacterAS : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TextOverHead;
    [SerializeField] public TextMeshProUGUI ProfessionText;
    [SerializeField] public float professionHeight;
    private Vector2 PlayerScreenPosition => Camera.main.WorldToScreenPoint(this.transform.position);

    public OtherCharacterUGI SelfWrapper;
    void Start()
    {

    }

    void Update()
    {
        this.TextOverHead.transform.position = PlayerScreenPosition.WithOffset(0,45,0);
        this.ProfessionText.transform.position = PlayerScreenPosition.WithOffset(0, professionHeight, 0);
    }
}
