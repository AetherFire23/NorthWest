using Assets.INVENTORY3;
using UnityEngine;

public class LockMask : MonoBehaviour
{
    [SerializeField] InventoryStaticGameObjects _staticObjects;
    [SerializeField] private float _limit;
    [SerializeField] float resetPosition;
    public Vector3 _localPosition
    {
        get
        {
            return _staticObjects.RoomInventoryScrollView.transform.localPosition;
        }

        set
        {
            _staticObjects.RoomInventoryScrollView.transform.localPosition = value;
        }
    }
    private void Update()
    {
        ClampRightwardsInventoryMovement();
    }
    private void ClampRightwardsInventoryMovement()
    {
        if (_localPosition.x > _limit)
        {
            _localPosition = _localPosition.SetX(resetPosition);
        }
    }
}
