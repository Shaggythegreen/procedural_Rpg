using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu]
public class Mountain_region_Presets : ScriptableObject
{
    public string _name;
    public Biome biome;
    public Tile LeftTopCorner, RightTopCorner, RightTopCornerEdge, leftTopCornerEdge, TopCenter, TopEdge, LeftCenter, LeftEdge, Center, RightCenter, RightEdge, BottomCenter, BottomLeftCorner, BottomRightCorner, floorTile, left_path, right_path, center_Path, cave;
    public Building_Object Layouts;
    public int height,width,tree_Smoothness, plantsmoothness;
    
}
