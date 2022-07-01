using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 0, 0);
    public float cubeChangePlaceSpeed = 0.7f;
    public Transform cubeToPlace;
    private float camMoveYPosition, camMoveSpeed = 2f;
    public float camMoveZPosition;
    private CubePos startCamPosition;

    public GameObject[] cubesToCreate;
    public GameObject allCubes, vfx, platform;
    private Rigidbody allCubesRB;

    public Color[] bgColors;
    private Color toCameraColor;

    private Coroutine showCubeToPlace;
    public bool isLose = false, firstCube;
    //private int score = 0;
    
    public GameObject logo, tapToPlay, instaV, instaS, shop, restartButton;
    private Animator logoAnim, tapToPlayAnim, instaVAnim, instaSAnim, shopAnim;

    public TMP_Text Score, BestScore;

    private Transform mainCam;
    public AudioSource firstMusic, secondMusic;

    private List<CubePos> allCubesPositions = new List<CubePos>
    {
        //new CubePos(0, 1, 0)
    };

    private List<CubePos> possiblePossitions;
    private int currentPPIndex;

    private List<CubePos> possibleVectors = new List<CubePos>
    {
        new CubePos(1, 0, 0),
        new CubePos(-1, 0, 0),
        new CubePos(0, 1, 0),
        new CubePos(0, -1, 0),
        new CubePos(0, 0, 1),
        new CubePos(0, 0, -1),
    };

    private void Start()
    {
        if (PlayerPrefs.GetString("music") != "No")
            firstMusic.Play();

        mainCam = Camera.main.transform;
        startCamPosition.setVector(mainCam.position);

        camMoveYPosition = startCamPosition.y;
        camMoveZPosition = startCamPosition.z;

        allCubesRB = allCubes.GetComponent<Rigidbody>();

        toCameraColor = Camera.main.backgroundColor;
               
        var renderer = cubeToPlace.gameObject.GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("Transparent/Diffuse");
        renderer.material.color = cubesToCreate[PlayerPrefs.GetInt("ActiveCube")].GetComponent<Renderer>().sharedMaterial.color * 0.5f;

        var platformRenderer = platform.GetComponent<Renderer>();
        platformRenderer.material.color = cubesToCreate[PlayerPrefs.GetInt("ActiveCube")].GetComponent<Renderer>().sharedMaterial.color * 1f;

        RefreshPossiblePossitions();
        showCubeToPlace = StartCoroutine(ShowCubeToPlace());
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) 
                && cubeToPlace != null 
                && allCubes != null 
                && allCubesRB != null
                && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if(Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif
            
            if(!firstCube)
            {
                firstCube = true;

                logoAnim = logo.GetComponent<Animator>();
                logoAnim.Play("LogoPutAway");

                tapToPlayAnim = tapToPlay.GetComponent<Animator>();
                tapToPlayAnim.Play("TapToPlayPutAway");

                instaVAnim = instaV.GetComponent<Animator>();
                instaVAnim.Play("InstaVPutAway");

                instaSAnim = instaS.GetComponent<Animator>();
                instaSAnim.Play("InstaSPutAway");

                shopAnim = shop.GetComponent<Animator>();
                shopAnim.Play("ShopPutAway");

                Score.gameObject.SetActive(true);
                BestScore.gameObject.SetActive(true);

                if (PlayerPrefs.GetString("music") != "No")
                {
                    firstMusic.Stop();
                    secondMusic.Play();
                }
            }
            
            GameObject newCube = Instantiate(cubesToCreate[PlayerPrefs.GetInt("ActiveCube")], cubeToPlace.position, Quaternion.identity) as GameObject;
            
            newCube.transform.SetParent(allCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube);

            GameObject newVvfx = Instantiate(vfx, cubeToPlace.position, Quaternion.identity) as GameObject;
            Destroy(newVvfx, 1.5f);
            
            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();

            allCubesRB.isKinematic = true;
            allCubesRB.isKinematic = false;

            //allCubes.GetComponent<Rigidbody>().AddExplosionForce(1f, possibleVectors[UnityEngine.Random.Range(0, possibleVectors.Count)].GetVector(), 1f);
            allCubes.GetComponent<Rigidbody>().AddExplosionForce(10f, Vector3.down, 1f);

            //score++;
            cubeChangePlaceSpeed -= 0.005f;
            
            calcNewCameraPosChangeBg();
            RefreshPossiblePossitions();
            SpawnPositions();            
        }

        if(!isLose && ((allCubes != null
                        && allCubesRB != null 
                        && allCubesRB.velocity.magnitude > 0.2f 
                        && allCubes.transform.childCount > 0) || allCubes == null))
        {            
            isLose = true;
        }

        if(isLose && !restartButton.activeSelf)
        {
            Destroy(cubeToPlace.gameObject);

            StopCoroutine(showCubeToPlace);
            camMoveZPosition -= 5;
            camMoveYPosition = startCamPosition.y;

            restartButton.SetActive(true);
        }

        if (!CubePos.bEquals(mainCam.localPosition, new Vector3(mainCam.localPosition.x, camMoveYPosition, camMoveZPosition)))
        {
            mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
                new Vector3(mainCam.localPosition.x, camMoveYPosition, camMoveZPosition), camMoveSpeed * Time.deltaTime);
        }

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
    }

    IEnumerator ShowCubeToPlace()
    {
        while(true)
        {
            SpawnPositions();

            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        if (currentPPIndex != -1 && cubeToPlace != null)
        {
            cubeToPlace.position = possiblePossitions[currentPPIndex].GetVector();

            currentPPIndex = currentPPIndex == possiblePossitions.Count - 1 ? 0 : currentPPIndex + 1;
        }
        else
        {
            isLose = true;
        }
    }

    private void RefreshPossiblePossitions()
    {
        if (nowCube == new CubePos(0, 0, 0))
        {
            possiblePossitions = new List<CubePos>
            {
                new CubePos(1, 1, 0),
                new CubePos(-1, 1, 0),
                new CubePos(1, 1, 1),
                new CubePos(-1, 1, 1),
                new CubePos(1, 1, -1),
                new CubePos(-1, 1, -1),
                new CubePos(0, 1, 1),
                new CubePos(0, 1, -1),
                new CubePos(0, 1, 0),
            };
        }
        else
        {
            possiblePossitions = new List<CubePos>();

            foreach (CubePos possibleVect in possibleVectors)
            {
                if (IsPositionEmpty(nowCube + possibleVect))
                    possiblePossitions.Add(nowCube + possibleVect);
            }
        }

        currentPPIndex = possiblePossitions.Count == 0 ? -1 : 0;
    }

    private bool IsPositionEmpty(CubePos targetPos)
    {
        if (targetPos.y == 0)
            return false;

        foreach(CubePos pos in allCubesPositions)
        {
            if (pos == targetPos)
                return false;
        }

        return true;
    }

    private void calcNewCameraPosChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0;
        int maxCountHorizontal;

        for (int i = 0; i < allCubesPositions.Count; i++)
        {
            if (allCubes.transform.GetChild(i).gameObject.GetComponent<MyCube>().visible)
            { 
                if (Math.Abs(allCubesPositions[i].x) > maxX)
                    maxX = Math.Abs(allCubesPositions[i].x);

                if (Math.Abs(allCubesPositions[i].z) > maxZ)
                    maxZ = Math.Abs(allCubesPositions[i].z);
            }

            if (allCubesPositions[i].y > maxY)
                maxY = allCubesPositions[i].y;
        }

        if (PlayerPrefs.GetInt("score") < maxY)
            PlayerPrefs.SetInt("score", maxY);

        Score.text = $"Score: {maxY}";
        BestScore.text = $"Best Score: {PlayerPrefs.GetInt("score")}";

        maxCountHorizontal = maxX > maxZ ? maxX : maxZ;

        camMoveYPosition = startCamPosition.y + nowCube.y;
        camMoveZPosition = startCamPosition.z - (2 * (int)(maxCountHorizontal / 3));

        if(maxY <= 5)
            toCameraColor = bgColors[0];
        else if(maxY <= 10)
            toCameraColor = bgColors[1];
        else if (maxY <= 15)
            toCameraColor = bgColors[2];
        else if (maxY <= 20)
            toCameraColor = bgColors[3];
        else if (maxY <= 25)
            toCameraColor = bgColors[4];
        else if (maxY <= 30)
            toCameraColor = bgColors[5];
        else if (maxY <= 35)
            toCameraColor = bgColors[6];
        else if (maxY <= 40)
            toCameraColor = bgColors[7];
        else if (maxY <= 45)
            toCameraColor = bgColors[8];
        else if (maxY <= 50)
            toCameraColor = bgColors[9];
        else if (maxY <= 55)
            toCameraColor = bgColors[10];
    }
    
}

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void setVector(Vector3 vect)
    {
        x = Convert.ToInt32(vect.x);
        y = Convert.ToInt32(vect.y);
        z = Convert.ToInt32(vect.z);
    }


    public void addVector(Vector3 b)
    {
        x += Convert.ToInt32(b.x);
        y += Convert.ToInt32(b.y);
        z += Convert.ToInt32(b.z);
    }

    public void addX(int a)
    {
        x += a;
    }

    public void addY(int a)
    {
        y += a;
    }

    public void addZ(int a)
    {
        z += a;
    }

    public static CubePos operator +(CubePos a, Vector3 b)
    {        
        return new CubePos(a.x + Convert.ToInt32(b.x), a.y + Convert.ToInt32(b.y), a.z + Convert.ToInt32(b.z));
    }

    public static Vector3 operator +(Vector3 a, CubePos b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static CubePos operator +(CubePos a, CubePos b)
    {
        return new CubePos(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static bool operator ==(CubePos a, CubePos b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return true;
        else
            return false;
    }

    public static bool bEquals(Vector3 a, Vector3 b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return true;
        else
            return false;
    }

    public static bool operator !=(CubePos a, CubePos b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return false;
        else
            return true;
    }

    public override bool Equals(object b)
    {
        if (x == ((CubePos)b).x && y == ((CubePos)b).y && z == ((CubePos)b).z)
            return true;
        else
            return false;
    }
    public override int GetHashCode()
    {
        return 0;
    }
}
