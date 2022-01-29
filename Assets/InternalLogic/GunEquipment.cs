using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class GunEquipment : ScriptableObject, IGunEquipment
	{
		[SerializeField]
		private string description;
		[SerializeField]
		private List<GunModifier> modifiers;
		[SerializeField]
		private string equipmentName;
		[SerializeField]
		private GameObject inventoryHusk;

		public string Name => equipmentName;

		public string Description => description;

		public GameObject InventoryHusk => inventoryHusk;

		public IEnumerable<IGunModifier> Modifiers => modifiers;
	}
}