using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

    public class Town_Region_Generator : MonoBehaviour
    {

    public Tile LeftTopCorner, RightTopCorner, TopCenter, LeftCenter, Center, RightCenter, BottomCenter, BottomLeftCorner, BottomRightCorner, floorTile;
    public Tilemap trail_Map, floor_Map, nature, FloorM, WallM, DoorM;
    public Tile[]  nature_Tiles;
    public int width;
    public int height;
    public float cellsize = 1;
    public float building_Amount;
    public GameObject[] Building_Objects;
    public Building_Object[] Building_Layouts;
    public LayerMask buildings;

    public string seed;
    public bool useRandomSeed;
    public List<Vector2> _locations;
    [Range(0, 100)]
    public int randomFillPercent;
    [Range(0, 100)]
    public int secondLayerFillPercent;  // New adjustable fill percentage for the second layer

    int[,] map;
    public int citizen_Number;
    List<Vector3Int> bottomCenterPositions = new List<Vector3Int>();
    System.Random pseudoRandom;
    private void Start()
    {
        
    }
    public void Start_Genertaion()
    {
        map = null;
        trail_Map.ClearAllTiles();
        floor_Map.ClearAllTiles();
        nature.ClearAllTiles();
        _locations.Clear();

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
        Fill_Floor_Map();
        GenerateMap();
        RenderMap();
        Fill_Foliage();
        Place_Buildings();
      
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

                if (neighbourWallTiles > 4)
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

    public void Place_Buildings()
    {
        List<Vector2> BuildingSpots = new List<Vector2> ();
        for (int i = 0; i < building_Amount; i++)
        {
            for (int x = 0; x < width-1; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);



                    if (map[x, y] == 1)
                    {


                        Vector2 pos = new Vector2(x, y);
                        BuildingSpots.Add(pos);


                    }
                }
            }


            int RanRang = Random.Range(0,BuildingSpots.Count);
            int RanRangtwo = Random.Range(0, Building_Layouts.Length);
            if (Is_Space_Availabile(BuildingSpots[RanRang], Building_Layouts[RanRangtwo]))
            {
                Build_Building_At_Location(new Vector3Int(Mathf.FloorToInt(BuildingSpots[RanRang].x), Mathf.FloorToInt(BuildingSpots[RanRang].y), 0), Building_Layouts[RanRangtwo]);
            }
        }
    }
    void RenderMap()
    {
        trail_Map.ClearAllTiles();
        bottomCenterPositions.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);



                if (map[x, y] == 1)
                {
                    Tile tileToPlace = DetermineTile(x, y);
                    floor_Map.SetTile(position, tileToPlace);


                  

                }
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
            }
        }
        int plant_Amount = Random.Range(0, Mathf.FloorToInt(_locations.Count / 2));
        for (int i = 0; i < plant_Amount; i++)
        {
            int NextPlant = Random.Range(0, _locations.Count);
            int plantnumber = Random.Range(0, nature_Tiles.Length);
            Vector3Int plantPosition = new Vector3Int(Mathf.FloorToInt(_locations[NextPlant].x), Mathf.FloorToInt(_locations[NextPlant].y), 0);
            _locations.Remove(_locations[NextPlant]);
            nature.SetTile(plantPosition, nature_Tiles[plantnumber]);

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

    public void Build_Building_At_Location(Vector3Int Position, Building_Object building)
    {


        int randomrand = Random.Range(1, building.Width - 1);

        for (int x = 0; x < building.Width; x++)
        {
            for (int y = 0; y < building.Height; y++)
            {
                if (x > 0 && y > 0 && x != building.Width && y != building.Height)
                {
                    floor_Map.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.floorTile);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x == 0 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallBlc);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x == building.Width - 1 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + building.Width, Position.y + y), building.WallBrc);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x == building.Width - 1 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x + 1, Position.y + y), building.WallTrc);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x == 0 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallTlc);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }

                if (x == 0 && y > 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallLeft);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x == building.Width - 1 && y > 0 && y < building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x + 1, Position.y + y), building.WallRight);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x > 0 && y == 0)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallCenter);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }
                if (x >= 1 && y == building.Height - 1)
                {
                    WallM.SetTile(new Vector3Int(Position.x + x, Position.y + y), building.WallCenter);
                    nature.SetTile(new Vector3Int(Position.x + x, Position.y + y), null);
                }


            }
        }
        
        bool downbelow = false;
        for (int g = 0; g < building.Width+1; g++)
        {
            if(WallM.GetTile(new Vector3Int(Position.x+g,Position.y-1,0)) != null)
            {
                downbelow = true;
            }
        }
       if(!downbelow)
        {
            DoorM.SetTile(new Vector3Int(Position.x + randomrand, Position.y), building.DoorTile);
            WallM.SetTile(new Vector3Int(Position.x + randomrand, Position.y), null);
        }
        else
        {
            DoorM.SetTile(new Vector3Int(Position.x + randomrand, Position.y + building.Height - 1), building.DoorTile);
            WallM.SetTile(new Vector3Int(Position.x + randomrand, Position.y + building.Height - 1), null);
        }
        citizen_Number++;
    }

    public bool Is_Space_Availabile(Vector3 position, Building_Object building)
    {
        for (int x = 0; x < building.Width; x++)
        {


            for (int y = 0; y < building.Height; y++)
            {
                Vector3Int NewCheckPos = new Vector3Int(Mathf.FloorToInt(position.x) + x, Mathf.FloorToInt(position.y) + y, 0);

                if (WallM.GetTile(NewCheckPos) != null)
                {
                    return false;
                }
           
                if(NewCheckPos.y >= height || NewCheckPos.x >= width)
                {
                    return false;
                }

            }


        }

        return true;
    }


}