using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class Astar : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    PFGrid grid;

    [SerializeField]
    Transform end, start;

   public  Node currentNode, startNode, targetNode;

    public bool goalReached;

    public int xMax, yMax;

    public List<Node> shorthestPath = new List<Node>();

    public List<Node> lastShorthestPath = new List<Node>();

    public int maxSteps;

    EnemyMovement movement;

    Heap<Node> openList;

    HashSet<Node> closedList;

    public int steps;

    public Vector2 lastKnownLocation = Vector2.zero;

    Vector3 lastEnd;

    public bool initiatePath = false;

    public bool newDirective = false;

    public bool shouldFollowPath = false;

    public float distanceTreshold;

    public int currentNodeIndexToFollow;

    [SerializeField]
    int minimumCount;

    public void SetStartAndEndTargets(Transform start, Transform end)
    {
        this.start = start;
        this.end = end;
    }

    public void SetMovementObject(EnemyMovement movement)
    {
        this.movement = new EnemyMovement(movement);
    }


    /*   bool isInGrid()
       {
           return movement.nearObject(distanceTreshold, grid.transform.position);
       }*/


    private bool ShouldBuildPath()
    {

       return !isNearPlayer() && (HasPlayerMoved() || !goalReached);

    }

    [SerializeField]
    float playerTreshold;

    private bool isNearPlayer()
    {
        return movement.nearObject(playerTreshold, end.position);
    }

    public bool nearPlayer = false;

    bool NewDirective()
    {
        return steps == maxSteps || steps == 0 || goalReached;
    }

    private string order;

    private Transform dummyTarget;

    public void SetDummyTarget(Transform dummy)
    {
        dummyTarget = dummy;
    }

    public void SetNewOrderCommand(string newOrder)
    {
        order = newOrder;
    }

    void Init()
    {

        openList = new Heap<Node>(grid.maxSize);
        closedList = new HashSet<Node>();

        shorthestPath = new List<Node>();

        startNode = grid.NodeFromWorldPoint(start.position);

        goalReached = false;


        // targetNode = grid.NodeFromWorldPoint(end.position);
        targetNode = GetTargetNode(order , grid.NodeFromWorldPoint(dummyTarget.position));


        steps = maxSteps;

        openList.Add(startNode);
    }

    public Node GetTargetNode(string mode, Node target)
    {
        switch(mode)
        {
            case "Direct":
               target = grid.NodeFromWorldPoint(end.position);
                break;
            case "Area":
                return GetAreaTargetNode(0, 0, 2, 2);
                break;
        }

        return target;
    }

    private Node GetAreaTargetNode(int tX, int tY, int mX, int mY)
    {
        Node target = grid.NodeFromWorldPoint(end.position);

        int x = (int)target.gridPosition.x;
        int y = (int)target.gridPosition.y;

        Random rnd = new Random();


        int nX = (x + tX) + rnd.Next(0, mX);
        int nY = (y + tY) + rnd.Next(0, mY);

        return grid.getNode(nX,nY);
    }

    private void CheckForNewDirective()
    {
        newDirective = NewDirective();

        if (NewDirective() || !CoordsInGrid((int)startNode.gridPosition.x,(int)startNode.gridPosition.y))
        {
            Init();
        }

        Debug.Log(!CoordsInGrid((int)startNode.gridPosition.x, (int)startNode.gridPosition.y));
    }

    private bool HasPlayerMoved()
    {
        return end.position - lastEnd != Vector3.zero;
    }

    public void Start()
    {
        currentNodeIndexToFollow = startingFollowingIndex;

        xMax = grid.gridSizeX;
        yMax = grid.gridSizeY;

        lastEnd = end.position;

        steps = maxSteps;

        StartCoroutine(UpdateLastKnownPlayerPosition(1f));

        Init();
    }

    public void AstarUpdate()
    {

        targetNode = grid.NodeFromWorldPoint(end.position);

        nearPlayer = isNearPlayer();

            if (!initiatePath)
                initiatePath = ShouldBuildPath();

            if (initiatePath && !nearPlayer)
                FindPath();

          FollowPath(start, lastShorthestPath);

        
        lastEnd = end.position;
    }

    public IEnumerator UpdateLastKnownPlayerPosition(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (end != null)
            {
               lastEnd = end.position;
            }
        }
    }

    public Node currentCheck;

   void FindPath()
    {

        CheckForNewDirective();

        if (!goalReached && openList.Count != 0 && steps != 0)
        {
            currentNode = openList.RemoveFirst();
            closedList.Add(currentNode);

            AddNeighbours(currentNode, openList, closedList);

            currentCheck = currentNode;

            steps--;

            if (currentNode == targetNode)
            {
                goalReached = true;
                initiatePath = false;
                steps = maxSteps;

                TrackPath();
            }
        }
    }

    public void TrackPath()
    {
        Node current = targetNode;

        Node overTimeStart = grid.NodeFromWorldPoint(start.position);

        while (current != startNode)
        {
            overTimeStart = grid.NodeFromWorldPoint(start.position);

            current = current.parent;

            shorthestPath.Add(current);

            if (NeighbouringNode(current,overTimeStart))
               break;
        }

        if (shorthestPath.Count >= minimumCount)
        {
            lastShorthestPath = shorthestPath;
        }
       
    }

    private bool NeighbouringNode(Node nodeA, Node nodeB)
    {
        return Math.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x) < 3 &&
                 Math.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y) < 3;
    }

    void AddNeighbours(Node n, Heap<Node> openList, HashSet<Node> closedList)
    {
        int nx = (int)n.gridPosition.x;
        int ny = (int)n.gridPosition.y;

        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

           

                int checkX = nx + x;
                int checkY = ny + y;


               // if ((checkX < xMax && checkY < yMax) && (checkY >= 0 && checkX >= 0))
               if(CoordsInGrid(checkX,checkY))
                {
                    Node neighbour = grid.grid[checkX, checkY];


                    if (neighbour.CanAcces() && !closedList.Contains(neighbour))
                    {

                        if (!neighbour.CanAcces())
                        {
                            // Draw a red ray pointing up from the center of the inaccessible tile
                            //Used for debugging and also looks cool :P
                            Debug.DrawRay(neighbour.worldPosition, Vector3.up * 0.5f, Color.red, 1f);
                            continue;
                        }
<<<<<<< Updated upstream
=======

                       
>>>>>>> Stashed changes
                        // int moveCost = (x != 0 && y != 0) ? 14 : 10;

                        int moveCost = GetOctileDistance(n, neighbour);

                        int tentativeGCost = n.gCost + moveCost;

                        if (tentativeGCost < neighbour.gCost || !openList.Contains(neighbour))
                        {
                            neighbour.gCost = tentativeGCost;
                              neighbour.hCost = GetOctileDistance(neighbour, targetNode);

                            //neighbour.hCost = GetManhatanDistance(neighbour, targetNode);

                            neighbour.parent = n;

                            if (!openList.Contains(neighbour))
                                openList.Add(neighbour);
                            else openList.UpdateItem(neighbour);
                        }

                    }
                }

            }
    }

    private bool CoordsInGrid(int x, int y)
    {
        return (x <= grid.endGridX && y <= grid.endGridY) && (x >= grid.startGridX && y >= grid.startGridY);
    }


    int GetManhatanDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs((int)(nodeA.gridPosition.x - nodeB.gridPosition.x));
        int dstY = Mathf.Abs((int)(nodeA.gridPosition.y - nodeB.gridPosition.y));

        return 10 * (dstX + dstY);
    }

    int GetOctileDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs((int)(nodeA.gridPosition.x - nodeB.gridPosition.x));
        int dstY = Mathf.Abs((int)(nodeA.gridPosition.y - nodeB.gridPosition.y));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private Node currentNodeToFollow;

    public int startingFollowingIndex;

    public int lastCount = 1;

      public void FollowPath(Transform entity, List<Node> path)
      {
          int count = path.Count;

          if (lastCount != count)
              currentNodeIndexToFollow = startingFollowingIndex; 

          if (nearPlayer)
          {
              movement.moveTowards(end.position);
          }
          else
         if (count > 1)
          {
              Vector2 currentPosition = grid.NodeFromWorldPoint(entity.position).gridPosition;

              currentNodeToFollow = path[(count - 1) - currentNodeIndexToFollow];

              if (areVectorsEqual(currentPosition, currentNodeToFollow.gridPosition))
                  currentNodeIndexToFollow = Mathf.Clamp(currentNodeIndexToFollow + 1, 1, count - 1);

              movement.moveTowards(currentNodeToFollow.worldPosition);

          }
          else currentNodeIndexToFollow = startingFollowingIndex;

          

          lastCount = count;
      }




    private bool areVectorsEqual(Vector2 v1, Vector2 v2)
    {
        return Mathf.Approximately(v1.x,v2.x) && Mathf.Approximately(v2.y , v2.y);
    }

}
