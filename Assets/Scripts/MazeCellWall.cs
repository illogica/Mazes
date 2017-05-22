using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellWall : MonoBehaviour {

    public float scaleSize;

    private bool visible = true;
    private Renderer r;

    public bool Visible
    {
        get { return visible; }
        set
        {
            visible = value;
            r.enabled = visible;
        }
    }

    public void Awake()
    {
        r = GetComponent<Renderer>();
    }
}
