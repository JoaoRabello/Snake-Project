using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    /// <summary>
    /// Carrega a scene com o jogo per si
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Fecha o jogo
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
