using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	
    public EnemyData Data { get; private set; }
    public Action<int> OnAttack;
    public Action<int> OnAttacked;

	private Image image;
	private int remainingHealth;
	public bool IsDead => remainingHealth <= 0;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Start()
	{
		OnAttacked += OnEnemyAttacked;
	}

	public void SetEnemyData(EnemyData data)
	{
		Data = data;
		healthBar.slider.maxValue = data.Health;
		healthBar.slider.value = data.Health;
		remainingHealth = data.Health;
	}

	public void SetEnemyColor(Color color)
	{
		Data.Color = color;
		image.color = color;
	}

	public void Attack(Hero hero, Action<bool> onAnimComplete)
    {
		var oriPos = transform.position;
		var offset = new Vector3(2, 0, 0);
		var targetPos = hero.transform.position + offset; 
		transform
			.DOMove(targetPos, .75f)
			.SetEase(Ease.InBack)
			.OnComplete(() =>
			{
				// TODO: Add damage dealt indicator
				// TODO: Add strike particle/animation
				// TODO: Add screen shake
				OnAttack?.Invoke(Data.AttackPower);
				hero.SetHeroHealth(hero.Data.Health - Data.AttackPower);
				transform
					.DOMove(oriPos, .75f)
					.SetEase(Ease.OutQuart)
					.SetDelay(.05f)
					.OnComplete(() => onAnimComplete.Invoke(!hero.IsAlive));
			});
    }

	private void OnEnemyAttacked(int damageDealt)
	{
		remainingHealth -= damageDealt;
		healthBar.SetHealth(remainingHealth);

		if (remainingHealth < 1)
		{
			gameObject.SetActive(false);
		}
	}
}
