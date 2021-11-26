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
	[SerializeField] private Enemy currentEnemy;
	[SerializeField] private EnemySO enemySO;
	[SerializeField] private Button attackButton;

	private Hero selectedHero;

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
		var enemy = enemySO.Enemies[0];
		currentEnemy.SetEnemyData(enemy);
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
		var oriPos = hero.transform.position;
		var offset = new Vector3(2, 0, 0);
		var targetPos = currentEnemy.transform.position - offset;
		hero.transform
			.DOMove(targetPos, .75f)
			.SetEase(Ease.InBack)
			.OnComplete(() =>
			{
				// TODO: Add damage dealt indicator
				// TODO: Add strike particle/animation
				// TODO: Add screen shake
				currentEnemy.OnAttacked?.Invoke(hero.Data.AttackPower);
				hero.transform
					.DOMove(oriPos, .75f)
					.SetEase(Ease.OutQuart)
					.SetDelay(.05f)
					.OnComplete(() => PassTurn(BattleState.EnemyTurn));
			});
	}

	private void DoEnemyAttack(Hero hero)
	{
		currentEnemy.Attack(hero);

		if (hero.Data.Health < 1)
		{
			// Dead
			hero.gameObject.SetActive(false);
		}

		PassTurn(BattleState.PlayerTurn);
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

	private Hero PickRandomHero()
	{
		var heroIndex = Random.Range(0, heroSlots.Count);
		return heroSlots[heroIndex];
	}

	private void PassTurn(BattleState nextState)
	{
		BattleStateController.SetBattleState(nextState);
		
		if (BattleStateController.CurrentBattleState == BattleState.EnemyTurn)
		{
			var heroToAttack = PickRandomHero();
			while (!heroToAttack.IsAlive)
			{
				heroToAttack = PickRandomHero();
			}

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