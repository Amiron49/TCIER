#nullable enable
using UnityEngine;

namespace InternalLogic
{
	public interface IEnemyConfiguration
	{
		string Name { get; }
		int Level { get; }
		int DifficultyScore { get; }
		int MaxConcurrent { get; }
		GameObject MenuHuskPrefab { get; }
		GameObject EnemyPrefab { get; }
		IBulletEquipConfig? AsBulletEquipment { get; }
		IGunEquipment? AsGunEquipment { get; }
		IBodyEquipment? AsBodyEquipment { get; }
		int BasePrice { get; }
	}
}