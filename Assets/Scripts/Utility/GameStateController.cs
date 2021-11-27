using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
	public static GameState CurrentGameState { get; private set; }
	public static Action<GameState> OnGameStateChanged;

	public static void SetGameState(GameState state)
	{
		CurrentGameState = state;
		OnGameStateChanged?.Invoke(state);
	}
}
