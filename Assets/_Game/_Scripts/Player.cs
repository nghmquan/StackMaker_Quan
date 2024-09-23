using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float threshHold = 0.1f;
    [SerializeField] private float distancePlayer;
    private Vector3 startPosition, endPosition;
    private Direction direction;

    private void Update()
    {
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
            }

            endPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(GetDirection() != Vector3.zero)
            {
                Move(direction,endPosition);
            }
        }
    }

    private Vector3 GetDirection()
    {
        if(Mathf.Abs(endPosition.x - startPosition.x)> Mathf.Abs(endPosition.y - startPosition.y))
        {
            if(Mathf.Abs(endPosition.x - startPosition.x) > threshHold)
            {
                if(endPosition.x > startPosition.x)
                {
                    direction = Direction.RIGHT;
                    SetDirection(direction);
                }else if(endPosition.x < startPosition.x)
                {
                    direction = Direction.LEFT;
                    SetDirection(direction);
                }
            }else if(Mathf.Abs(endPosition.x - startPosition.x) < Mathf.Abs(endPosition.y - startPosition.y))
            {
                if(Mathf.Abs(endPosition.y - startPosition.y) > threshHold)
                {
                    if(endPosition.y > startPosition.y)
                    {
                        direction = Direction.FORWARD;
                        SetDirection(direction);
                    }
                    else if(endPosition.y < startPosition.y)
                    {
                        direction = Direction.BACK;
                        SetDirection(direction);
                    }
                }
            }
        }

        return Vector3.zero;
    }

    private void SetDirection(Direction _direction)
    {
        switch (_direction)
        {
            case Direction.FORWARD:
                transform.position = Vector3.forward;
                break;
            case Direction.BACK:
                transform.position = Vector3.back;
                break;
            case Direction.LEFT:
                transform.position = Vector3.left;
                break;
            case Direction.RIGHT:
                transform.position = Vector3.right;
                break;
        }
        Move(_direction, endPosition);
    }

    private void Move(Direction _direction, Vector3 _endPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, _endPosition, distancePlayer * Time.deltaTime);
        Debug.Log($"Moving {_direction} to {_endPosition}");
    }
}

public enum Direction
{
    FORWARD = 0,
    BACK = 1,
    LEFT = 2,
    RIGHT = 3
}
