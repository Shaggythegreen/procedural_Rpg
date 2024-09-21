using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Town_Region_Presets : ScriptableObject
{
    public string _name;
    public Biome biome;
    public Tile LeftTopCorner, RightTopCorner, TopCenter, LeftCenter, Center, RightCenter, BottomCenter, BottomLeftCorner, BottomRightCorner, floorTile;
    public Building_Object Layouts;
    public int height, width,tree_Smoothness,plantsmoothness;
    public List<Person_Npc_Culture_Presets> npc_Presets;
}
