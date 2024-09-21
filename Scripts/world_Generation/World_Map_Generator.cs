using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class World_Map_Generator : MonoBehaviour
{
    
    public List<World_Map_Location> world_Locations;
    public string World_Seed;
    public Tilemap Water_Map,World_Map,Location_Map, wall_Map, floor_Map, cave_map, nature, trees;
    public Mountain_region_Presets[] Mountain_Presets;
    public Town_Region_Presets[] Town_Presets;
    public Tile LeftTopCorner, RightTopCorner, TopCenter, LeftCenter, Center, RightCenter, BottomCenter, BottomLeftCorner, BottomRightCorner, WaterTile;
    public Tile town,mountain, beach, flat_01,flat_02,cave,dungeon;
    public List<World_Map_Location> World_Map_Locations;
    public int height,width,mountAmnt,beachamnt,caveamnt,forestAmnt,flatlandAmount,TownAmnt,DungeonAmnt;
    public int[,] map;
    [Range(0, 100)]
    public int randomFillPercent;
    System.Random pseudoRandom;
    public Deity_Generator deity_Generator;
    [SerializeField] Mountain_Region_Generator mountain_Region_Generator;
    public Town_Region_Generator town_Region_Generator;
    public Region_Npc_Handler npc_Handler;
    public Biome Current_region_Biome;

    private void Start()
    {
        pseudoRandom = new System.Random(World_Seed.GetHashCode());
        Random.InitState(World_Seed.GetHashCode());
        town_Region_Generator.Start_Genertaion();
        npc_Handler.Generate();
        // GenerateMap();

    }
    void GenerateMap()
    {

        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Water_Map.SetTile(new Vector3Int(x, y), WaterTile);
            }
        }
     
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap(map);
        }
        RenderMap();
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

    void RenderMap()
    {
        wall_Map.ClearAllTiles();


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);



                if (map[x, y] == 1)
                {
                    Tile tileToPlace = DetermineTile(x, y);
                    World_Map.SetTile(position, tileToPlace);



                    if (tileToPlace == Center)
                    {
                        World_Map.SetTile(position, Center);
                        Generate_beach_location(position.x, position.y);

                    }
                    if (tileToPlace == TopCenter)
                    {
                      
                        World_Map.SetTile(position, TopCenter);
                        Generate_beach_location(position.x, position.y);
                    }
                    if (tileToPlace == LeftTopCorner)
                    {
                       
                        World_Map.SetTile(position, LeftTopCorner);
                        Generate_beach_location(position.x, position.y);

                    }
                    if (tileToPlace == RightTopCorner)
                    {
                   
                        World_Map.SetTile(position, RightTopCorner);
                        Generate_beach_location(position.x, position.y);
                    }
                    if (tileToPlace == LeftCenter)
                    {
                  
                        World_Map.SetTile(position, LeftCenter);
                        Generate_beach_location(position.x, position.y);
                    }
                    if (tileToPlace == RightCenter)
                    {
                       
                        World_Map.SetTile(position, RightCenter);
                        Generate_beach_location(position.x, position.y);


                    }

                }

            }
        }
    }

    public void Generate_mountain_location(int x, int y)
    {
        if(Current_region_Biome == Biome.Haunted)
        {
            World_Map_Location New_Location = new World_Map_Location();
            New_Location.x = x;
            New_Location.y = y;
            New_Location.type = Location_Type.mountain;
            New_Location.biome = Biome.Haunted_Hills;
            Location_Map.SetTile(new Vector3Int(x, y, 0), mountain);
            World_Map_Locations.Add(New_Location);
        }
    }
    public void Generate_beach_location(int x, int y)
    {
        if (Current_region_Biome == Biome.Haunted)
        {
            World_Map_Location New_Location = new World_Map_Location();
            New_Location.x = x;
            New_Location.y = y;
            New_Location.type = Location_Type.beach;
            New_Location.biome = Biome.Haunted_Beach;
            Location_Map.SetTile(new Vector3Int(x, y, 0), beach);
            World_Map_Locations.Add(New_Location);

        }
    }
    public void Generate_village_location(int x, int y)
    {
        if (Current_region_Biome == Biome.Haunted)
        {
            World_Map_Location New_Location = new World_Map_Location();
            New_Location.x = x;
            New_Location.y = y;
            New_Location.type = Location_Type.village;
            New_Location.biome = Biome.Haunted_Village;
            Location_Map.SetTile(new Vector3Int(x, y, 0),town);

            World_Map_Locations.Add(New_Location);
        }
    }
    public void Generate_flat_location(int x, int y, Location_Type type)
    {
        if (Current_region_Biome == Biome.Haunted)
        {
            World_Map_Location New_Location = new World_Map_Location();
            New_Location.x = x;
            New_Location.y = y;
            New_Location.type = type;
            if(type == Location_Type.grassland)
            {
                New_Location.biome = Biome.Haunted_Grassland;
                Location_Map.SetTile(new Vector3Int(x, y, 0), flat_01);
                World_Map_Locations.Add(New_Location);

            }
            if (type == Location_Type.forest)
            {
                New_Location.biome = Biome.Forest;
                Location_Map.SetTile(new Vector3Int(x, y, 0), flat_01);
                World_Map_Locations.Add(New_Location);

            }
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