using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HeroSelectionController HeroSelectionController;
    public Button BattleButton;
    public GameState GameState;

    private int selectionCount;

    private void Awake()
    {
        HeroSelectionController.OnStartPhaseCompleted += InitEvents;
    }

    private void InitEvents()
    {
        foreach (var hero in HeroSelectionController.Heroes)
        {
            hero.OnHeroSelected += OnHeroSelected;
            hero.OnHeroDeselected += OnHeroDeselected;
        }
    }

    private void OnHeroSelected()
    {
        Debug.LogWarning("OnHeroSelected");
        selectionCount++;
        Debug.LogWarning($"Selection count: {selectionCount}");
        if (selectionCount == 3)
        {
            BattleButton.interactable = true;
        }
    }

    private void OnHeroDeselected()
    {
        Debug.LogWarning("OnHeroDeselected");
        selectionCount--;
        Debug.LogWarning($"Selection count: {selectionCount}");
        if (selectionCount < 3)
        {
            BattleButton.interactable = false;
        }
    }
}
