using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Npc_Handler : MonoBehaviour
    {
    public NpcInteractable[] people;
    public List<Vector3Int> regions;
    public Tile[] femaleSprites, maleSprites;
    public int Number_Of_Citizens = 0;
    public World_Map_Generator WorldHandler;
    public Tilemap citizenMap;
    // Use this for initialization
    void Start()
        {
        

        }
    public void Generate()
    {
        Town_Region_Generator reGen = WorldHandler.town_Region_Generator;
        for (int x = 0; x < reGen.width; x++)
        {
            for (int y = 0; y < reGen.height; y++)
            {
                if (reGen.DoorM.GetTile(new Vector3Int(x, y)) != null)
                {
                    Number_Of_Citizens++;
                    regions.Add(new Vector3Int(x, y));
                }
            }
        }
        people = new NpcInteractable[Number_Of_Citizens];
        for (int i = 0; i < Number_Of_Citizens; i++)
        {
            NpcInteractable NewCitizen = Generate_Human_Npc(regions[i]);
            people[i] = NewCitizen;
            citizenMap.SetTile(people[i].Current_Location, people[i].Personality.Still_Tile);
        }
    }
        // Update is called once per frame
        void Update()
        {

        }

      public NpcInteractable Generate_Human_Npc(Vector3Int Location)
    {
        NpcInteractable npcInteractable = new NpcInteractable();
        Ai_Personality personality = new Ai_Personality();
        personality._name = WorldHandler.deity_Generator.GenerateGodlyName();
        int RanRang = Random.Range(0, 2);
        if(RanRang > 0)
        {
            personality.gender = 1;
            RanRang = Random.Range(0, femaleSprites.Length);
            personality.Still_Tile = femaleSprites[RanRang];
        }
        if(RanRang == 0)
        {
            personality.gender = 0;
            RanRang = Random.Range(0,maleSprites.Length);
            personality.Still_Tile = maleSprites[RanRang];
        }
        personality.occupation = Npc_Type.citizen;
        npcInteractable.Energy = 10;
        npcInteractable.health = 10;
        npcInteractable.nourishment = 10;
        npcInteractable.Personality = personality;
        npcInteractable.Current_Location = Location;
        return npcInteractable;
    }

    }

[System.Serializable]
public struct NpcInteractable
{
    public Ai_Personality Personality;
    [Range(0,10)]
    public int Energy;
    [Range(0, 10)]
    public int nourishment;
    [Range(0, 10)]
    public int health;
    public Vector3Int Current_Location;
}