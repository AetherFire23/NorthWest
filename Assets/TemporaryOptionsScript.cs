using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryOptionsScript : MonoBehaviour
{
    [SerializeField] private string CurrentPlayerUID;

    public Guid CurrentPlayerID => new Guid(CurrentPlayerUID);

    [SerializeField] public string BenUID; // ben
    [SerializeField] public string FredUID; // fred

    //[SerializeField] public Guid BenUID = new Guid("B3543B2E-CD81-479F-B99E-D11A8AAB37A0"); // ben
    //[SerializeField] public Guid FredUID = new Guid("7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F"); // fred
}
