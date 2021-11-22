using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroCollectionSO", menuName = "ScriptableObjects/HeroCollectionSO", order = 1)]
public class HeroCollectionSO : ScriptableObject
{
    public Hero[] Heroes;
}
