
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public bool IsMoving { get; set; } = false; // aclled pour animations

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float _moveSpeed = 55;

    // private NewInputManager _newInputManager;
    // [Inject] private NewRayCaster _raycasts;

    private Vector2 PointerInWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public Vector2 _targetPosition = Vector2.zero;


    private void Update()
    {
        bool isPlayerAtTargetPosition = rb.position.Equals(_targetPosition);
        if (IsMoving && isPlayerAtTargetPosition)
        {
            IsMoving = false;
            // Debug.Log("Reached destination");
        }


        if (Input.GetMouseButtonDown(0) && !UIRaycast.Any())
        {
            IsMoving = true;
            _targetPosition = PointerInWorldPosition;
            return;
        }
    }


    private async void FixedUpdate()
    {
        if (!IsMoving) return;



        Vector2 currentPosition = this.rb.position;
        float maxMoveDistance = _moveSpeed * Time.deltaTime;
        Vector2 moveAmount = Vector3.MoveTowards(currentPosition, _targetPosition, maxMoveDistance);
        this.rb.MovePosition(moveAmount);
    }

    public async UniTask MoveCharacterCoroutine()
    {
        while (rb.position != _targetPosition)
        {
            Vector2 currentPosition = this.rb.position;
            float maxMoveDistance = _moveSpeed * Time.deltaTime;
            Vector2 moveAmount = Vector3.MoveTowards(currentPosition, _targetPosition, maxMoveDistance);
            this.rb.MovePosition(moveAmount);
            await UniTask.WaitForFixedUpdate();

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        this.IsMoving = false;
        _targetPosition = this.rb.position;
    }
}
