using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static List<HeroData> OwnedHeroes = new List<HeroData>(10);
    public static bool HasReachedHeroLimit
    {
        get
        {
            return OwnedHeroes.Count == 10;
        }
    }
}
