using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroSelectionController : MonoBehaviour
{
    [SerializeField] private Hero heroPrototype;
    [SerializeField] private List<Hero> heroes;

    public List<Hero> Heroes => heroes;
    public Action OnStartPhaseCompleted;

    void Start()
    {
        foreach (var ownedHero in PlayerData.OwnedHeroes)
        {
            var hero = Instantiate(heroPrototype, this.transform);
            hero.SetHeroData(ownedHero);
            hero.SetHeroColor(ownedHero.Color);
            hero.Button.onClick.AddListener(() => HandleSelection(hero.HeroData.Id));
            heroes.Add(hero);
        }
        
        heroPrototype.gameObject.SetActive(false);
        
        OnStartPhaseCompleted?.Invoke();
    }

    private void HandleSelection(int id)
    {
        foreach (var hero in heroes)
        {
            hero.Select(hero.HeroData.Id == id);
        }
    }
}
