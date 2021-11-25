using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattlePhaseController : MonoBehaviour
{
    [SerializeField] private List<Hero> heroSlots;
    [SerializeField] private Enemy currentEnemy;
	[SerializeField] private EnemySO enemySO;
	[SerializeField] private Button attackButton;

	private Hero selectedHero;
	private BattleState battleState;

	private void Start()
	{
		attackButton.onClick.AddListener(() => AttackEnemy(selectedHero));
	}

	public void InitializeBattle(List<Hero> selectedHeroes)
	{
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
            heroSlots[i].SetHeroData(selectedHeroes[i].Data);
            heroSlots[i].SetHeroColor(selectedHeroes[i].Data.Color);
            
            var id = selectedHeroes[i].Data.Id;
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
            var select = hero.Data.Id == id; 
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

		attackButton.interactable = allowed;
	}

	private void AttackEnemy(Hero hero)
	{
		// TODO: Do tween towards enemy, attack/slash animation. Show damage dealt on top of the enemy.
		battleState = BattleState.Attacking;
		currentEnemy.OnAttacked?.Invoke(hero.Data.AttackPower);

		battleState = BattleState.EnemyTurn;
		PassTurn();
	}

	private void PassTurn()
	{
		if (battleState == BattleState.EnemyTurn)
		{
			SetInteraction(false);
			var heroToAttack = PickRandomHero();
			while (!heroToAttack.IsAlive)
			{
				heroToAttack = PickRandomHero();
			}
			AttackHero(heroToAttack);
		}
		else
		{
			SetInteraction(true);
		}
	}

	private void AttackHero(Hero hero)
	{
		currentEnemy.Attack(hero);

		if (hero.Data.Health < 1)
		{
			// Dead
			hero.gameObject.SetActive(false);
		}

		battleState = BattleState.PlayerTurn;
		PassTurn();
	}

	private Hero PickRandomHero()
	{
		var heroIndex = Random.Range(0, heroSlots.Count);
		return heroSlots[heroIndex];
	}
}
