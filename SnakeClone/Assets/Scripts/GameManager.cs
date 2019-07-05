using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject whiteGround;
    public GameObject blackGround;

    [HideInInspector]
    public List<Vector2> ground;
    public int linhas = 2;
    public int colunas = 2;

    void Awake()
    {
        CreateWorld();
    }

    void Update()
    {
        
    }

    void CreateWorld()
    {
        bool white = true;
        Vector2 pos = transform.position;
        for (int x = 0; x < linhas; x++)
        {
            pos.x = transform.position.x;
            for (int y = 0; y < colunas; y++)
            {
                ground.Add(pos);

                if (white)
                {
                    Instantiate(whiteGround, pos, Quaternion.identity);
                    white = false;
                }
                else
                {
                    Instantiate(blackGround, pos, Quaternion.identity);
                    white = true;
                }
                pos.x += 1;
            }
            pos.y -= 1;
        }
    }
}
