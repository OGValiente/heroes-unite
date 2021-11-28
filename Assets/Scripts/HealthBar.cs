using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;

	public void SetRemainingHealth(int health)
	{
		slider.value = health;
	}

	public void SetMaximumHealth(int health)
	{
		slider.maxValue = health;
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}
}
