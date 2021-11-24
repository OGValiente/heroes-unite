using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattlePhaseController : MonoBehaviour
{
    [SerializeField] private List<Hero> heroSlots;
    [SerializeField] private Enemy currentEnemy;
	[SerializeField] private EnemySO enemySO;
	[SerializeField] private Button attackButton;

	private Hero selectedHero;
	private BattleState battleState;

    public void InitializeBattle(List<Hero> selectedHeroes)
	{
		attackButton.onClick.AddListener(() => AttackEnemy(selectedHero));
		InitHeroes(selectedHeroes);
        InitEnemy();
		
		battleState = BattleState.PlayerTurn;
	}

	private void InitEnemy()
	{
		var enemy = enemySO.Enemies[0];
		currentEnemy.SetEnemyData(enemy);
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

			if (select)
			{
				selectedHero = hero;
				attackButton.interactable = true;
			}
        }
    }

    public void SetInteraction(bool allowed)
    {
        foreach (var hero in heroSlots)
        {
            hero.Button.interactable = allowed;
        }
    }

	private void AttackEnemy(Hero hero)
	{
		battleState = BattleState.Attacking;
		// TODO: Do tween towards enemy, attack/slash animation. Show damage dealt on top of the enemy.
		Debug.LogWarning($"{hero.HeroData.Name} deals {hero.HeroData.AttackPower} to Enemy");
		Debug.LogWarning($"Enemy is left with {currentEnemy.EnemyData.Health} - {hero.HeroData.AttackPower} = {currentEnemy.EnemyData.Health - hero.HeroData.AttackPower} HP");
		currentEnemy.OnAttacked?.Invoke(hero.HeroData.AttackPower);
	}
}
