using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
    public static GameState CurrentGameState { get; set; }
	public static Action<GameState> OnGameStateChanged;
}