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
    }

    public void SetHeroAttackPower(int attackPower)
    {
        Data.AttackPower = attackPower;
    }

    public void SetHeroLevel(int level, int experience)
    {
        Data.Level = level; 
        Data.Experience = experience;
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

	private void OnGameStateChanged(GameState state)
	{
		healthBar.SetActive(state == GameState.Battle);
	}
}
