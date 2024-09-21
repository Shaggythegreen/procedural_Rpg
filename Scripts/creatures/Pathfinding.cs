using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public float nodeSize;
    public Vector2 worldBottomLeft;
    public Tilemap walkableTilemap;  // Tilemap with walkable tiles
    public Tilemap nonWalkableTilemap;  // Tilemap with non-walkable tiles
    public LayerMask layerMask;
    private Node[,] grid;

    private void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 worldPoint = worldBottomLeft + new Vector2(x * nodeSize, y * nodeSize);

                // Check if the node overlaps with the tilemap collider
                bool walkable = !Physics2D.OverlapCircle(worldPoint,1, layerMask);

                grid[x, y] = new Node(worldPoint, x, y, walkable);
            }
        }
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x - worldBottomLeft.x) / (gridWidth * nodeSize);
        float percentY = (worldPosition.y - worldBottomLeft.y) / (gridHeight * nodeSize);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((gridHeight - 1) * percentY);

        return grid[x, y];
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public class Node
    {
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;
        public bool walkable;
        public int gCost;
        public int hCost;
        public Node parent;

        public Node(Vector2 _worldPos, int _gridX, int _gridY, bool _walkable)
        {
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
            walkable = _walkable;
            gCost = 0;
            hCost = 0;
            parent = null;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }
    }
    public bool IsTileWalkable(Vector3Int tilePosition)
    {
        // Check if the tile is set on the walkableTilemap and not on the nonWalkableTilemap
        TileBase walkableTile = walkableTilemap.GetTile(tilePosition);
        TileBase nonWalkableTile = nonWalkableTilemap.GetTile(tilePosition);

        if (nonWalkableTile != null)
        {
            // Tile is non-walkable
            return false;
        }

        return walkableTile != null;
    }
}