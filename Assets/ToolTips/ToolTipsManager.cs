using Assets;
using Assets.Input_Management;
using Assets.Raycasts;
using Assets.Raycasts.NewRaycasts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

public class ToolTipsManager : MonoBehaviour // faudrait resize au start vu qe ca changera pas pendant le texte des tooltip
{ // Le raycast ne track pas le dernier item donc ca restart a cahque fois que je bouge-rebouge 

    [Inject] NewRayCaster _newRayCaster;
    [Inject] NewInputManager input;
    [SerializeField] RectTransform tooltipCanvasTransform;
    [SerializeField] Canvas tooltipCanvasComponent;

    private List<string> _tooltipableTags = new List<string>() { "Item", "PlayerTaskButton" };
    private List<int> _tooltipableLayers = new List<int>() { 6, };
    private TooltipUGI _tooltip;
    private float _timeOnToolTip;

    void Start()
    {

    }

    void Update()
    { // raouter le fait que ca doit prendre du temps 
        var rayResult = _newRayCaster.PointerUIRayCast(x => IsTooltipable(x.gameObject.tag, x.gameObject.layer));

        if (!rayResult.HasFoundHit)
        {
            _timeOnToolTip = 0f;

            if (_tooltip is not null)
            {
                _tooltip.UnityInstance.SelfDestroy();
                _tooltip = null;
            }
            return;
        }
        _timeOnToolTip += Time.deltaTime;

        if (_timeOnToolTip < 0.7f) return;
        if (_tooltip is not null) return;

        var toolInfo = rayResult.GameObject.GetComponentSafely<TooltipInfo>();

        var h = Screen.height;
        var w = Screen.width;

        _tooltip = new TooltipUGI(tooltipCanvasTransform.gameObject, toolInfo);

        // Si la tooltip depasse du screen remonte-la un peu

        //Les rect transform ne se calculent pas tout seul en les creatant
        Canvas.ForceUpdateCanvases();

        RectTransform rect = (RectTransform)_tooltip.UnityInstance.transform;
        // imparfait mais ca marche ahhaa
        var toolTipHeight = Math.Abs(rect.rect.y);
        var lowestPoint = input.PointerPosition.y - toolTipHeight;
    
        if (lowestPoint < 0)
        {
            var adjustedPosition = input.PointerPosition.y + (lowestPoint + toolTipHeight );
            _tooltip.Position = input.PointerPosition.SetY(adjustedPosition);
        }
        else
        {
            _tooltip.Position = input.PointerPosition;

        }

        _timeOnToolTip = 0f;
    }

    private void LateUpdate()
    {
        if (_tooltip is null) return;

        RectTransform rect = (RectTransform)_tooltip.UnityInstance.transform;
    }

    public List<string> s = new();
    private void OnGUI()
    {
        if (_tooltip is null) return;
        
        //RectTransform rect = (RectTransform)_tooltip.UnityInstance.transform;

        //rect.position = input.PointerPosition;
        //    s.Clear();
        //    string empt = string.Empty;
        //    var h = Screen.height;
        //    var w = Screen.width;

        //    s.Add(_tooltip.UnityInstance.transform.position.ToString());
        //    s.Add($"{input.PointerPosition}");
        //    s.Add($"size delta {_tooltip.UnityInstance.GetComponent<RectTransform>().sizeDelta.y.ToString()}");

        //    s.Add($"rectSize x : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.x}");
        //    s.Add($"rectSize y : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.y}");

        //    s.Add($"rectSize xmin : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.xMin}");
        //    s.Add($"rectSize ymin : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.yMin}");

        //    s.Add($"rectSize xmax : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.xMax}");
        //    s.Add($"rectSize ymax : {_tooltip.UnityInstance.GetComponent<RectTransform>().rect.yMax}");


        //    foreach (string info in s)
        //    {
        //        empt += $"{Environment.NewLine}{info}";
        //    }
        //    Rect r = new Rect(55, 55, 150, 350);
        //    GUI.TextArea(r, empt);

        //    s.Clear();

    }

    public bool IsTooltipable(string tag, int layer)
    {
        return _tooltipableTags.Contains(tag) || _tooltipableLayers.Contains(layer);
    }
}
