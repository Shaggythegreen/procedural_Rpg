using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deity/Domain")]
public class Deities_Domain : ScriptableObject
    {
     public Biome Biome;
     public List <string> domainNames = new List <string> ();
    }
