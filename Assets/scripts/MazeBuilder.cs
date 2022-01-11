using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Maze m = new Maze(10, 10);
        m.saveAsPng("/home/ollie/UnityProjects/MazeGamePrograms/MazeGame/Assets/output.png");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
