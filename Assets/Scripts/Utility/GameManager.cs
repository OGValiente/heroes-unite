using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup heroSelectionCanvas;
    [SerializeField] private CanvasGroup battleCanvas;
	[SerializeField] private CanvasGroup resultCanvas;
    [SerializeField] private HeroSelectionPhaseController heroSelectionPhaseController;
    [SerializeField] private BattlePhaseController battlePhaseController;
	[SerializeField] private BattleResultPanel battleResultPanel;
	[SerializeField] private HeroCollectionSO heroCollection;

    private void Start()
    {
        heroSelectionPhaseController.BattleButton.onClick.AddListener(PrepareBattle);
		battlePhaseController.OnBattleWon += OnBattleWon;
		battlePhaseController.OnBattleLost += OnBattleLost;
		battlePhaseController.OnFiveBattlesMade += AddNewHeroToCollection;
		GameStateController.OnGameStateChanged += OnGameStateChanged;
	}

	private void PrepareBattle()
    {
		battlePhaseController.InitializeBattle(heroSelectionPhaseController.SelectedHeroes);
        GameStateController.SetGameState(GameState.Battle);
    }

	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.HeroSelection:
				battleCanvas.alpha = 0f;
				resultCanvas.alpha = 0f;
				heroSelectionCanvas.alpha = 1f;
				heroSelectionPhaseController.InitializeHeroSelection();
				break;
			case GameState.Battle:
				heroSelectionCanvas.alpha = 0f;
				resultCanvas.alpha = 0f;
				battleCanvas.alpha = 1f;
				break;
			case GameState.Result:
				heroSelectionCanvas.alpha = 0f;
				battleCanvas.alpha = 0f;
				resultCanvas.alpha = 1f;
				battlePhaseController.IncrementBattlesMadeCount();
				break;
			case GameState.Idle:
				break;
			default:
				Debug.LogError("Unknown game state.");
				break;
		}
	}

	private void OnBattleLost()
	{
		battleResultPanel.Init(false);
	}

	private void OnBattleWon(List<Hero> heroes)
	{
		battleResultPanel.Init(true);
		battleResultPanel.UpdateHeroStats(heroes);
	}

	private void AddNewHeroToCollection()
	{
		if (PlayerData.HasReachedHeroLimit)
		{
			return;
		}
		
		var hero = GetRandomHeroFromCollection();
		while (PlayerData.OwnedHeroIds.Contains(hero.Id))
		{
			hero = GetRandomHeroFromCollection();
		}
		
		PlayerData.OwnedHeroes.Add(hero);
		PlayerData.OwnedHeroIds.Add(hero.Id);
	}

	private HeroData GetRandomHeroFromCollection()
	{
		int index = Random.Range(0, heroCollection.Heroes.Length - 1);
		return heroCollection.Heroes[index];
	}
	
	// TODO: Add attacking effects & indicators
	// TODO: Able to save the game between sessions
	// TODO: Touch controls on mobile
	// TODO: Polish game visuals
	// TODO: Remove unused libraries
}
