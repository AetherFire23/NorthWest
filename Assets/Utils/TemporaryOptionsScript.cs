using System;
using UnityEngine;

public class TemporaryOptionsScript : MonoBehaviour
{
    [SerializeField] private string CurrentPlayerUID;

    public Guid CurrentPlayerID => new Guid(CurrentPlayerUID);

    [SerializeField] public string BenUID; // ben
    [SerializeField] public string FredUID; // fred
}
