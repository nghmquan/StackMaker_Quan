using System.Collections.Generic;
using UnityEngine;

public enum Direction // Declare enum constants to identify movement directions
{
    NONE, FORWARD, BACK, RIGHT, LEFT
};

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Variable for speed object
    [SerializeField] private PlayerBrick playerBrick;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask winLayer;

    [SerializeField] private List<GameObject> collectedList = new();

    private Vector3 firstMouseClick, endMouseClick; // Variable for first mouse click and end mouse click
    private Vector3 targetPosition;
    private RaycastHit hit;
    private GameObject currentBrick;

    private Direction swipeDirection;


    private int moveCount = 0;
    private bool isSwipping = true;
    private bool isMoving = false;

    void Update()
    {
        if (isSwipping && !isMoving)
        {
            // If player when click left mouse mdown
            if (Input.GetMouseButtonDown(0))
            {
                firstMouseClick = Input.mousePosition; // Save mouse position when user click mouse position
            }


            // If player when click left mouse up
            if (Input.GetMouseButtonUp(0))
            {
                endMouseClick = Input.mousePosition; // Save mouse position when user release mouse click
                swipeDirection = GetSwipeDirection(); // Caculation swipe direction

                CheckLastBrick(swipeDirection);
            }
        }

        // Check raycast target position
        if (Physics.Raycast(targetPosition + Vector3.up * .5f, Vector3.down, out hit, brickLayer))
        {
            // if playr is moving and hit collided not null and hit collieded object with tag "MoveBrick"
            if (isMoving && hit.collider != null && hit.collider.CompareTag("MoveBrick"))
            {
                currentBrick = hit.collider.gameObject;
                //Method move is active with parameter swipe direction and target position
                Move(swipeDirection, targetPosition);
            }
            else
            {
                isSwipping = true;
                isMoving = false;
            }
        }
        else
        {
            isSwipping = true;
            isMoving = false;
            moveCount = 0;
        }

    }

    // Method to caculate swipe direction
    private Direction GetSwipeDirection()
    {
        Vector3 swipeVector = endMouseClick - firstMouseClick; // Caculate end mouse click and first mouse click

        // Check if it is a swipe or tap
        if (swipeVector.magnitude < 50)
        {
            return Direction.NONE; //Return none direction
        }

        // Check swipe direction
        // If x > y
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            return swipeVector.x > 0 ? Direction.RIGHT : Direction.LEFT; // Return left direction if x > 0 and vice versa
        }
        else //if y > 0
        {
            return swipeVector.y > 0 ? Direction.FORWARD : Direction.BACK; // Return foward direction if y > 0 and vice versa
        }
    }

    // Method to caculate tartget position based on direction
    private Vector3 GetTargetPosition(Direction _direction)
    {
        Vector3 endPosition = transform.position;
        switch (_direction)
        {
            case Direction.FORWARD:
                endPosition += Vector3.forward; //Forward direction
                break;
            case Direction.BACK:
                endPosition += Vector3.back; // Back direction
                break;
            case Direction.RIGHT:
                endPosition += Vector3.right; // Right direction
                break;
            case Direction.LEFT:
                endPosition += Vector3.left; // Left direction
                break;
            case Direction.NONE:
                endPosition += Vector3.zero; // Not moving
                break;
        }

        return endPosition;
    }

    // Method move object 
    private void Move(Direction _direction, Vector3 _endPosition)
    {
        // Handles moving to new position 
        transform.position = Vector3.MoveTowards(gameObject.transform.position, _endPosition, moveSpeed * Time.deltaTime);

        //When player move to end position with distance less than 0.1f
        if (Vector3.Distance(transform.position, _endPosition) < 0.1f)
        {
            isMoving = false; // return isMoving false
            transform.position = _endPosition;
            CheckLastBrick(_direction);
        }
    }

    //Method check player move to last way
    private void CheckLastBrick(Direction _direction)
    {
        //Check parameter direction not swipe screen
        if (_direction != Direction.NONE)
        {
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), Vector3.down, out hit, brickLayer);

            BrickOnGround(hit.collider.gameObject);

            targetPosition = GetTargetPosition(_direction); // Caculation tartget position based on swipe direction
            isMoving = true; // Return isMoving equal true
            moveCount++;
        }

        FinalPositionToPlayerWin();
    }
    private void TurnOffDimian(GameObject newBrick)
    {
        if (newBrick == null) return;
        if (newBrick.CompareTag("MoveBrick") == false) return;

        if (newBrick == null)
        {
            return;
        }
        newBrick.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void TurnOnDinin(GameObject _newBrick)
    {
        if (_newBrick == null) return;
        if (_newBrick.CompareTag("MoveBrick") == false) return;

        if (_newBrick == null)
        {
            return;
        }
        _newBrick.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void FinalPositionToPlayerWin()
    {
        if (Physics.Raycast(targetPosition + Vector3.up * .5f, Vector3.down, out hit, winLayer))
        {
            var playerWin = hit.collider.GetComponentInParent<PlayerWin>();
            if (playerWin != null)
            {
                isMoving = false;
                isSwipping = false;
                playerBrick.ClearAllBrick();
                playerWin.WhenPlayerWin();
                Debug.Log("Win");
            }
        }
    }

    private void BrickOnGround(GameObject _gameObject)
    {
        if (_gameObject != null && moveCount >= 0)
        {
            if (collectedList.Contains(_gameObject) == false)
            {
                if (_gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                {
                    playerBrick.AddBrick();
                    TurnOffDimian(_gameObject);
                }
                else if (_gameObject.transform.GetChild(0).gameObject.activeSelf == false)
                {
                    playerBrick.RemoveBrick();
                    TurnOnDinin(_gameObject);
                }
                collectedList.Add(_gameObject);
            }
        }
    }
}