using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup heroSelectionCanvas;
    [SerializeField] private CanvasGroup battleCanvas;
	[SerializeField] private CanvasGroup resultCanvas;
    [SerializeField] private HeroSelectionPhaseController heroSelectionPhaseController;
    [SerializeField] private BattlePhaseController battlePhaseController;
	[SerializeField] private BattleResultPanel battleResultPanel;

    private void Start()
    {
        heroSelectionPhaseController.BattleButton.onClick.AddListener(PrepareBattle);
		battlePhaseController.OnBattleWon += OnBattleWon;
		battlePhaseController.OnBattleLost += OnBattleLost;
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

	private void OnBattleWon(List<Hero> obj)
	{
		battleResultPanel.Init(true);
	}
	
	// TODO: Reward the player with additional hero on battle win
	// TODO: Fix next battle
	// TODO: Add attacking effects & indicators
	// TODO: Make enemies change each battle if the battle is won
	// TODO: Able to save the game between sessions
	// TODO: Touch controls on mobile
}
