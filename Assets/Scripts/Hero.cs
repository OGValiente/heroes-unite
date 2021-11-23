using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] private GameObject heroSelectedOutline;
    [SerializeField] private FloatingHeroPanel panel;

    private Image image;
    public Button Button { get; set; }
    public HeroData HeroData { get; set; }

    public Action<Hero> OnHeroSelected;
    public Action<Hero> OnHeroDeselected;
    public bool IsSelected;
    
    private void Awake()
    {
        Button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void SetHeroId(int id)
    {
        HeroData.Id = id;
    }

    public void SetHeroName(string name)
    {
        HeroData.Name = name;
    }

    public void SetHeroHealth(int health)
    {
        HeroData.Health = health;
    }

    public void SetHeroAttackPower(int attackPower)
    {
        HeroData.AttackPower = attackPower;
    }

    public void SetHeroLevel(int level, int experience)
    {
        HeroData.Level = level; 
        HeroData.Experience = experience;
    }

    public void SetHeroColor(Color color)
    {
        HeroData.Color = color;
        image.color = color;
    }

    public void SetHeroData(HeroData heroData)
    {
        HeroData = heroData;
    }

    public void Select(bool @select)
    {
        if (@select && IsSelected)
        {
            IsSelected = false;
            OnHeroDeselected?.Invoke(this);
            heroSelectedOutline.SetActive(false);
            panel.Disable();
            return;
        }
        
        if (@select)
        {
            IsSelected = true;
            OnHeroSelected?.Invoke(this);
            heroSelectedOutline.SetActive(true);
            panel.Initialize(HeroData.Name, HeroData.Level, HeroData.AttackPower, HeroData.Experience);
        }
    }

    public void SelectInBattle(bool isSelected)
    {
        if (isSelected)
        {
            heroSelectedOutline.SetActive(true);
            panel.Initialize(HeroData.Name, HeroData.Level, HeroData.AttackPower, HeroData.Experience);
        }
        else
        {
            panel.Disable();
            heroSelectedOutline.SetActive(isSelected);
        }
    }
}
