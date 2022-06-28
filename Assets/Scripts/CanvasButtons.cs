using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadInstagramV()
    {
        Application.OpenURL("https://www.instagram.com/ishchenko4433/");
    }

    public void LoadInstagramS()
    {
        Application.OpenURL("https://www.instagram.com/serg.life/");
    }
}
