using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipResizer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI HeaderText;
    [SerializeField] public TextMeshProUGUI BodyText;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int _maxLength = 18;

    private void Start()
    {
        int headerLength = HeaderText.text.Length;
        int bodyLength = BodyText.text.Length;
        bool exceedsLength = headerLength > _maxLength
            || bodyLength > _maxLength;

        if (exceedsLength)
        {
            _layoutElement.enabled = true;
        }
        else
        {
            _layoutElement.enabled = false;
        }
    }
    void Update()
    {
        int headerLength = HeaderText.text.Length;
        int bodyLength = BodyText.text.Length;
        bool exceedsLength = headerLength > _maxLength
            || bodyLength > _maxLength;

        if (exceedsLength)
        {
            _layoutElement.enabled = true;
        }
        else
        {
            _layoutElement.enabled = false;
        }
    }
}
