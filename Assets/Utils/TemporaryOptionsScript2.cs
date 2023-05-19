using System;
using UnityEngine;

public class TemporaryOptionsScript2 : MonoBehaviour
{
    private static TemporaryOptionsScript2 instance;

    [SerializeField] private string currentPlayerUID;
    public Guid CurrentPlayerID => new Guid(currentPlayerUID);

    [SerializeField] public string BenUID; // ben
    [SerializeField] public string FredUID; // fred

    private void Awake()
    {
        // If an instance already exists, destroy this instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton instance
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static TemporaryOptionsScript2 Instance
    {
        get { return instance; }
    }
}