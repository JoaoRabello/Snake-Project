﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected enum Direction { RIGHT, LEFT, UP, DOWN };
    protected Direction op;

    private Vector2 desiredPosition;
    public float moveFrequency;
    private float counter;

    public List<GameObject> body;
    int bodySize = 2;
    public List<Vector2> path;

    protected virtual void Start()
    {
        desiredPosition = transform.position;
        path.Add(body[1].transform.position);
        path.Add(transform.position);
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
                    desiredPosition += Vector2.up;
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
        path.Add(transform.position);
        MoveBody();
        path.Remove(path[0]);
    }

    private void MoveBody()
    {
        for(int i = 1; i < body.Count; i++)
        {
            body[i].transform.position = path[i];
        }
    }

    protected void GrowBody()
    {
        GameObject newBody = Instantiate(body[1], path[1], Quaternion.identity);
        body.Add(newBody);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Fruit"))
        {
            GrowBody();
            Destroy(c.gameObject);
        }
    }
}