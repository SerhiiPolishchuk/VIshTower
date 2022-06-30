using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    public AudioSource firstMusic, secondMusic;
    public GameObject gc;

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

        SceneManager.LoadScene("Shop");
    }

    public void CloseShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Main");
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

            if (SceneManager.GetActiveScene().name == "Shop")
                firstMusic.Play();
            else
            {
                if (!gc.GetComponent<GameController>().firstCube)
                    firstMusic.Play();
                else
                    secondMusic.Play();
            }
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;

            if (SceneManager.GetActiveScene().name == "Shop")
                firstMusic.Stop();
            else
            {
                if (!gc.GetComponent<GameController>().firstCube)
                    firstMusic.Stop();
                else
                    secondMusic.Stop();
            }
        }
    }
}
