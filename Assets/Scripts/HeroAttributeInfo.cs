using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAttributeInfo : MonoBehaviour
{
	[SerializeField] private Text heroName;
	[SerializeField] private Text experience;
	[SerializeField] private Text level;
	[SerializeField] private Text attack;
	[SerializeField] private Text health;

	public void SetAttributes(HeroData oldData, HeroData newData)
	{
		heroName.text = oldData.Name;
		experience.text = $"{oldData.Experience} >> {newData.Experience}";
		level.text = $"{oldData.Level} >> {newData.Level}";
		attack.text = $"{oldData.AttackPower} >> {newData.AttackPower}";
		health.text = $"{oldData.Health} >> {newData.Health}";
	}
}
