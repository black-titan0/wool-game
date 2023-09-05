using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Parser : LevelGenerator
{
    void Start()
    {
        string filePath = "Assets/InstantiateInfo.txt";
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 1)
        {
            Debug.LogError("File is empty.");
            return;
        }

        string[] dimensions = lines[0].Split(' ');
        if (dimensions.Length != 4)
        {
            Debug.LogError("Invalid format in the first line: " + lines[0]);
            return;
        }

        w = int.Parse(dimensions[0]);
        h = int.Parse(dimensions[1]);
        x = int.Parse(dimensions[2]);
        y = int.Parse(dimensions[3]);

        Regex regex = new Regex(@"(\d+)\s+(\d+)\s+\(([\d.]+),\s+([\d.]+),\s+([\d.]+)\)\s+(\d+)");
        
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            Match match = regex.Match(line);

            if (!match.Success)
            {
                Debug.LogError("Invalid format in line " + (i + 1) + ": " + line);
                continue;
            }

            int wx = int.Parse(match.Groups[1].Value);
            int wy = int.Parse(match.Groups[2].Value);
            float shX = float.Parse(match.Groups[3].Value);
            float shY = float.Parse(match.Groups[4].Value);
            float shZ = float.Parse(match.Groups[5].Value);
            float ang = float.Parse(match.Groups[6].Value);

            Vector3 sh = new Vector3(shX, shY, shZ);

            // Instantiate walls using the parsed information
            Instantiate(Wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), Level);
        }
        cam.m_Lens.OrthographicSize = Mathf.Pow(w / 3 + h / 2, 0.7f) + 1;
    }
}
