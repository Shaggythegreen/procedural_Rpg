using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deity_Generator: MonoBehaviour
{
    private ClusterType previousClusterType;
    private int comboCount;
    private int consonantClusterCount;
    private int vowelClusterCount;
    private const int maxComboCount = 2;
    private const int maxConsonantClusterCount = 2;
    private const int maxVowelClusterCount = 2;
    public int maximumNameLength = 7;
    public Deities_Domain domain;

    public List< World_Deity> Deities;
    private void Start()
    {
        if(domain != null)
        {
            List<World_Deity> worldDeities = new List<World_Deity>();
            for (int i = 0; i < 10; i++)
            {
                World_Deity fortytwo = CreateNewDeity(domain);
                worldDeities.Add(fortytwo);
                
            }
            
        }
    }
    public string GenerateGodlyName()
    {
        List<string> New = new List<string>();
        string NewName = "";

        int NewLength = Random.Range(2, maximumNameLength);
        for (int i = 0; i < NewLength; i++)
        {
            ClusterType randomChoice;
            do
            {
                randomChoice = (ClusterType)Random.Range(0, 4);
            } while (!IsValidNextType(randomChoice));

            switch (randomChoice)
            {
                case ClusterType.convowelcombo:
                    conVowelCombo newConVowelCombo = (conVowelCombo)Random.Range(0, 24);
                    New.Add(newConVowelCombo.ToString());
                    previousClusterType = ClusterType.convowelcombo;
                    comboCount++;
                    break;
                case ClusterType.consenents:
                    Consenents newConsonants = (Consenents)Random.Range(0, 19);
                    New.Add(newConsonants.ToString());
                    previousClusterType = ClusterType.consenents;
                    break;
                case ClusterType.vowelcluster:
                    VowelCluster newVowelCluster = (VowelCluster)Random.Range(0, 23);
                    New.Add(newVowelCluster.ToString());
                    previousClusterType = ClusterType.vowelcluster;
                    vowelClusterCount++;
                    break;
                case ClusterType.consenentscluster:
                    ConsenentsCluster newConsonantsCluster = (ConsenentsCluster)Random.Range(0, 12);
                    New.Add(newConsonantsCluster.ToString());
                    previousClusterType = ClusterType.consenentscluster;
                    consonantClusterCount++;
                    break;
            }
        }

        foreach (var item in New)
        {
            NewName += item;
        }

        return NewName;
    }

    private bool IsValidNextType(ClusterType currentType)
    {
        if (previousClusterType == ClusterType.convowelcombo)
        {
            if (currentType == ClusterType.consenents || currentType == ClusterType.consenentscluster)
                return true;
            return false;
        }
        else if (previousClusterType == ClusterType.consenents || previousClusterType == ClusterType.consenentscluster)
        {
            if (currentType == ClusterType.vowelcluster)
                return true;
            return false;
        }
        else if (previousClusterType == ClusterType.vowelcluster)
        {
            if (currentType == ClusterType.consenents || currentType == ClusterType.consenentscluster)
                return true;
            return false;
        }
        return true;
    }

    public World_Deity CreateNewDeity(Deities_Domain domainList)
    {
        int maximumNumbTries = 15;
        World_Deity NewGod = new World_Deity();
        int NumbOfDomains = Random.Range(3, 11);

        List<string> Godly_domains = new List<string>();
        for (int i = 0; i < NumbOfDomains; i++)
        {
            int attempts = 0;

            while (attempts < maximumNumbTries)
            {
                string chosenDomain = domainList.domainNames[Random.Range(0, domainList.domainNames.Count)];
                if (!Godly_domains.Contains(chosenDomain))
                {
                    Godly_domains.Add(chosenDomain);
                    break;
                }
                attempts++;
            }

            if (attempts >= maximumNumbTries)
            {
                // Maximum number of tries reached, discard this option
                break;
            }
        }
        //if(Godly_domains.Contains("Life")&& Godly_domains.Contains("the Universe"))
        //{
        //    Godly_domains.Clear();
        //    Godly_domains.Add("Life");
        //    Godly_domains.Add("the Universe");
        //    Godly_domains.Add("Everything");
        //}
        NewGod.Domains = Godly_domains;
        NewGod.Name = GenerateGodlyName();
        return NewGod;
    }

    public bool ContainsDomain(List<Godly_Domain> Godly_domains, Godly_Domain Domain)
    {
        if (Godly_domains.Contains(Domain)) return true;
        else return false;
    }
}

public enum ClusterType
{
    vowelcluster, consenentscluster, consenents, convowelcombo
}

public enum conVowelCombo
{
    te, ta, tu, ti, de, da, id, ad, ed, et, at, it, ot, ut, uz, iz, az, ez, oz, op, po, pa, ip, pu, pe
}

public enum Consenents
{
    b, c, d, f, g, h, j, k, l, m, n, p, q, r, s, t, v, w, x, y, z,
}
public enum ConsenentsCluster
{
    th, nd, ng, gn, ch, sh, ht, cd, ct, dc, pp, tt, nm, zz
}
public enum VowelCluster
{
    aa, ae, ai, ao, au, ee, ea, ei, eo, eu, ii, ie, ia, io, iu, oo, oe, oa, oi, ou, uu, ua, ue, ui, uo
}

