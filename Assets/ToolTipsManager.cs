using Assets;
using Assets.Input_Management;
using Assets.Raycasts;
using Assets.Raycasts.NewRaycasts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;

public class ToolTipsManager : MonoBehaviour // faudrait resize au start vu qe ca changera pas pendant le texte des tooltip
{ // Le raycast ne track pas le dernier item donc ca restart a cahque fois que je bouge-rebouge 
    
    [Inject] NewRayCaster _newRayCaster;
    [Inject] NewInputManager input;
    [SerializeField] RectTransform tooltipCanvas;

    private List<string> _tooltipableTags = new List<string>() { "Item", };
    private List<int> _tooltipableLayers = new List<int>() { 6, };
    private TooltipUGI _tooltip;

    void Start()
    {

    }

    void Update()
    { // raouter le fait que ca doit prendre du temps 
        var rayResult = _newRayCaster.PointerUIRayCast(x => IsTooltipable(x.gameObject.tag, x.gameObject.layer));
        if (!rayResult.HasFoundHit)
        {
            if(_tooltip is not null)
            {
                _tooltip.UnityInstance.SelfDestroy();
                _tooltip = null;
            }
            return;
        }

        if (_tooltip is not null) return;

        var toolInfo = rayResult.GameObject.GetComponentSafely<TooltipInfo>();
        _tooltip = new TooltipUGI(tooltipCanvas.gameObject, toolInfo);
        _tooltip.UnityInstance.transform.position = input.PointerPosition.WithOffset(15, -15,0);
    }

    public bool IsTooltipable(string tag, int layer)
    {
        return _tooltipableTags.Contains(tag) || _tooltipableLayers.Contains(layer);
    }
}
