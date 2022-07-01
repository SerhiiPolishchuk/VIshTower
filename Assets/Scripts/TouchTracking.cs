using UnityEngine;

public class TouchTracking : MonoBehaviour
{
    void Update()
    {        
        if(Input.touchCount != 1)
        {
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if(touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Cube")
                {
                    PrefCube cube = hit.collider.gameObject.GetComponent<PrefCube>();

                    cube.CubeTouched();
                }
            }
        }
    }
}
