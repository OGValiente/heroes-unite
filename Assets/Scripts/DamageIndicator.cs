using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageIndicator : MonoBehaviour
{
	[SerializeField] private Text damageText;
	[SerializeField] private CanvasGroup canvasGroup;

	public void DisplayDamageDealt(int damage)
	{
		damageText.text = $"-{damage}";
		canvasGroup.alpha = 1f;
		canvasGroup.DOFade(0f, 1.5f);
	}
}
