using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;

    private List<CubePos> allCubesPositions = new List<CubePos>
    {
        new CubePos(0, 1, 0)
    };

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
        StartCoroutine(ShowCubeToPlace());
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
        List<CubePos> positions = new List<CubePos>();

        foreach(CubePos possibleVect in possibleVectors)
        {
            if (IsPositionEmpty(nowCube + possibleVect))
                positions.Add(nowCube + possibleVect);
        }

        cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)].GetVector();
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

    public static bool operator !=(CubePos a, CubePos b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return false;
        else
            return true;
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
}
