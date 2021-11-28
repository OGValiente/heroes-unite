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
	[SerializeField] private GameObject attributesPanel;
	[SerializeField] private List<HeroAttributeInfo> attributeInfos;

    public void Init(bool win)
	{
		BlockRaycasts(true);
		GameStateController.SetGameState(GameState.Result);
		title.text = win ? winTitle : loseTitle;
		buttonText.text = win ? winButtonText : loseButtonText;
		attributesPanel.SetActive(win);
		
		button.onClick.AddListener(() =>
		{
			BlockRaycasts(false);
			GameStateController.SetGameState(GameState.HeroSelection);
		});
	}

	public void UpdateHeroStats(List<Hero> heroes)
	{
		for (int i = 0; i < heroes.Count; i++)
		{
			// Old data doesn't stay old.
			var oldData = heroes[i].Data;
			heroes[i].IncrementHeroExperience();
			var newData = heroes[i].Data;
			
			attributeInfos[i].SetAttributes(oldData, newData);
		}
	}

	public void BlockRaycasts(bool block)
	{
		canvasGroup.blocksRaycasts = block;
	}
}
