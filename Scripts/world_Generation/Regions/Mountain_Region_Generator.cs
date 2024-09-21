using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mountain_Region_Generator : MonoBehaviour
{

    public Tile EdgeTile,LeftTopCorner, RightTopCorner, RightTopCornerEdge, leftTopCornerEdge, TopCenter, TopEdge, LeftCenter, LeftEdge, Center, RightCenter, RightEdge, BottomCenter, BottomLeftCorner, BottomRightCorner, floorTile, left_path, right_path, center_Path, cave;

    public Tilemap wall_Map, floor_Map, cave_map, nature, trees,edge_map;
    public Tile[] CliffFaceTiles, nature_Tiles,Tree_Tiles;
    public int width;
    public int height;
    public float cellsize = 1;
    public int cave_amount = 0;
    public GameObject Dungeon_Object;
    public int TreeSmoothness = 4, PlantSmoothness = 4;
    public string seed;
    public bool useRandomSeed;
    public List<Vector2> _locations;
    [Range(0, 100)]
    public int randomFillPercent;

    public int Current_Layer = 0;
    int[,] map;

    List<Vector3Int> bottomCenterPositions = new List<Vector3Int>();
    System.Random pseudoRandom;
    private void Start()
    {
        Start_Genertaion();
    }
    public void Start_Genertaion()
    {
      
        map = null;
        wall_Map.ClearAllTiles();
        floor_Map.ClearAllTiles();
        cave_map.ClearAllTiles();
        nature.ClearAllTiles();
        trees.ClearAllTiles();
        _locations.Clear();
        Fill_Edge_Map();
        pseudoRandom = new System.Random(seed.GetHashCode());
        if (useRandomSeed)
        {
            seed = Random.Range(1, 10000).ToString();
        }
        // randomFillPercent = Random.Range(40, 60);
        if (seed != null)
        {
            Random.InitState(seed.GetHashCode());
        }
        cave_amount = Random.Range(1, 12);

        Fill_Floor_Map();

        GenerateMap();

        RenderMap();
        PlacePaths();
        Place_Cliff_Faces();
        PlaceCaves();

        Fill_Foliage();
        Fill_trees();
        
        _locations.Clear();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap(map);
        }
    }

    public void Fill_Edge_Map()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0)
                {

                    Vector3Int _Loc = new Vector3Int(x - 1, y, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (x == width - 1)
                {
                    Vector3Int _Loc = new Vector3Int(x + 1,y, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if( y == 0)
                {
                  Vector3Int _Loc =  new Vector3Int(x, y - 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (y == height - 1)
                {
                    
                    Vector3Int _Loc = new Vector3Int(x, y + 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (x == 0 && y == 0)
                {
                    Vector3Int _Loc = new Vector3Int(x - 1, y - 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (x == 0 && y == height)
                {
                    Vector3Int _Loc = new Vector3Int(x - 1, y + 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (x == width && y == height)
                {
                    Vector3Int _Loc = new Vector3Int(x + 1, y + 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
                if (x == width && y == 0)
                {
                    Vector3Int _Loc = new Vector3Int(x + 1, y - 1, 0);
                    edge_map.SetTile(_Loc, EdgeTile);
                }
            }
        }
    }

    void Fill_Floor_Map()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int Tile_Location = new Vector3Int(x, y, 0);
                floor_Map.SetTile(Tile_Location, floorTile);
                Vector3 open_Position = new Vector3Int(x, y, 0);

            }
        }
    }

    void RandomFillMap()
    {


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 0;

                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }



    void SmoothMap(int[,] mapToSmooth)
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(mapToSmooth, x, y);

                if (neighbourWallTiles > 5)
                    mapToSmooth[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    mapToSmooth[x, y] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int[,] mapToCheck, int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += mapToCheck[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void RenderMap()
    {
        wall_Map.ClearAllTiles();
        bottomCenterPositions.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);



                if (map[x, y] == 1)
                {
                    Tile tileToPlace = DetermineTile(x, y);
                    wall_Map.SetTile(position, tileToPlace);


                    if (tileToPlace == BottomCenter)
                    {
                        bottomCenterPositions.Add(position);
                    }

                    if (tileToPlace == Center)
                    {
                        floor_Map.SetTile(position, Center);
                        wall_Map.SetTile(position, null);
                    }
                    if (tileToPlace == TopCenter)
                    {
                        wall_Map.SetTile(position, TopEdge);
                        floor_Map.SetTile(position, TopCenter);

                    }
                    if (tileToPlace == LeftTopCorner)
                    {
                        wall_Map.SetTile(position, leftTopCornerEdge);
                        floor_Map.SetTile(position, LeftTopCorner);


                    }
                    if (tileToPlace == RightTopCorner)
                    {
                        wall_Map.SetTile(position, RightTopCornerEdge);
                        floor_Map.SetTile(position, RightTopCorner);

                    }
                    if (tileToPlace == LeftCenter)
                    {
                        wall_Map.SetTile(position, LeftEdge);
                        floor_Map.SetTile(position, LeftCenter);

                    }
                    if (tileToPlace == RightCenter)
                    {
                        wall_Map.SetTile(position, RightEdge);
                        floor_Map.SetTile(position, RightCenter);

                    }

                }

            }
        }
    }





    public void PlacePaths()
    {
        List<Vector3Int> pathPositions = new List<Vector3Int>();

        foreach (var pos in bottomCenterPositions)
        {
            Vector3Int left = pos + Vector3Int.left;
            Vector3Int right = pos + Vector3Int.right;

            if (bottomCenterPositions.Contains(left) && !pathPositions.Contains(pos))
            {
                floor_Map.SetTile(pos, left_path);
                floor_Map.SetTile(left, right_path);
                wall_Map.SetTile(pos, null);
                wall_Map.SetTile(left, null);
                pathPositions.Add(pos);
                pathPositions.Add(left);
                bottomCenterPositions.Remove(left);
                bottomCenterPositions.Remove(pos);
                break;

            }
            else if (bottomCenterPositions.Contains(right) && !pathPositions.Contains(pos))
            {
                floor_Map.SetTile(pos, right_path);
                floor_Map.SetTile(right, left_path);
                wall_Map.SetTile(pos, null);
                wall_Map.SetTile(right, null);
                pathPositions.Add(pos);
                pathPositions.Add(right);
                bottomCenterPositions.Remove(right);
                bottomCenterPositions.Remove(pos);
                break;
            }
        }
    }

    public void PlaceCaves()
    {
        System.Random random = new System.Random();

        bottomCenterPositions = bottomCenterPositions.OrderBy(a => random.Next()).ToList();

        for (int i = 0; i < cave_amount && i < bottomCenterPositions.Count; i++)
        {
            Vector3Int cavePosition = bottomCenterPositions[i];
            cave_map.SetTile(cavePosition, cave);

            for (int b = 0; b < cavePosition.y; b++)
            {
                Vector3Int checkPosition = new Vector3Int(cavePosition.x, cavePosition.y - b, 0);

                if (wall_Map.GetTile(checkPosition) == TopCenter || wall_Map.GetTile(checkPosition) == LeftTopCorner || wall_Map.GetTile(checkPosition) == LeftTopCorner)
                {
                    if (wall_Map.GetTile(checkPosition) == TopCenter)
                    {
                        Debug.Log("tile Found");
                        Debug.Log("Topcenter" + checkPosition.x + "," + checkPosition.y);
                        Vector3Int Checkleft = new Vector3Int(checkPosition.x - 1, checkPosition.y, 0);
                        Vector3Int CheckRight = new Vector3Int(checkPosition.x + 1, checkPosition.y, 0);

                        if (wall_Map.GetTile(Checkleft) != Center)
                        {
                            Debug.Log("left");
                            wall_Map.SetTile(checkPosition, null);
                            floor_Map.SetTile(checkPosition, center_Path);
                            Debug.Log("tileset");
                            break;
                        }
                        
                        
                        else if (wall_Map.GetTile(CheckRight) != Center)
                        {
                            Debug.Log("left");
                            wall_Map.SetTile(checkPosition, null);
                            floor_Map.SetTile(checkPosition, center_Path);
                            Debug.Log("tileset");
                            break;
                        }
                    }
                }
            }
        }
        wall_Map.RefreshAllTiles();
        floor_Map.RefreshAllTiles();
    }

    public void Place_Cliff_Faces()
    {
        foreach (var pos in bottomCenterPositions)
        {
            if (wall_Map.GetTile(pos) == BottomCenter)
            {
                wall_Map.SetTile(pos, CliffFaceTiles[Random.Range(0, CliffFaceTiles.Length)]);
            }
        }
    }

    void Fill_Foliage()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    Vector3 mapxy = new Vector3Int(x, y, 0);
                    _locations.Add(mapxy);
                }
                if (map[x, y] == 1)
                {
                    if (floor_Map.GetTile(new Vector3Int(x, y, 0)) == Center)
                    {
                        Vector3 mapxy = new Vector3Int(x, y, 0);
                        _locations.Add(mapxy);
                    }
                }

            }
        }
        int plant_Amount = Random.Range(0, Mathf.FloorToInt(_locations.Count / PlantSmoothness));
        for (int i = 0; i < plant_Amount; i++)
        {
            int NextPlant = Random.Range(0, _locations.Count);
            int plantnumber = Random.Range(0, nature_Tiles.Length);
            Vector3Int plantPosition = new Vector3Int(Mathf.FloorToInt(_locations[NextPlant].x), Mathf.FloorToInt(_locations[NextPlant].y), 0);
            _locations.Remove(_locations[NextPlant]);
            nature.SetTile(plantPosition, nature_Tiles[plantnumber]);

        }
    }

    void Fill_trees()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
             
                if (map[x, y] == 1)
                {
                    if (floor_Map.GetTile(new Vector3Int(x, y, 0)) == Center)
                    {
                        Vector3 mapxy = new Vector3Int(x, y, 0);
                        _locations.Add(mapxy);
                    }
                }
            }
        }
        int plant_Amount = Random.Range(0, Mathf.FloorToInt(_locations.Count / TreeSmoothness));
        for (int i = 0; i < plant_Amount; i++)
        {
            int NextPlant = Random.Range(0, _locations.Count);
            int plantnumber = Random.Range(0, Tree_Tiles.Length);
            Vector3Int plantPosition = new Vector3Int(Mathf.FloorToInt(_locations[NextPlant].x/2), Mathf.FloorToInt(_locations[NextPlant].y/2), 0);
            _locations.Remove(_locations[NextPlant]);
            trees.SetTile(plantPosition, Tree_Tiles[plantnumber]);

        }
    }

    Tile DetermineTile(int x, int y)
    {
        bool top = y < height - 1 && map[x, y + 1] == 1;
        bool bottom = y > 0 && map[x, y - 1] == 1;
        bool left = x > 0 && map[x - 1, y] == 1;
        bool right = x < width - 1 && map[x + 1, y] == 1;

        if (top && left && !right && !bottom) return BottomRightCorner;
        if (top && right && !left && !bottom) return BottomLeftCorner;
        if (bottom && left && !right && !top) return RightTopCorner;
        if (bottom && right && !left && !top) return LeftTopCorner;
        if (!top && left && right) return TopCenter;
        if (!bottom && left && right) return BottomCenter;
        if (!left && top && bottom) return LeftCenter;
        if (!right && top && bottom) return RightCenter;

        return Center;
    }



}