using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroSelectionPhaseController : MonoBehaviour
{
    [SerializeField] private Hero heroPrototype;
    [SerializeField] private Button battleButton;

	private Dictionary<int, Hero> heroes = new Dictionary<int, Hero>(PlayerData.OwnedHeroes.Capacity);
	private int selectionCount;
    public List<Hero> SelectedHeroes;
	public Button BattleButton => battleButton;

    void Start()
    {
        GameStateController.SetGameState(GameState.HeroSelection);
    }

	public void InitializeHeroSelection()
	{
		heroPrototype.gameObject.SetActive(true);
		InitHeroes();
	}

	private void InitHeroes()
    {
        foreach (var ownedHero in PlayerData.OwnedHeroes)
        {
			if (heroes.ContainsKey(ownedHero.Id))
			{
				continue;
			}
			
            var hero = Instantiate(heroPrototype, this.transform);
            hero.SetHeroData(ownedHero);
            hero.SetHeroColor(ownedHero.Color);
            hero.Button.onClick.AddListener(() => HandleSelection(hero.Data.Id));
            hero.OnHeroSelected += OnHeroSelected;
            hero.OnHeroDeselected += OnHeroDeselected;
            heroes.Add(hero.Data.Id, hero);
        }
		
		heroPrototype.gameObject.SetActive(false);
    }

    private void HandleSelection(int id)
    {
        foreach (var hero in heroes)
        {
            var select = hero.Value.Data.Id == id; 
            hero.Value.Select(select);
        }
    }

    public void SetSelectionAllowance(bool allowed)
    {
        foreach (var hero in heroes)
        {
            hero.Value.Button.interactable = allowed ? true : hero.Value.IsSelected;
        }
    }
    
    private void OnHeroSelected(Hero hero)
    {
        SelectedHeroes.Add(hero);
        selectionCount++;
        if (selectionCount == 3)
        {
            battleButton.interactable = true;
            SetSelectionAllowance(false);
        }
    }

    private void OnHeroDeselected(Hero hero)
    {
        SelectedHeroes.Remove(hero);
        selectionCount--;
        if (selectionCount < 3)
        {
            battleButton.interactable = false;
            SetSelectionAllowance(true);
        }
    }
}
