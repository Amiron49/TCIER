using Lightning;
using UnityEngine;

[CreateAssetMenu(menuName = "OnceOf/EnemyPrefabs")]
public class EnemyPrefabs : ScriptableObject
{
	public GameObject MoneyPrefab;
	public GameObject DefaultDeathAnimation;
	public PeriodicBouncingEnemyLightningEmit PeriodicBouncingEnemyLightningEmit;
	public GameObject EnemyInfo;
}