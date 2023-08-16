using System;
using System.IO;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public TextAsset fileAsset; // Attach file

    private void Start()
    {
        if (fileAsset != null)
        {
            string[] lines = fileAsset.text.Split('\n');
            int height = int.Parse(lines[0].Split(' ')[0]);
            int width = int.Parse(lines[0].Split(' ')[1]);

            Debug.Log($"Height: {height}, Width: {width}");

            // Parse the 2D matrix
            int[,] matrix = new int[height, width];
            for (int i = 0; i < height; i++)
            {
                string[] row = lines[i + 1].Split(' ');
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = int.Parse(row[j]);
                }
            }

            // Parse start location
            string[] startCoords = lines[height + 1].Split(' ');
            int startX = int.Parse(startCoords[0]);
            int startY = int.Parse(startCoords[1]);

            // Parse ball locations
            for (int i = height + 2; i < lines.Length - 1; i++)
            {
                string[] ballData = lines[i].Split(')');
                string[] ballStart = ballData[0].Trim('(', ' ').Split(',');
                string[] ballEnd = ballData[1].Trim('(', ' ').Split(',');

                int ballStartX = int.Parse(ballStart[0]);
                int ballStartY = int.Parse(ballStart[1]);
                int ballEndX = int.Parse(ballEnd[0]);
                int ballEndY = int.Parse(ballEnd[1]);

                Debug.Log($"Ball's location: ({ballStartX}, {ballStartY}), Hole's location: ({ballEndX}, {ballEndY})");
            }
        }
        else
        {
            Debug.LogError("assign the file fucker");
        }
    }
}
