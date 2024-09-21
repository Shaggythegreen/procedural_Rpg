using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Animations;


[System.Serializable]
public enum Biome
{
    Haunted,Forest, Grassland, Desert,Frozen,Blasted,Beach,Grass_Mountains,Blasted_Mountains,Blasted_Beach,Cold_Beach, Cold_Mountain, Cold_Desert,Haunted_Grassland,Haunted_Hills,Haunted_Beach,Haunted_Forest,Haunted_Village
}
[System.Serializable]
public enum Temperature
{
    frozen,cold, cool, warm, hot, scorching,
}


[System.Serializable]
public struct Building_Object
{
    public int Height;
    public int Width;
    public Tile WallBlc, WallBrc, WallLeft, WallRight, WallTlc, WallTrc, WallCenter, floorTile, DoorTile;

}

public enum Location_Type
{
   forest,grassland,desert,mountain,beach,cave,dungeon,village
}
[System.Serializable]

public enum Godly_Domain
{
    fire, water, stone, air, blood, madess, sanity, joy, depression, stories, crime, law, nature, sickness, health, storms, combat,werefolk, life, death, darkness, light, knowledge, ignorance, narcotics, art, engineering, animals, family, protection, fertility, time, monsters, ressurection, luck, craft
} 



[System.Serializable]

public struct World_Deity
{
    public string Name;
    public List<string> Domains;
}


[System.Serializable]
public enum Composition
{
    Wet,
    Dry,
    Gas,
    Rocky,
    Icy
}
[System.Serializable]

public enum Non_Sentient_Species_Type
{
    mammalian, reptilian, avian, arthropoid, amphibian, fishlike, wormling, plant, fungus
}
[System.Serializable]
public struct Species
{
    [Range(1, 7)]
    public int size;
    public int limbs;
    public bool hasWings;
    public bool hasLimbs;
    public Biome Environment;
    public string Description;
}
[System.Serializable]
public struct Sentient_species
{
    
    public Color color;
    public string name;
    public Sprite head;
    public Sprite body;
    public Biome preferredClimate;
    public Species_Type type;
}
[System.Serializable]
public enum Species_Type
{
    Humanoid, Reptiloid, Avian, Arthropoid, Wormling, Fungaloid, Mermoid
}

public enum Race

{ 
    human,elf,goblin,bugling,wormoid,zombie,vampire
}

[System.Serializable]

public enum Creature_Descriptor_Type 
{
    body,limb_amount,color,creature_type
}
[System.Serializable]

public enum Npc_Type
{
    cultist,mayor,sherrif,detective,raider,barkeep,trader,farmer,citizen
}
[System.Serializable]
public enum monster_Type
{
    Ram_Boss

}
[System.Serializable]

public enum BossState
{
    Stall,Stunned,Attack_One,Attack_Two,Idle,Exploring
}
[System.Serializable]
public struct Ai_Personality
{
    public Npc_Type occupation;
    public string _name;
    public Tile Still_Tile;
    [Range(0,1)]
    public int gender;
    

}
[System.Serializable]
public enum Npc_Action
{
    move,deposit,gather,eat,sleep,talk
}

[System.Serializable]
public struct World_Map_Location
{
    public string locationName;
    public Biome biome;
    public int x, y;
    public Location_Type type;
}
