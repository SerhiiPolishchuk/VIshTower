using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Sound")
            GetComponent<Image>().sprite = musicOff;
    }
    public void RestartGame()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        StartCoroutine(Wait(0.5f));       
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
    }

    public void LoadInstagramV()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        Application.OpenURL("https://www.instagram.com/ishchenko4433/");
    }

    public void LoadInstagramS()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        Application.OpenURL("https://www.instagram.com/serg.life/");
    }

    public void musicWork()
    {
        if(PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<Image>().sprite = musicOn;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
