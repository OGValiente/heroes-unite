using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattlePhaseController : MonoBehaviour
{
	[SerializeField] private List<Hero> heroSlots;
	[SerializeField] private Enemy enemy;
	[SerializeField] private EnemySO enemySO;
	[SerializeField] private Button attackButton;

	private Hero selectedHero;
	private int battlesMade;
	private List<Hero> aliveHeroes = new List<Hero>(3);
	public Action<List<Hero>> OnBattleWon;
	public Action OnBattleLost;
	public Action OnFiveBattlesMade;

	private void Start()
	{
		attackButton.onClick.AddListener(() => AttackEnemy(selectedHero));
		BattleStateController.OnBattleStateChanged += OnBattleStateChanged;
	}

	public void InitializeBattle(List<Hero> selectedHeroes)
	{
		aliveHeroes = selectedHeroes;
		InitHeroes(selectedHeroes);
		InitEnemy();

		BattleStateController.SetBattleState(BattleState.PlayerTurn);
		attackButton.interactable = false;
	}

	private void InitHeroes(List<Hero> selectedHeroes)
	{
		for (int i = 0; i < heroSlots.Count; i++)
		{
			var hero = heroSlots[i];
			hero.SetHeroData(selectedHeroes[i].Data);
			hero.SetHeroColor(selectedHeroes[i].Data.Color);
			hero.SetHeroRemainingHealth(selectedHeroes[i].Data.Health);

			var id = selectedHeroes[i].Data.Id;
			hero.Button.onClick.AddListener(() =>
			{
				if (BattleStateController.CurrentBattleState == BattleState.PlayerTurn)
				{
					HandleSelection(id);
				}
			});
			
			hero.Deselect();
			hero.gameObject.SetActive(true);
		}
	}

	private void InitEnemy()
	{
		EnemyData enemyData;
		if (battlesMade >= enemySO.Enemies.Length)
		{
			enemyData = enemySO.Enemies[Random.Range(0, battlesMade - 1)];
		}
		else
		{
			enemyData = enemySO.Enemies[battlesMade];
		}
		
		enemy.SetEnemyData(enemyData);
		enemy.SetEnemyColor(enemyData.Color);
		enemy.gameObject.SetActive(true);
	}

	private void HandleSelection(int id)
	{
		foreach (var hero in heroSlots)
		{
			var select = hero.Data.Id == id;
			hero.SelectInBattle(select);

			if (select && hero.IsAlive)
			{
				selectedHero = hero;
				attackButton.interactable = true;
			}
		}
	}

	private void AttackEnemy(Hero hero)
	{
		BattleStateController.SetBattleState(BattleState.Attacking);
		DoHeroAttack(hero);
	}

	private void DoHeroAttack(Hero hero)
	{
		hero.Attack(enemy, (bool dead) =>
		{
			if (dead)
			{
				OnBattleWon?.Invoke(aliveHeroes);
				return;
			}
			
			PassTurn(BattleState.EnemyTurn);
		});
	}

	private void DoEnemyAttack(Hero hero)
	{
		enemy.Attack(hero, (bool dead) =>
		{
			if (dead)
			{
				aliveHeroes.Remove(hero);
			}
			
			if (aliveHeroes.Count == 0)
			{
				OnBattleLost?.Invoke();
				return;
			}
			
			PassTurn(BattleState.PlayerTurn);
		});
	}

	public void SetInteraction(bool allowed)
	{
		if (GameStateController.CurrentGameState == GameState.HeroSelection)
		{
			foreach (var hero in heroSlots)
			{
				hero.Button.interactable = allowed;
			}
		}
		
		attackButton.interactable = allowed;

		if (selectedHero != null && !selectedHero.IsAlive)
		{
			attackButton.interactable = false;
		}
	}

	private Hero PickRandomHero(List<Hero> aliveHeroes)
	{
		var heroIndex = Random.Range(0, aliveHeroes.Count);
		return aliveHeroes[heroIndex];
	}

	private void PassTurn(BattleState nextState)
	{
		BattleStateController.SetBattleState(nextState);
		
		if (BattleStateController.CurrentBattleState == BattleState.EnemyTurn)
		{
			aliveHeroes.Clear();
			foreach (var hero in heroSlots)
			{
				if (hero.IsAlive)
				{
					aliveHeroes.Add(hero);
				}
			}
			
			var heroToAttack = PickRandomHero(aliveHeroes);
			DoEnemyAttack(heroToAttack);
		}
	}

	private void OnBattleStateChanged(BattleState state)
	{
		switch (state)
		{
			case BattleState.PlayerTurn:
				SetInteraction(true);
				break;
			case BattleState.Attacking:
			case BattleState.EnemyTurn:
				SetInteraction(false);
				break;
			default:
				Debug.LogError("Unknown battle state!");
				break;
		}
	}

	public void IncrementBattlesMadeCount()
	{
		battlesMade++;
		if (battlesMade % 5 == 0)
		{
			OnFiveBattlesMade?.Invoke();
		}
	}
}