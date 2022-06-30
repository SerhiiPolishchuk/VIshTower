using System;
using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject explosion, restartButton;
    public Transform mainCamera;

    private bool _collisionSet = false;
        
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet)
        {
            GameObject newVvfx = Instantiate(explosion, collision.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(newVvfx, 2.5f);

            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();

            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.up, 15f);
                child.SetParent(null);
            }

            //restartButton.SetActive(true);

            Camera.main.gameObject.AddComponent<CameraShake>();

            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }

}
