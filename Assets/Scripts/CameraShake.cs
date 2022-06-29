using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDur = 0.6f, shakeAmount = 0.04f, decreaseFactor = 1.5f;


    void Start()
    {
        camTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if(shakeDur > 0)
        {
            camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDur = 0;
        }
    }
}
