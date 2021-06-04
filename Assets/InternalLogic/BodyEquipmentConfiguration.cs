using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class BodyEquipmentConfiguration : ScriptableObject, IBodyEquipConfig
	{
		public string description;
		public List<BodyModifier> modifiers;

		public string Description => description;
		public IEnumerable<IBodyModifier> Modifiers => modifiers;
	}
}