using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHeroPanel : MonoBehaviour
{
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text level;
    [SerializeField]
    private Text attackPower;
    [SerializeField]
    private Text experience;

    public Text Name => name;
    public Text Level => level;
    public Text AttackPower => attackPower;
    public Text Experience => experience;

    public void Initialize(string name, int level, int attackPower, int experience)
    {
        UpdateFields(name, level, attackPower, experience);
        Enable();
    }

    public void Enable()
    {
        this.gameObject.SetActive(true);
    }
    
    public void Disable()
    {
        this.gameObject.SetActive(false);
        ClearFields();
    }

    public void UpdateFields(string name, int level, int attackPower, int experience)
    {
        Name.text = $"Name: {name}";
        Level.text = $"Level: {level}";
        AttackPower.text = $"Attack Power: {attackPower}";
        Experience.text = $"Experience: {experience}";
    }

    public void ClearFields()
    {
        Name.text = string.Empty;
        Level.text = string.Empty;
        AttackPower.text = string.Empty;
        Experience.text = string.Empty;
    }
}
