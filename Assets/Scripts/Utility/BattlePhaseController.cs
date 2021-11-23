using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePhaseController : MonoBehaviour
{
    [SerializeField] private List<Hero> heroSlots;
    [SerializeField] private Enemy enemy;

    public void InitializeBattle(List<Hero> selectedHeroes)
    {
        InitHeroes(selectedHeroes);
        InitEnemy();
    }

    private void InitEnemy()
    {
        // TODO: Init enemy object
    }

    private void InitHeroes(List<Hero> selectedHeroes)
    {
        for (int i = 0; i < heroSlots.Count; i++)
        {
            heroSlots[i].SetHeroData(selectedHeroes[i].HeroData);
            heroSlots[i].SetHeroColor(selectedHeroes[i].HeroData.Color);
            
            var id = selectedHeroes[i].HeroData.Id;
            heroSlots[i].Button.onClick.AddListener(() =>
            {
                HandleSelection(id);
            });
        }
    }

    private void HandleSelection(int id)
    {
        foreach (var hero in heroSlots)
        {
            var select = hero.HeroData.Id == id; 
            hero.SelectInBattle(select);
        }
    }

    public void SetInteraction(bool allowed)
    {
        foreach (var hero in heroSlots)
        {
            hero.Button.interactable = allowed;
        }
    }
}
