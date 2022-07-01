using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour
{
    public float speed;
    public Transform[] cubes;

    private void Start()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
    }
    private void Update()
    {
        foreach (Transform cube in cubes)
            cube.Rotate(0, 0.5f * speed * Time.deltaTime, speed * Time.deltaTime);                
    }
}
