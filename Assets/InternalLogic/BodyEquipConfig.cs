using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class BodyEquipConfig : ScriptableObject, IBodyEquipConfig
	{
		public string description;
		public IEnumerable<IBodyModifier> modifiers;

		public string Description => description;
		public IEnumerable<IBodyModifier> Modifiers => modifiers;
	}
}