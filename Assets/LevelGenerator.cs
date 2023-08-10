using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelGenerator : MonoBehaviour
{
    public bool[,] horizantal, vertical;
    public CinemachineVirtualCamera cam;
    public Transform Level;
    public GameObject Wall;
    public int w = 5, h = 5;
    public int x = 2, y = 2;

    public int initialPlayerX = 0, initialPlayerY = 0;
    // Call This Function On Start On Its Child Classes.
    protected void init()
    {
        foreach (Transform child in Level)
            if (child.tag != "Player")
                Destroy(child.gameObject);

        horizantal = new bool[w + 1, h];
        vertical = new bool[w, h + 1];
    }
}
