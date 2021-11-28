using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private DamageIndicator damageIndicator;
	
    public EnemyData Data { get; private set; }
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
		healthBar.SetMaximumHealth(data.Health);
		healthBar.SetRemainingHealth(data.Health);
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
				hero.DisplayDamageDealt(Data.AttackPower);
				hero.SetHeroRemainingHealth(Math.Max(0, hero.RemainingHealth - Data.AttackPower));
				transform
					.DOMove(oriPos, .75f)
					.SetEase(Ease.OutQuart)
					.SetDelay(.05f)
					.OnComplete(() => onAnimComplete.Invoke(!hero.IsAlive));
			});
    }

	private void OnEnemyAttacked(int damageDealt)
	{
		damageIndicator.DisplayDamageDealt(damageDealt);
		remainingHealth -= damageDealt;
		healthBar.SetRemainingHealth(remainingHealth);

		if (remainingHealth < 1)
		{
			gameObject.SetActive(false);
		}
	}
}
