using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu(menuName = "Definitions/GunEquipmentConfiguration")]
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
		public IEnumerable<IPropertyModifier> Modifiers => ModifiersTyped;

		public GameObject InventoryHusk => inventoryHusk;

		public IEnumerable<IGunModifier> ModifiersTyped => modifiers;
	}
}