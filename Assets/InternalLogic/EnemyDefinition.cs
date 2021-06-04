#nullable enable
using InternalLogic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDefinition : ScriptableObject, IEnemyConfig
{
	public string enemyName;
	public int level;
	public int difficultyScore;
	public int maxConcurrent;
	public GameObject menuHuskPrefab;
	public GameObject enemyPrefab;
	public BulletDefinition? asBulletEquipment;
	public GunEquipmentConfiguration? asGunEquipment;
	public BodyEquipmentConfiguration? asBodyEquipment;
	public int basePrice;

	public string Name => enemyName;
	public int Level => level;
	public int DifficultyScore => difficultyScore;
	public int MaxConcurrent => maxConcurrent;
	public GameObject MenuHuskPrefab => menuHuskPrefab;
	public GameObject EnemyPrefab => enemyPrefab;
	public IBulletEquipConfig? AsBulletEquipment => asBulletEquipment;
	public IGunEquipConfiguration? AsGunEquipment => asGunEquipment;
	public IBodyEquipConfig? AsBodyEquipment => asBodyEquipment;
	public int BasePrice => basePrice;
}

