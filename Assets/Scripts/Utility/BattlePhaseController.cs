using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
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

	private void Start()
	{
		attackButton.onClick.AddListener(() => AttackEnemy(selectedHero));
		BattleStateController.OnBattleStateChanged += OnBattleStateChanged;
	}

	public void InitializeBattle(List<Hero> selectedHeroes)
	{
		InitHeroes(selectedHeroes);
		InitEnemy();

		BattleStateController.SetBattleState(BattleState.PlayerTurn);
		attackButton.interactable = false;
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
				if (BattleStateController.CurrentBattleState == BattleState.PlayerTurn)
				{
					HandleSelection(id);
				}
			});
		}
	}

	private void InitEnemy()
	{
		var enemyData = enemySO.Enemies[0];
		enemy.SetEnemyData(enemyData);
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

	private void AttackEnemy(Hero hero)
	{
		BattleStateController.SetBattleState(BattleState.Attacking);
		DoHeroAttack(hero);
	}

	private void DoHeroAttack(Hero hero)
	{
		hero.Attack(enemy, () => PassTurn(BattleState.EnemyTurn));
	}

	private void DoEnemyAttack(Hero hero)
	{
		enemy.Attack(hero, () =>
		{
			if (aliveHeroes.Count == 0)
			{
				// Game Over
				Debug.LogError("GAME OVER");
				Time.timeScale = 0f;
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
}