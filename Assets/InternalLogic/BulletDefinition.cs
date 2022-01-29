using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class BulletDefinition : ScriptableObject, IBulletEquipConfig
	{
		[SerializeField]
		private string description;
		[SerializeField]
		private GameObject bulletPrefab;
		[SerializeField]
		private string bulletName;
		[SerializeField] 
		private List<IGunProperty> baseStats = new List<IGunProperty>();

		public string Name => bulletName;
		public string Description => description;
		public GameObject BulletPrefab => bulletPrefab;
		public Dictionary<GunProperties, float> BaseStats => baseStats.ToDictionary(x => x.Property, x => x.Value);
	}
}

