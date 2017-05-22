using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {
    
    private MazeCellWall n, s, e, w;

    public IntVector2 position;
    public bool Visited { get; set; }

    public void Initialize(int posX, int posZ, Transform t)
    {
        position.x = posX;
        position.z = posZ;
        Visited = false;
        name = "Maze Cell " + posX + ", " + posZ;
        transform.parent = t;
    }
        
    public MazeCellWall North {
        get { return n; }
        set
        {
            n = value;
            n.transform.localScale = new Vector3(1.0f, 1.0f, n.scaleSize);
        }
    }

    public MazeCellWall South
    {
        get { return s; }
        set
        {
            s = value;
            s.transform.localScale = new Vector3(1.0f, 1.0f, s.scaleSize);
        }
    }

    public MazeCellWall East
    {
        get { return e; }
        set
        {
            e = value;
            e.transform.localScale = new Vector3(e.scaleSize, 1.0f, 1.0f);
        }
    }

    public MazeCellWall West
    {
        get { return w; }
        set {
            w = value;
            w.transform.localScale = new Vector3(w.scaleSize, 1.0f, 1.0f);
        }
    }
}

