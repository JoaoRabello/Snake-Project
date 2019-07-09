using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected enum Direction { RIGHT, LEFT, UP, DOWN };
    protected Direction op = Direction.RIGHT;

    #region Movimento
    private Vector2 desiredPosition;
    public float moveFrequency;
    protected float counter;
    #endregion

    #region Corpo
    public List<GameObject> body;
    public GameObject newBody;
    public List<Vector2> path;
    #endregion

    #region Sounds & Music
    public AudioClip eatSFX;
    public AudioClip gameOverSFX;
    public AudioSource sFXSource;
    public AudioSource musicSource;
    #endregion

    protected virtual void Start()
    {
        desiredPosition = transform.position;
        path.Add(body[1].transform.position);
        path.Add(body[2].transform.position);
        path.Add(transform.position);
    }

    #region Métodos de Movimento
    /// <summary>
    /// Modifica a posição desejada (desiredPosition) dependendo da direção obtida em PlayerControl e chama MoveTo(), passando esta posição.
    /// </summary>
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

    /// <summary>
    /// Muda a posição da cabeça da cobra para a posição desejada e adiciona a posição à lista path.
    /// </summary>
    /// <param desPos="desiredPosition"></param>
    void MoveTo(Vector2 desiredPosition)
    {
        transform.position = desiredPosition;
        path.Add(transform.position);
        MoveBody();
        path.Remove(path[0]);
    }

    /// <summary>
    /// Cada pedaço do corpo da cobra, exceto a cabeça, é levado a próxima posição na lista path.
    /// </summary>
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

    /// <summary>
    /// Rotaciona o objeto passado para um dos 4 sentidos (Up, Down, Left, Right).
    /// </summary>
    /// <param sprite="spt"></param>
    /// <param direction="dir"></param>
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
    #endregion

    #region Métodos de Pontuação
    /// <summary>
    /// Instancia um corpo na posição da fruta devorada e o adiciona a lista body.
    /// </summary>
    protected void GrowBody()
    {
        path.Add(desiredPosition);
        newBody = Instantiate(newBody, path[1], Quaternion.identity);
        body.Add(newBody);
    }

    /// <summary>
    /// Toca o SFX de comer, muda para o estado EATING do GameManager, cresce o corpo da cobra e aumenta sua velocidade. Por fim, destrói a fruta passada como parâmetro.
    /// </summary>
    /// <param fruit="fruit"></param>
    private void EatFruit(GameObject fruit)
    {
        sFXSource.PlayOneShot(eatSFX, 0.7F);
        GameManager.gameState = GameManager.GameState.EATING;
        GameManager.fruitCounter++;
        GrowBody();
        moveFrequency *= 0.95f;
        Destroy(fruit);
    }
    #endregion

    /// <summary>
    /// Muda o corpo da cobra para vermelho e toca o som de game over, indicando a derrota. Altera, também, o GameManager para o estado DEAD.
    /// </summary>
    private void Die()
    {
        for (int i = 0; i < body.Count; i++)
        {
            body[i].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
        musicSource.Stop();
        musicSource.clip = gameOverSFX;
        musicSource.loop = false;
        musicSource.Play();
        GameManager.gameState = GameManager.GameState.DEAD;
        enabled = false;
        
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
