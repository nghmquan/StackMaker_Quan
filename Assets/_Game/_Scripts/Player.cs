using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;
    public float minSwipeDistance = 50f; // Minimum distance for a swipe
    public float moveDistance = 1f; // Distance the player moves on swipe

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
        }

        // Handle touch input for mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Horizontal swipe
                if (swipeDelta.x > 0)
                    Move(Direction.RIGHT);
                else
                    Move(Direction.LEFT);
            }
            else
            {
                // Vertical swipe
                if (swipeDelta.y > 0)
                    Move(Direction.FORWARD);
                else
                    Move(Direction.BACK);
            }
        }
    }

    void Move(Direction _direction)
    {

        Vector3 _endPosition = transform.position;

        switch (_direction)
        {
            case Direction.FORWARD:
                _endPosition += Vector3.forward * moveDistance;
                break;
            case Direction.BACK:
                _endPosition += Vector3.back * moveDistance;
                break;
            case Direction.LEFT:
                _endPosition += Vector3.left * moveDistance;
                break;
            case Direction.RIGHT:
                _endPosition += Vector3.right * moveDistance;
                break;
        }
        var collider = Collided(_endPosition);
        if(collider != null)
        {
            Move(_direction, collider.transform.position);
        }
        // Call Move function with direction and end position
        
    }

    void Move(Direction _direction, Vector3 _endPosition)
    {
        // Perform the actual movement logic, such as animation or position update
        transform.position = _endPosition;
        Debug.Log("Moved " + _direction + " to position " + _endPosition);
        Move(_direction);
    }
    Collider Collided(Vector3 _endPostion)
    {
        var direction = _endPostion - transform.position;
        direction = direction.normalized + Vector3.down * .5f;
        Debug.Log(direction);
        Physics.Raycast(transform.position, direction, out var hitInfo);
        var collider = hitInfo.collider;
        if(collider != null)
        {
            Debug.Log("Collider is not null: " + collider.name);
        }
        else
        {
            Debug.Log("Collider is null");
        }
        Debug.DrawRay(transform.position, direction, Color.red, 5);
        return collider;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.forward);
    }
}

public enum Direction
{
    FORWARD = 0,
    BACK = 1,
    LEFT = 2,
    RIGHT = 3
}
