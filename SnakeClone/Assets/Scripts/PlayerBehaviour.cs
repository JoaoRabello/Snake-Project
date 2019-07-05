using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected enum Direction { RIGHT, LEFT, UP, DOWN };
    protected Direction op;

    private Vector2 desiredPosition;
    public float moveFrequency;
    private float counter;

    protected virtual void Start()
    {
        desiredPosition = transform.position;
    }

    protected void SwitchDirection()
    {
        if (counter >= moveFrequency)
        {
            counter = 0;

            switch (op)
            {
                case Direction.UP:
                    desiredPosition += Vector2.up;
                    break;
                case Direction.DOWN:
                    desiredPosition += Vector2.down;
                    break;
                case Direction.LEFT:
                    desiredPosition += Vector2.left;
                    break;
                case Direction.RIGHT:
                    desiredPosition += Vector2.right;
                    break;
                default:
                    desiredPosition += Vector2.right;
                    break;
            }
            MoveTo(desiredPosition);
        }
        else
        {
            counter += Time.deltaTime;
        }
    }

    void MoveTo(Vector2 desiredPosition)
    {
        transform.position = desiredPosition;
    }
}
