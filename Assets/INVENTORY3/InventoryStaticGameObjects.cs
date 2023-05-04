using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.INVENTORY3
{
    public class InventoryStaticGameObjects : MonoBehaviour
    {
        // player-side
        [SerializeField] public Button PlayerInventoryButton;

        /// <summary>
        /// insert slots here
        /// </summary>
        [SerializeField] public RectTransform PlayerInventoryPanel;
        [SerializeField] public Canvas PlayerInventoryCanvas;

        // room-side
        [SerializeField] public Button RoomInventoryButton;
        [SerializeField] public RectTransform RoomInventoryScrollView;
        [SerializeField] public Mask ScrollviewMask;
        [SerializeField] public ScrollRect ScrollRect;
        [SerializeField] public Canvas RoomInventoryCanvas;
        [SerializeField] public TextMeshProUGUI RoomNameText;
    }
}
