using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NONE, FORWARD, BACK, RIGHT, LEFT
};

public enum AnimationState { IDLE, UNDETERMINED, WIN }

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private Brick playerBrick;
    [SerializeField] private List<GameObject> collectedList = new();
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private GameObject chestFront;
    [SerializeField] private GameObject chestOpen;
    [SerializeField] private GameObject chestClose;
    [SerializeField] private BoxCollider winCondition;

    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask winLayer;

    private Direction swipeDirection;

    private Vector3 firstMouseClick, endMouseClick;
    private Vector3 targetPosition;
    private RaycastHit hit;

    private GameObject currentBrick;

    private bool isSwipping = true;
    private bool isMoving = false;

    void Update()
    {
        HandleInput();
        HandleRaycast();
    }

    private void HandleInput()
    {
        if (!isSwipping || isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            firstMouseClick = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endMouseClick = Input.mousePosition;
            swipeDirection = GetSwipeDirection();
            if (swipeDirection != Direction.NONE)
            {
                CheckLastBrick(swipeDirection);
            }
        }
    }

    private void HandleRaycast()
    {
        if (!isMoving) return;

        if (Physics.Raycast(targetPosition + Vector3.up * 0.5f, Vector3.down, out hit, brickLayer))
        {
            if (hit.collider != null && hit.collider.CompareTag("MoveBrick"))
            {
                currentBrick = hit.collider.gameObject;
                Move(swipeDirection, targetPosition);
            }
            else
            {
                ResetMovement();
            }
        }
        else
        {
            ResetMovement();
        }
    }

    private void ResetMovement()
    {
        isSwipping = true;
        isMoving = false;
    }

    // Method to caculate swipe direction
    private Direction GetSwipeDirection()
    {
        Vector3 swipeVector = endMouseClick - firstMouseClick;

        // Check if it is a swipe or tap
        if (swipeVector.magnitude < 50) return Direction.NONE;

        // Check swipe direction
        // If x > y
        return Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y)
            ? (swipeVector.x > 0 ? Direction.RIGHT : Direction.LEFT)
            : (swipeVector.y > 0 ? Direction.FORWARD : Direction.BACK);
    }

    // Method to caculate tartget position based on direction
    private Vector3 GetTargetPosition(Direction _direction)
    {
        Vector3 endPosition = transform.position;
        switch (_direction)
        {
            case Direction.FORWARD: Quaternion.Euler(0, 0, 0); return endPosition += Vector3.forward;
            case Direction.BACK: Quaternion.Euler(0, 0, 0); return endPosition += Vector3.back;
            case Direction.RIGHT: Quaternion.Euler(0, 0, 0); return endPosition += Vector3.right;
            case Direction.LEFT: Quaternion.Euler(0, -90, 0); return endPosition += Vector3.left;
            case Direction.NONE: Quaternion.Euler(0, 0, 0); return endPosition += Vector3.zero;
            default: return endPosition;
        }

    }

    private void Move(Direction _direction, Vector3 _endPosition)
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, _endPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _endPosition) < 0.1f)
        {
            isMoving = false;
            transform.position = _endPosition;
            CheckLastBrick(_direction);
        }
    }

    private void CheckLastBrick(Direction _direction)
    {
        if (_direction == Direction.NONE) return;

        Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, brickLayer);

        if (hit.collider != null) BrickOnGround(hit.collider.gameObject);
        targetPosition = GetTargetPosition(_direction);
        isMoving = true;

        CheckForWinCondition();
    }

    private void BrickOnGround(GameObject _brick)
    {
        if (_brick == null || collectedList.Contains(_brick)) return;

        bool isBrickActive = _brick.transform.GetChild(0).gameObject.activeSelf;

        if (isBrickActive)
        {
            playerBrick.AddBrick();
            ToggleBrickState(_brick, false);
        }
        else
        {
            playerBrick.RemoveBrick();
            ToggleBrickState(_brick, true);
        }

        collectedList.Add(_brick);
    }

    private void ToggleBrickState(GameObject _brick, bool _isActive)
    {
        if (_brick.CompareTag("MoveBrick"))
        {
            _brick.transform.GetChild(0).gameObject.SetActive(_isActive);
        }
    }

    private void CheckForWinCondition()
    {
        if (Physics.Raycast(targetPosition + Vector3.up * 0.5f, Vector3.down, out hit, winLayer))
        {
            Transform currentTransform = hit.collider.transform;

            while (currentTransform != null)
            {
                if (currentTransform.gameObject.CompareTag("Win"))
                {
                    isMoving = isSwipping = false;
                    playerBrick.ClearAllBrick();
                    CheckConditionActiveChest();
                    animatorPlayer.SetInteger("state", 2);
                    GameManager.Instance.WinGame();
                    return;
                }
                currentTransform = currentTransform.parent;
            }
        }
    }

    private void CheckConditionActiveChest()
    {
        transform.position = chestFront.transform.position;
        chestOpen.SetActive(true);
        chestClose.SetActive(false);
    }
}