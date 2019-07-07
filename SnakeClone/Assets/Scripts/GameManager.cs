using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { START, PLAYING, EATING, DEAD }
    public static GameState gameState = GameState.START;

    public GameObject lightGround;
    public GameObject darkGround;

    [HideInInspector]
    public List<Vector2> grounds;
    [HideInInspector]
    public List<Vector2> walls;
    public GameObject wall;
    public int linhas;
    public int colunas;

    public GameObject fruit;

    void Awake()
    {
        CreateWorld();
    }

    void Update()
    {
        if(gameState == GameState.EATING || gameState == GameState.START)
        {
            SpawnFruit();
        }
    }

    void SpawnFruit()
    {
        int randomIndex = Random.Range(0, grounds.Count);
        Instantiate(fruit, grounds[randomIndex], Quaternion.identity);
        gameState = GameState.PLAYING;
    }
    
    void CreateWorld()
    {
        bool white = true;
        Vector2 pos = transform.position;
        for (int y = 0; y < linhas; y++)
        {
            pos.x = transform.position.x;
            for (int x = 0; x < colunas; x++)
            {
                if((y == 0 || y == linhas - 1)|| (x == 0 || x == colunas - 1))
                {
                    walls.Add(pos);
                    Instantiate(wall, pos, Quaternion.identity);
                }
                else
                {
                    grounds.Add(pos);
                }

                if (white)
                {
                    Instantiate(lightGround, pos, Quaternion.identity);
                    white = false;
                }
                else
                {
                    Instantiate(darkGround, pos, Quaternion.identity);
                    white = true;
                }
                pos.x += 1;
            }
            pos.y -= 1;
        }
    }
}
