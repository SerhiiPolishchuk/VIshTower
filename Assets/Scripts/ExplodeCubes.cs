using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject restartButton;
    public Transform mainCamera;

    private bool _collisionSet = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cube" && !_collisionSet)
        {
            for(int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, Vector3.up, 15f);
                child.SetParent(null);
            }

            restartButton.SetActive(true);
            mainCamera.localPosition += new Vector3(0, 0, -3);

            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }
}
