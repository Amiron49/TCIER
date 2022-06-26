using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu(menuName = "Definitions/BulletDefinition")]
	public class BulletDefinition : ScriptableObject, IBulletEquipConfig
	{
		[SerializeField]
		private string description;
		[SerializeField]
		private GameObject bulletPrefab;
		[SerializeField]
		private string bulletName;
		[SerializeField] 
		private List<GunProperty> baseStats;
        [SerializeField] 
		private GameObject menuHusk;

		public string Name => bulletName;
		public string Description => description;
		public GameObject BulletPrefab => bulletPrefab;
		[CanBeNull]
		public GameObject MenuHusk => menuHusk;
		public Dictionary<GunProperties, float> BaseStats => baseStats.ToDictionary(x => x.Property, x => x.Value);
	}
}

