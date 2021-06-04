using UnityEngine;

namespace InternalLogic
{
	public interface IBulletEquipConfig
	{
		public string Name { get; }
		public string Description { get; }
		public GameObject BulletPrefab { get; }
		public float Cooldown { get; }
		public bool Channeled { get; }
		public float WindUp { get; }
		public float WindDown { get; }
	}
}