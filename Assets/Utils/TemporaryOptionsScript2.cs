using System;
using UnityEngine;

public class TemporaryOptionsScript2 : MonoBehaviour
{
    private static TemporaryOptionsScript2 instance;

    [SerializeField] public string CurrentPlayerUID;

    [SerializeField] public string BenUID; // ben
    [SerializeField] public string FredUID; // fred

    // 7E7B80A5-D7E2-4129-A4CD-59CF3C493F7F
    // B3543B2E-CD81-479F-B99E-D11A8AAB37A0
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