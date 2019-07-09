using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected enum Direction { RIGHT, LEFT, UP, DOWN };
    protected Direction op = Direction.RIGHT;

    private Vector2 desiredPosition;
    public float moveFrequency;
    protected float counter;

    public List<GameObject> body;
    public GameObject newBody;
    int bodySize = 2;
    public List<Vector2> path;

    protected virtual void Start()
    {
        desiredPosition = transform.position;
        path.Add(body[1].transform.position);
        path.Add(body[2].transform.position);
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
        path.Add(desiredPosition);
        newBody = Instantiate(newBody, path[1], Quaternion.identity);
        body.Add(newBody);
        bodySize++;
    }

    private void EatFruit(GameObject fruit)
    {
        GameManager.gameState = GameManager.GameState.EATING;
        GameManager.fruitCounter++;
        GrowBody();
        moveFrequency *= 0.95f;
        Destroy(fruit);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Fruit"))
        {
            EatFruit(c.gameObject);
        }

        if (c.CompareTag("Body") || c.CompareTag("Wall"))
        {
            for (int i = 0; i < body.Count; i++)
            {
                body[i].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            }
            enabled = false;
        }
    }
}
