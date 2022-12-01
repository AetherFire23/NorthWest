using Assets.Input_Management;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ToolTipResizer : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI HeaderText;
    [SerializeField] public TextMeshProUGUI BodyText;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int _maxLength = 18;
    
    bool initialized = false;

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

        //if (initialized) return;
        //var rect = this.gameObject.GetComponent<RectTransform>();
        //var rectHeight = Math.Abs(rect.rect.y);
        //var lowestPoint = _input.PointerPosition.y - Math.Abs(rectHeight);

        //bool mustAdjust = lowestPoint < 0;
        //if(mustAdjust)
        //{
        //    var absPoint = Math.Abs(lowestPoint);
        //    float remainder = Screen.height % absPoint;

        //}

        //initialized = true;
    }
}
