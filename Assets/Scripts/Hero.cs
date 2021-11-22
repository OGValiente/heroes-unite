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

    private bool isHeroSelected;
    public Action OnHeroSelected;
    public Action OnHeroDeselected;
    
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

    public void Select(bool isSelected)
    {
        // Temporary code for hero selection screen.
        if (isSelected && isHeroSelected)
        {
            OnHeroDeselected?.Invoke();
            heroSelectedOutline.SetActive(false);
            panel.Disable();
            isHeroSelected = false;
            return;
        }
        
        isHeroSelected = isSelected;
        
        if (isSelected)
        {
            OnHeroSelected?.Invoke();
            heroSelectedOutline.SetActive(true);
            panel.Initialize(HeroData.Name, HeroData.Level, HeroData.AttackPower, HeroData.Experience);
        }
        else
        {
            // Re-enable later
            //panel.Disable();
            //heroSelectedOutline.SetActive(isSelected);
        }
        
        // If GameState is "selection", allow multiple selections.
        // If GameState is "battle", allow only 1 selection.
        // Allow deselect in selection phase.
    }
}
