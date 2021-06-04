﻿using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class GunEquipConfiguration : ScriptableObject, IGunEquipConfiguration
	{
		public string description;
		public List<GunModifier> modifiers;
		
		public string Description => description;
		public IEnumerable<IGunModifier> Modifiers => modifiers;
	}
}