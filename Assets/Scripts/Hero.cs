using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject heroSelectedOutline;
    [SerializeField] private FloatingHeroPanel panel;
	[SerializeField] private HealthBar healthBar;
	[SerializeField] private Image image;
	[SerializeField] private Button button;

    public HeroData Data { get; set; }

	private const int expRequiredToLevelUp = 5;
    public Action<Hero> OnHeroSelected;
    public Action<Hero> OnHeroDeselected;
	public Button Button => button;
    public bool IsSelected;
	public bool IsAlive => RemainingHealth > 0;
	public int RemainingHealth;

	private void Start()
	{
		healthBar.SetActive(false);
		GameStateController.OnGameStateChanged += OnGameStateChanged;
	}

	public void SetHeroData(HeroData heroData)
	{
		Data = heroData;
		healthBar.SetMaximumHealth(heroData.Health);
	}

    public void SetHeroRemainingHealth(int health)
    {
		RemainingHealth = health;
		healthBar.SetRemainingHealth(health);
		gameObject.SetActive(health > 0);
    }

    public void IncrementHeroExperience()
	{
		Data.Experience++;
		if (Data.Experience == expRequiredToLevelUp)
		{
			LevelUp();
			Data.Experience = 0;
		}
    }

	private void LevelUp()
	{
		Data.Level++;
		var newHealth = Data.Health * 1.1f;
		var newAtkPow = Data.AttackPower * 1.1f;
		Data.Health = Mathf.RoundToInt(newHealth);
		Data.AttackPower = Mathf.RoundToInt(newAtkPow);
	}

    public void SetHeroColor(Color color)
    {
        Data.Color = color;
        image.color = color;
    }

    public void Select(bool select)
    {
        if (select && IsSelected)
        {
            Deselect();
            return;
        }
        
        if (select)
        {
            IsSelected = true;
            OnHeroSelected?.Invoke(this);
            heroSelectedOutline.SetActive(true);
            panel.Initialize(Data.Name, Data.Level, Data.AttackPower, Data.Experience);
        }
    }

	public void Deselect()
	{
		IsSelected = false;
		OnHeroDeselected?.Invoke(this);
		heroSelectedOutline.SetActive(false);
		panel.Disable();
	}

    public void SelectInBattle(bool isSelected)
    {
        if (isSelected)
        {
            heroSelectedOutline.SetActive(true);
            panel.Initialize(Data.Name, Data.Level, Data.AttackPower, Data.Experience);
        }
        else
        {
            panel.Disable();
            heroSelectedOutline.SetActive(isSelected);
        }
    }

	public void Attack(Enemy enemy, Action<bool> onAnimComplete)
	{
		var oriPos = transform.position;
		var offset = new Vector3(2, 0, 0);
		var targetPos = enemy.transform.position - offset;
		transform
			.DOMove(targetPos, .75f)
			.SetEase(Ease.InBack)
			.OnComplete(() =>
			{
				// TODO: Add damage dealt indicator
				// TODO: Add strike particle/animation
				// TODO: Add screen shake
				enemy.OnAttacked?.Invoke(Data.AttackPower);
				transform
					.DOMove(oriPos, .75f)
					.SetEase(Ease.OutQuart)
					.SetDelay(.05f)
					.OnComplete(() => onAnimComplete.Invoke(enemy.IsDead));
			});
	}

	private void OnGameStateChanged(GameState state)
	{
		healthBar.SetActive(state == GameState.Battle);
	}
}
