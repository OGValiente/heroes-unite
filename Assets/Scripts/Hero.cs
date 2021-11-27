using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject heroSelectedOutline;
    [SerializeField] private FloatingHeroPanel panel;
	[SerializeField] private HealthBar healthBar;

    private Image image;
    public Button Button { get; set; }
    public HeroData Data { get; set; }

	private const int expRequiredToLevelUp = 5;
    public Action<Hero> OnHeroSelected;
    public Action<Hero> OnHeroDeselected;
    public bool IsSelected;
	public bool IsAlive => Data.Health > 0;
    
    private void Awake()
	{
        Button = GetComponent<Button>();
        image = GetComponent<Image>();
	}

	private void Start()
	{
		healthBar.SetActive(false);
		GameStateController.OnGameStateChanged += OnGameStateChanged;
	}

	public void SetHeroData(HeroData heroData)
	{
		Data = heroData;
		healthBar.slider.maxValue = heroData.Health;
		healthBar.slider.value = heroData.Health;
	}

    public void SetHeroHealth(int health)
    {
        Data.Health = health;
		healthBar.SetHealth(health);
		
		if (Data.Health < 1)
		{
			gameObject.SetActive(false);
		}
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
		Data.Health *= 11 / 10;
		Data.AttackPower *= 11 / 10;
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
            IsSelected = false;
            OnHeroDeselected?.Invoke(this);
            heroSelectedOutline.SetActive(false);
            panel.Disable();
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
