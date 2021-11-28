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
        this.name.text = $"Name: {name}";
        this.level.text = $"Level: {level}";
        this.attackPower.text = $"Attack Power: {attackPower}";
        this.experience.text = $"Experience: {experience}";
    }

    public void ClearFields()
    {
		this.name.text = string.Empty;
		this.level.text = string.Empty;
		this.attackPower.text = string.Empty;
		this.experience.text = string.Empty;
    }
}
