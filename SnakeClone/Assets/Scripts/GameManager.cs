﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { START, PLAYING, EATING, DEAD }
    public static GameState gameState;

    public GameObject groundOne;
    public GameObject groundTwo;

    [HideInInspector]
    public List<Vector2> grounds;
    [HideInInspector]
    public List<Vector2> walls;
    public GameObject wall;
    public GameObject borderWall;
    public int height;
    public int width;

    public GameObject fruit;
    public static int fruitCounter;
    public Text scoreText;

    private PlayerBehaviour player;
    public GameObject snakeHead;
    public GameObject snakeBody;
    public GameObject snakeTail;

    [SerializeField]
    private GameObject gameOverPanel;

    void Awake()
    {
        fruitCounter = 0;
        CreateWorld();
        SpawnSnakeRandomly();
        gameState = GameState.START;
    }


    void Update()
    {
        if(gameState == GameState.EATING || gameState == GameState.START)
        {
            SpawnFruit();
        }
        else
        {
            if(gameState == GameState.DEAD)
            {
                gameOverPanel.SetActive(true);
            }
        }

        scoreText.text = fruitCounter.ToString();
    }

    void SpawnFruit()
    {
        int randomIndex = Random.Range(0, grounds.Count);
        bool canSpawn = false;
        while (!canSpawn)
        {
            foreach (Vector2 pos in player.path)
            {
                if (grounds[randomIndex] == pos)
                {
                    randomIndex = Random.Range(0, grounds.Count);
                }
                else
                {
                    canSpawn = true;
                }
            }
        }
        Instantiate(fruit, grounds[randomIndex], Quaternion.identity);
        gameState = GameState.PLAYING;
    }
    
    void CreateWorld()
    {
        Vector2 pos = transform.position;
        int randomGround;
        for (int y = 0; y < height; y++)
        {
            pos.x = transform.position.x;
            for (int x = 0; x < width; x++)
            {
                if(y == 0)
                {
                    if(x == 0)
                    {
                        walls.Add(pos);
                        Instantiate(borderWall, pos, Quaternion.Euler(0f, 0f, 270f));  // bord esquerda superior
                    }
                    else
                    {
                        if (x == width - 1)
                        {
                            walls.Add(pos);
                            Instantiate(borderWall, pos, Quaternion.Euler(0f, 0f, 180f));  // bord direita superior
                        }
                        else
                        {
                            walls.Add(pos);
                            Instantiate(wall, pos, Quaternion.Euler(0f,0f,180f));
                        }
                    }
                }
                else
                {
                    if(y != height - 1)
                    {
                        if(x == 0)
                        {
                            walls.Add(pos);
                            Instantiate(wall, pos, Quaternion.Euler(0f, 0f, 270f));  //right wall
                        }
                        else
                        {
                            if(x == width - 1)
                            {
                                walls.Add(pos);
                                Instantiate(wall, pos, Quaternion.Euler(0f, 0f, 90f));  //left wall
                            }
                            else
                            {
                                grounds.Add(pos);
                            }
                        }
                    }
                    else
                    {
                        if(y == height - 1)
                        {
                            if (x == 0)
                            {
                                walls.Add(pos);
                                Instantiate(borderWall, pos, Quaternion.identity);  // bord esquerda inferior
                            }
                            else
                            {
                                if (x == width - 1)
                                {
                                    walls.Add(pos);
                                    Instantiate(borderWall, pos, Quaternion.Euler(0f, 0f, 90f));  // bord esquerda inferior
                                }
                                else
                                {
                                    walls.Add(pos);
                                    Instantiate(wall, pos, Quaternion.identity);
                                }
                            }
                        }
                    }
                }

                randomGround = Random.Range(1, 10);
                if (randomGround <= 5)
                {
                    Instantiate(groundOne, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(groundTwo, pos, Quaternion.identity);
                }
                pos.x += 1;
            }
            pos.y -= 1;
        }
    }

    void SpawnSnakeRandomly()
    {
        int randomPos = Random.Range(0, grounds.Count);
        snakeHead.transform.position = grounds[randomPos];
        snakeBody.transform.position = new Vector2(snakeHead.transform.position.x - 1, snakeHead.transform.position.y);
        snakeTail.transform.position = new Vector2(snakeBody.transform.position.x - 1, snakeBody.transform.position.y);
        player = snakeHead.GetComponent<PlayerBehaviour>();
    }
}
