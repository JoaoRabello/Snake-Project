﻿using System.Collections.Generic;
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

    [SerializeField]
    private AudioClip eatSFX;
    public AudioSource audioSource;
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
                    RotateSprite(body[0], "Up");
                    break;
                case Direction.DOWN:
                    desiredPosition += Vector2.down;
                    RotateSprite(body[0], "Down");
                    break;
                case Direction.LEFT:
                    desiredPosition += Vector2.left;
                    RotateSprite(body[0], "Left");
                    break;
                case Direction.RIGHT:
                    desiredPosition += Vector2.right;
                    RotateSprite(body[0], "Right");
                    break;
                default:
                    desiredPosition += Vector2.up;
                    RotateSprite(body[0], "Up");
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
            if(body[i].transform.position.x - path[i].x < 0)
            {
                RotateSprite(body[i], "Right");
            }
            else
            {
                if(body[i].transform.position.x - path[i].x > 0)
                {
                    RotateSprite(body[i], "Left");
                }
                else
                {
                    if (body[i].transform.position.y - path[i].y < 0)
                    {
                        RotateSprite(body[i], "Up");
                    }
                    else
                    {
                        if (body[i].transform.position.y - path[i].y < 0)
                        {
                            RotateSprite(body[i], "Down");
                        }
                    }
                }
            }
            body[i].transform.position = path[i];
        }
    }

    protected void RotateSprite(GameObject spt, string dir)
    {
        switch (dir)
        {
            case "Up":
                spt.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case "Down":
                spt.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                break;
            case "Left":
                spt.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case "Right":
                spt.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
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
        audioSource.PlayOneShot(eatSFX, 0.7F);
        GameManager.gameState = GameManager.GameState.EATING;
        GameManager.fruitCounter++;
        GrowBody();
        moveFrequency *= 0.95f;
        Destroy(fruit);
    }

    private void Die()
    {
        for (int i = 0; i < body.Count; i++)
        {
            body[i].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
        enabled = false;
        GameManager.gameState = GameManager.GameState.DEAD;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Fruit"))
        {
            EatFruit(c.gameObject);
        }

        if (c.CompareTag("Body") || c.CompareTag("Wall"))
        {
            Die();
        }
    }
}
