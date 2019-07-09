using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
        GameManager.fruitCounter = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
