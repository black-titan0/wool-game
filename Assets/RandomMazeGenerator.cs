using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMazeGenerator : LevelGenerator
{
    public float probability;

    void Start()
    {
        this.init();
        var state = new int[w, h];
        void DFS(int x, int y)
        {
            state[x, y] = 1;

            var directions = new[]
            {
                (x - 1, y, horizantal, x, y, Vector3.right, 90),
                (x + 1, y, horizantal, x + 1, y, Vector3.right, 90),
                (x, y - 1, vertical, x, y, Vector3.up, 0),
                (x, y + 1, vertical, x, y + 1, Vector3.up, 0),
            };
            foreach (var (nx, ny, wall, wx, wy, sh, ang) in directions.OrderBy(d => Random.value))
                if (!(0 <= nx && nx < w && 0 <= ny && ny < h) || (state[nx, ny] == 2 && Random.value > probability))
                {
                    wall[wx, wy] = true;
                    Instantiate(Wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), Level);
                }
                else if (state[nx, ny] == 0) DFS(nx, ny);

            state[x, y] = 2;
        }

        DFS(0, 0);

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        cam.m_Lens.OrthographicSize = Mathf.Pow(w / 3 + h / 2, 0.7f) + 1;
    }
}