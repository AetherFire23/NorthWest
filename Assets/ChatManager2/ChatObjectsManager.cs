using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatObjectsManager : MonoBehaviour
{
    // Before chat is opened
    [SerializeField] public Button OpenChatButton;
    [SerializeField] Canvas ChatCanvas;

    // Chat is opened
    [SerializeField] public GameObject ScrollviewContent;
    [SerializeField] public GameObject PlayerInRoomContainer;
    [SerializeField] public TMP_InputField InputField;

    //RoomTabs
    [SerializeField] public GameObject TabsContainer;
    [SerializeField] public Button GlobalButton;
    [SerializeField] public Button AddRoomButton;
    [SerializeField] public Button LeaveRoomButton;
    // invite panel
    [SerializeField] public Button OpenInvitePanel;
    [SerializeField] public GameObject InvitePanel;
    void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        this.OpenChatButton.AddMethod(() => this.ChatCanvas.enabled = !this.ChatCanvas.enabled);
        this.OpenInvitePanel.AddMethod(() => this.InvitePanel.SetActive(!this.InvitePanel.active));
    }
}
