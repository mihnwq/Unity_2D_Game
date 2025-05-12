using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PFGrid : MonoBehaviour
{

    public Transform player;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;

    public float nodeRadius;

    float nodeDiameter;

    public int gridSizeX, gridSizeY;

    public Node[,] grid;

    public Astar pf;

    public int maxSize;

    public Node targetNode;

    public void Start()
    {
        nodeDiameter = 2 * nodeRadius;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        maxSize = gridSizeX * gridSizeY;

        CreateGrid();
    }


    public void FixedUpdate()
    {
        //if (shouldBuildGrid())
        //  CreateGrid(); 

        GetSubgrid();
    }

    public int horizontal = 3;
    public int vertical = 5;

    public bool shouldBuildGrid()
    {
        Node n = NodeFromWorldPoint(player.position);

        Vector2 playerGridPosition = n.gridPosition;

        int playerX = (int)playerGridPosition.x;

        int playerY = (int)playerGridPosition.y;

        return playerX >= gridSizeX - vertical || playerX <= vertical ||
               playerY >= gridSizeY - horizontal || playerY <= horizontal;
    }

    

 
    public void CreateGrid()
    {
        transform.position = player.position;

        grid = new Node[gridSizeX, gridSizeY];

        Vector2 worldBottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * gridWorldSize.x / 2 - Vector2.down * gridWorldSize.y / 2;

        

        for (int x = 0; x < gridSizeX; x++)
          for(int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.down * (y * nodeDiameter + nodeRadius);

                bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                grid[x, y] = new Node(worldPoint, walkable, new Vector2(x,y));
            }
    }

    public Vector2 maxIterativeGridSize = new Vector2(5,5);

    public int startGridX;
    public int startGridY;

    public int endGridX;
    public int endGridY;

    public void GetSubgrid()
    {
        Vector3 iterativePosition = player.position;

       int iterativeGridSizeX = Mathf.RoundToInt(maxIterativeGridSize.x / nodeDiameter);
        int iterativeGridSizeY = Mathf.RoundToInt(maxIterativeGridSize.y / nodeDiameter);

        Vector2 worldBottomLeft = new Vector2(iterativePosition.x, iterativePosition.y) - Vector2.right * maxIterativeGridSize.x / 2 - Vector2.down * maxIterativeGridSize.y / 2;

        Vector2 worldPoint;

        worldPoint = GetWorldPoint(0, 0, worldBottomLeft);

       // Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.down * (y * nodeDiameter + nodeRadius);

        Node start = NodeFromWorldPoint(worldPoint);

        startGridX = (int)start.gridPosition.x;
        startGridY = (int)start.gridPosition.y;

        worldPoint = GetWorldPoint(iterativeGridSizeX, iterativeGridSizeY, worldBottomLeft);

        Node end = NodeFromWorldPoint(worldPoint);

        endGridX = (int)end.gridPosition.x;
        endGridY = (int)end.gridPosition.y;
    }

    private Vector2 GetWorldPoint(int x, int y, Vector2 worldBottomLeft)
    {
        return  worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.down * (y * nodeDiameter + nodeRadius);
    }
   /* public void GetSubgrid()
    {
        Node playerNode = NodeFromWorldPoint(player.position);
        Vector2 playerGridPos = playerNode.gridPosition;

        int playerX = (int)playerGridPos.x;
        int playerY = (int)playerGridPos.y;

        int subgridWidth = Mathf.RoundToInt(maxIterativeGridSize.x / nodeDiameter);
        int subgridHeight = Mathf.RoundToInt(maxIterativeGridSize.y / nodeDiameter);

        // Make sure subgrid dimensions are odd so the player can be centered
        if (subgridWidth % 2 == 0) subgridWidth++;
        if (subgridHeight % 2 == 0) subgridHeight++;

        int halfWidth = subgridWidth / 2;
        int halfHeight = subgridHeight / 2;

        startGridX = Mathf.Clamp(playerX - halfWidth, 0, gridSizeX - 1);
        endGridX = Mathf.Clamp(playerX + halfWidth, 0, gridSizeX - 1);

        startGridY = Mathf.Clamp(playerY - halfHeight, 0, gridSizeY - 1);
        endGridY = Mathf.Clamp(playerY + halfHeight, 0, gridSizeY - 1);
    }*/

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
       
        float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y - transform.position.y + gridWorldSize.y / 2) / gridWorldSize.y;

  
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

      
        int x = Mathf.FloorToInt(gridSizeX * percentX);

        ///Need this since my origin is at (0,0)
        int y = gridSizeY - Mathf.FloorToInt(gridSizeY * percentY);

     
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

     
        
       return grid[x, y];
       
    }

    public Node getNode(int x, int y)
    {
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if(grid != null)
        {
            Node playerNode = NodeFromWorldPoint(new Vector2(player.position.x, player.position.y));

            targetNode = pf.targetNode;

            foreach(Node node in grid)
            {
                Gizmos.color = node.isSolid ? Color.red : Color.green;

             /*   if(node == playerNode)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                }*/

          /*      if(node.gridPosition.x >= startGridX && node.gridPosition.y >= startGridY &&
                    node.gridPosition.x <= endGridX && node.gridPosition.y <= endGridY)
                {
                   // Gizmos.color = Color.magenta;

                    Gizmos.color = node.isSolid ? Color.red : Color.green;

                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                    
                }*/
               
              

              /*  if (pf.lastShorthestPath.Contains(node))
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                }
                
                                if(node == targetNode)
                                {
                                    Gizmos.color = Color.cyan;
                                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                                }

                                if(node == pf.startNode)
                                {
                                    Gizmos.color = Color.magenta;
                                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                                }

                                if(node == pf.currentCheck)
                                {
                                    Gizmos.color = Color.white;
                                    Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                                }*/
                /*
                           if(node.isSolid == true)
                           {
                               Gizmos.color = Color.clear;
                               Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);
                           }*/




                   Gizmos.DrawSphere(new Vector3(node.worldPosition.x, node.worldPosition.y, 0), nodeRadius);

                  Gizmos.DrawCube(node.worldPosition, Vector2.one * (nodeDiameter - .1f));
            }
        }
    }
 
    

}

