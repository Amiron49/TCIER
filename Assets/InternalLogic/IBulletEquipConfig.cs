using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace InternalLogic
{
	public interface IBulletEquipConfig
	{
		public string Name { get; }
		public string Description { get; }
		public GameObject BulletPrefab { get; }
		[CanBeNull]
		public GameObject MenuHusk { get; }
		public Dictionary<GunProperties, float> BaseStats { get; }
	}
}