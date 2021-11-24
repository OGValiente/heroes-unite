using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	
    public EnemyData EnemyData { get; set; }
    public Action OnAttack;
    public Action<int> OnAttacked;

	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	private void Start()
	{
		OnAttacked += OnEnemyAttacked;
	}

	public void Attack()
    {
        // TODO: Pick a random hero from the battlefield and attack.
        OnAttack?.Invoke();
    }

    public void SetEnemyData(EnemyData data)
    {
        EnemyData = data;
		healthBar.slider.maxValue = data.Health;
		healthBar.slider.value = data.Health;
	}

    public void SetEnemyHealth(int health)
    {
        EnemyData.Health = health;
    }

    public void SetEnemyAttackPower(int power)
    {
        EnemyData.AttackPower = power;
    }

	public void SetEnemyColor(Color color)
	{
		EnemyData.Color = color;
		image.color = color;
	}

	private void OnEnemyAttacked(int damageDealt)
	{
		var remainingHealth = EnemyData.Health - damageDealt;
		healthBar.SetHealth(remainingHealth);
		SetEnemyHealth(remainingHealth);
	}
}
