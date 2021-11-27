using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPanel : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private Text title;
	[SerializeField] private Button button;
	[SerializeField] private Text buttonText;
	[SerializeField] private string winTitle;
	[SerializeField] private string loseTitle;
	[SerializeField] private string winButtonText;
	[SerializeField] private string loseButtonText;

    public void Init(bool win)
	{
		BlockRaycasts(true);
		GameStateController.SetGameState(GameState.Result);
		title.text = win ? winTitle : loseTitle;
		buttonText.text = win ? winButtonText : loseButtonText;
		
		button.onClick.AddListener(() =>
		{
			BlockRaycasts(false);
			GameStateController.SetGameState(GameState.HeroSelection);
		});
	}

	public void UpdateHeroStats(List<Hero> heroes)
	{
		foreach (var hero in heroes)
		{
			hero.IncrementHeroExperience();
		}
	}

	public void BlockRaycasts(bool block)
	{
		canvasGroup.blocksRaycasts = block;
	}
}
