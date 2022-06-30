using TMPro;
using UnityEngine;

public class PrefCube : MonoBehaviour
{
    public int needToUnLock;
    public Material DisabledMaterial;
    public TMP_Text txt;
    private float startScale = 100;
    private float currentScale;
    public bool bEnabled;
    public int currentNumber;

    private void OnMouseDown()
    {
        CubeTouched();
    }

    public void CubeTouched()
    {
        if (bEnabled)
        {
            PlayerPrefs.SetInt("ActiveCube", currentNumber);

            currentScale = startScale;

            //print(PlayerPrefs.GetInt("ActiveCube"));
        }
    }

    private void OnMouseExit()
    {
        if (bEnabled)
        {
            currentScale = startScale;
        }
    }

    private void OnMouseEnter()
    {
        if (bEnabled)
        {
            GetComponent<Animator>().enabled = false;
            currentScale = startScale * 0.8f;
        }
    }

    void Start()
    {
        currentScale = startScale;

        if (PlayerPrefs.GetInt("score") < needToUnLock)
        {
            bEnabled = false;
            GetComponent<MeshRenderer>().material = DisabledMaterial;
            txt.enabled = true;
        }
        else
        {
            bEnabled = true;
            txt.enabled = false;
        }
    }
    
    void Update()
    {  
        if (PlayerPrefs.GetInt("ActiveCube") == currentNumber)
        {            
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            GetComponent<Animator>().enabled = false;            
        }

        GetComponent<Transform>().localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}
