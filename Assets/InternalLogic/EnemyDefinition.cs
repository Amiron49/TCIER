#nullable enable
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class EnemyDefinition : ScriptableObject, IEnemyConfiguration
	{
		[SerializeField]
		private string enemyName;
		[SerializeField]
		private int level;
		[SerializeField]
		private int difficultyScore;
		[SerializeField]
		private int maxConcurrent;
		[SerializeField]
		private GameObject menuHuskPrefab;
		[SerializeField]
		private GameObject enemyPrefab;
		[SerializeField]
		private BulletDefinition? asBulletEquipment;
		[SerializeField]
		private GunEquipment? asGunEquipment;
		[SerializeField]
		private BodyEquipmentConfiguration? asBodyEquipment;
		[SerializeField]
		private int basePrice;

		public string Name => enemyName;
		public int Level => level;
		public int DifficultyScore => difficultyScore;
		public int MaxConcurrent => maxConcurrent;
		public GameObject MenuHuskPrefab => menuHuskPrefab;
		public GameObject EnemyPrefab => enemyPrefab;
		public IBulletEquipConfig? AsBulletEquipment => asBulletEquipment;
		public IGunEquipment? AsGunEquipment => asGunEquipment;
		public IBodyEquipment? AsBodyEquipment => asBodyEquipment;
		public int BasePrice => basePrice;
	}
}

