using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InternalLogic
{
	public interface IBulletEquipConfig
	{
		public string Name { get; }
		public string Description { get; }
		public GameObject BulletPrefab { get; }
		public Dictionary<GunProperties, float> BaseStats { get; }
	}

	
}