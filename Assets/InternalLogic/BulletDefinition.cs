using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
		private List<GunProperty> baseStats;

		public string Name => bulletName;
		public string Description => description;
		public GameObject BulletPrefab => bulletPrefab;
		[CanBeNull]
		public GameObject MenuHusk { get; } = null;
		public Dictionary<GunProperties, float> BaseStats => baseStats.ToDictionary(x => x.Property, x => x.Value);
	}
}

