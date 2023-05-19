using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNameAndProfessionScript : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _textOverHead;
    [SerializeField] public TextMeshProUGUI _professionText;
    [SerializeField] public float professionHeight;
    private Vector2 PlayerScreenPosition => Camera.main.WorldToScreenPoint(this.transform.position);

    void Update()
    {
        this._textOverHead.transform.position = PlayerScreenPosition.WithOffset(0, 45, 0);
        this._professionText.transform.position = PlayerScreenPosition.WithOffset(0, professionHeight, 0);
    }
}
