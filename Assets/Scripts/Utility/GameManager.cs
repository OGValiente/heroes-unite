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
    [SerializeField] private GameObject heroSelectionCanvas;
    [SerializeField] private GameObject battleCanvas;
    [SerializeField] private HeroSelectionPhaseController heroSelectionPhaseController;
    [SerializeField] private BattlePhaseController battlePhaseController;

    private void Start()
    {
        heroSelectionPhaseController.OnBattleButtonClicked += PrepareBattle;
    }

    private void PrepareBattle()
    {
        heroSelectionCanvas.SetActive(false);
        battleCanvas.SetActive(true);
        
        battlePhaseController.InitializeBattle(heroSelectionPhaseController.SelectedHeroes);
        GameStateController.CurrentGameState = GameState.Battle;
    }
}
