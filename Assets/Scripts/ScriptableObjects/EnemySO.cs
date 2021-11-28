using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO", order = 2)]
public class EnemySO : ScriptableObject
{
	public EnemyData[] Enemies;
}