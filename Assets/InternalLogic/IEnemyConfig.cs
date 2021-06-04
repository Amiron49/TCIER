#nullable enable
using UnityEngine;

namespace InternalLogic
{
	public interface IEnemyConfig
	{
		string Name { get; }
		int Level { get; }
		int DifficultyScore { get; }
		int MaxConcurrent { get; }
		GameObject MenuHuskPrefab { get; }
		GameObject EnemyPrefab { get; }
		IBulletEquipConfig? AsBulletEquipment { get; }
		IGunEquipConfiguration? AsGunEquipment { get; }
		IBodyEquipConfig? AsBodyEquipment { get; }
		int BasePrice { get; }
	}
}