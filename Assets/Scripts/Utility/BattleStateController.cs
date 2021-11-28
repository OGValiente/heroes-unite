using System;

public static class BattleStateController
{
	public static BattleState CurrentBattleState { get; private set; }
	public static Action<BattleState> OnBattleStateChanged;

	public static void SetBattleState(BattleState state)
	{
		CurrentBattleState = state;
		OnBattleStateChanged?.Invoke(state);
	}
}