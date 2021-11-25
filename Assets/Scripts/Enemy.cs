using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	
    public EnemyData Data { get; set; }
    public Action<int> OnAttack;
    public Action<int> OnAttacked;

	private Image image;
	private int remainingHealth;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Start()
	{
		OnAttacked += OnEnemyAttacked;
	}

	public void Attack(Hero hero)
    {
		// TODO: Do tween towards hero.
		hero.SetHeroHealth(hero.Data.Health - Data.AttackPower);
        OnAttack?.Invoke(Data.AttackPower);
    }

    public void SetEnemyData(EnemyData data)
    {
        Data = data;
		healthBar.slider.maxValue = data.Health;
		healthBar.slider.value = data.Health;
		remainingHealth = data.Health;
	}

    public void SetEnemyHealth(int health)
    {
		remainingHealth = health;
    }

    public void SetEnemyAttackPower(int power)
    {
        Data.AttackPower = power;
    }

	public void SetEnemyColor(Color color)
	{
		Data.Color = color;
		image.color = color;
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
