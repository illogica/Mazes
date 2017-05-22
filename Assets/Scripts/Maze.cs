using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

    public IntVector2 size;
    public MazeCell cellPrefab;
    public MazeCellWall wallPrefab;
    public float generationStepDelay;
    
    private MazeCell[,] cells;
    
	void Start () {
        Debug.Log("Maze.Start()");
	}
	
	void Update () {
		
	}

    public void Generate()
    {
        //WaitForSeconds delay = new WaitForSeconds(/*generationStepDelay*/0f);
        cells = new MazeCell[size.x, size.z];
        for(int x = 0; x<size.x; x++)
        {
            for(int z=0; z<size.z; z++)
            {
                //yield return delay;
                CreateCell(x, z);
            }
        }

        StartCoroutine(DFS());
    }

    private void CreateCell(int x, int z)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        newCell.Initialize(x, z, transform);
        cells[x, z] = newCell;
        //newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(x - size.x * 0.5f + 0.5f, 0f, z - size.z * 0.5f + 0.5f);
        
        if(x == 0)
        {
            newCell.West = Instantiate(wallPrefab) as MazeCellWall;
            newCell.West.transform.parent = newCell.transform.parent;
            newCell.West.transform.localPosition = newCell.transform.localPosition + new Vector3(-0.5f, 0f, 0f);
        } else
        {
            newCell.West = cells[x - 1, z].East;
        }

        if (z == 0)
        {
            newCell.South = Instantiate(wallPrefab) as MazeCellWall;
            newCell.South.transform.parent = newCell.transform.parent;
            newCell.South.transform.localPosition = newCell.transform.localPosition + new Vector3(0f, 0f, -0.5f);
        } else
        {
            newCell.South = cells[x, z - 1].North;
        }

        newCell.North = Instantiate(wallPrefab) as MazeCellWall;
        newCell.North.transform.parent = newCell.transform.parent;
        newCell.North.transform.localPosition = newCell.transform.localPosition + new Vector3(0f, 0f, 0.5f);

        newCell.East = Instantiate(wallPrefab) as MazeCellWall;
        newCell.East.transform.parent = newCell.transform.parent;
        newCell.East.transform.localPosition = newCell.transform.localPosition + new Vector3(+0.5f, 0f, 0f);
    }

    /*
     * Depth-first search, Recursive implementation

       This algorithm is a randomized version of the depth-first search algorithm.
       0 Frequently implemented with a stack, this approach is one of the simplest ways to generate a maze using a computer.
       Consider the space for a maze being a large grid of cells (like a large chess board),
       each cell starting with four walls.
       1 Starting from a random cell,
       2 the computer then selects a random neighbouring cell that has not yet been visited.
       3 The computer removes the wall between the two cells
       4 and marks the new cell as visited,
       5 and adds it to the stack to facilitate backtracking.
       6 The computer continues this process, with a cell that has no unvisited neighbours being considered a dead-end.
       7 When at a dead-end it backtracks through the path until it reaches a cell with an unvisited neighbour,
       continuing the path generation by visiting this new, unvisited cell (creating a new junction).
       8 This process continues until every cell has been visited,
       causing the computer to backtrack all the way back to the beginning cell.
       We can be sure every cell is visited.
    */

    public IEnumerator DFS()
    {
        //[0]
        Stack<MazeCell> cellsStack = new Stack<MazeCell>();

        //[1]
        MazeCell currentCell = cells[0, 0]; //todo: random initial cell
        currentCell.Visited = true;
        cellsStack.Push(currentCell);

        while (cellsStack.Count > 0)
        {
            //Give it some eye candy
            WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
            yield return delay;

            //[2]
            List<MazeCell> neighbours = getUnvisitedNeighbours(currentCell);

            //[6]
            if (neighbours.Count == 0)
            {
                //[7]
                currentCell = cellsStack.Pop();
                continue;
            }
                
            //[2]
            MazeCell nextCell = neighbours[Random.Range(0, neighbours.Count)];
            nextCell.gameObject.GetComponent<Renderer>().material.color = Color.blue;

            //[3]
            if (currentCell.position.x == nextCell.position.x)
            {
                if (currentCell.position.z < nextCell.position.z)
                    currentCell.North.Visible = false;
                else
                    currentCell.South.Visible = false;
            }

            if (currentCell.position.z == nextCell.position.z)
            {
                if (currentCell.position.x < nextCell.position.x)
                    currentCell.East.Visible = false;
                else
                    currentCell.West.Visible = false;
            }

            //[4]
            nextCell.Visited = true;

            //[5]
            cellsStack.Push(currentCell);
            currentCell = nextCell;
        }

        Debug.Log("DONE");
    }

    public void TestDFS()
    {
        for(int x = 0; x<size.x; x++)
        {
            for(int z =0; z<size.z; z++)
            {
                Debug.Log("Cell[" + x + "," + z + "]: " + getUnvisitedNeighbours(cells[x, z]).Count);
            }
        }

        cells[2, 2].North.Visible = false;
    }

    private List<MazeCell> getUnvisitedNeighbours(MazeCell cell)
    {
        List<MazeCell> cellsList = new List<MazeCell>();

        // just to shorten the code later
        int x = cell.position.x;
        int z = cell.position.z;

        //Analize south:
        if ( (z > 0) && !cells[x, z - 1].Visited)
        {
            cellsList.Add(cells[x, z - 1]);
        }

        //Analize west:
        if ((x > 0) && !cells[x - 1, z].Visited)
        {
            cellsList.Add(cells[x - 1, z]);
        }

        //Analize east:
        if ((x < size.x - 1) && !cells[x + 1, z].Visited)
        {
            cellsList.Add(cells[x + 1, z]);
        }

        //Analize north:
        if ((z < size.z - 1) && !cells[x, z + 1].Visited)
        {
            cellsList.Add(cells[x, z + 1]);
        }
        return cellsList;
    }

}
