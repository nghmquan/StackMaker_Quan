using UnityEngine;

public enum Direction // Declare enum constants to identify movement directions
{
    NONE, FORWARD, BACK, RIGHT, LEFT
};

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; // Variable for speed object
    [SerializeField] private LayerMask brickLayer;

    private Vector3 firstMouseClick, endMouseClick; // Variable for first mouse click and end mouse click
    private Vector3 targetPosition;

    private Direction swipeDirection;

    private bool isMoving = false;

    void Update()
    {

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, brickLayer))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //}

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
            
            if(swipeDirection != Direction.NONE)
            {
                targetPosition = GetTargetPosition(swipeDirection); // Caculation tartget position based on swipe direction
                isMoving = true;
            }
        }

        if (isMoving) 
        {
            Move(swipeDirection, targetPosition);
        }
    }

    // Fucntion to caculate swipe direction
    private Direction GetSwipeDirection()
    {
        Vector3 swipeVector = endMouseClick - firstMouseClick;
        if (swipeVector.magnitude < 50)  // Check if it is a swipe or tap   
            return Direction.NONE;

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

    // Function to caculate tartget position based on direction
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

    // Fucntion move object 
    private void Move(Direction _direction, Vector3 _endPosition)
    {

        // Handles moving to new position 
        transform.position = Vector3.MoveTowards(gameObject.transform.position, _endPosition, moveSpeed * Time.deltaTime);
    
        //When player move to end position with distance less than 0.1f
        if(Vector3.Distance(transform.position, _endPosition) < 0.1f)
        {
            isMoving = false; // return isMoving false
        }
    }
}