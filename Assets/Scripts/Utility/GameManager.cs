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
    [SerializeField] private HeroSelectionPhaseController heroSelectionPhaseController;
    [SerializeField] private BattlePhaseController battlePhaseController;

    private void Start()
    {
        heroSelectionPhaseController.OnBattleButtonClicked += PrepareBattle;
    }

    private void PrepareBattle()
    {
        heroSelectionCanvas.alpha = 0f;
		battleCanvas.alpha = 1f;
        
        battlePhaseController.InitializeBattle(heroSelectionPhaseController.SelectedHeroes);
        GameStateController.CurrentGameState = GameState.Battle;
		GameStateController.OnGameStateChanged.Invoke(GameState.Battle);
    }
}
