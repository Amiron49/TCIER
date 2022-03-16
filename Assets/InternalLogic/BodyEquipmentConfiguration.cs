using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	[CreateAssetMenu]
	public class BodyEquipmentConfiguration : ScriptableObject, IBodyEquipment
	{
		[SerializeField] private string description;
		[SerializeField] private List<BodyModifier> modifiers;
		[SerializeField] private GameObject inventoryHusk;
		[SerializeField] private string equipmentName;

		public string Name => equipmentName;
		public string Description => description;
		public IEnumerable<IPropertyModifier> Modifiers => ModifiersTyped;
		public GameObject InventoryHusk => inventoryHusk;
		public IEnumerable<IBodyModifier> ModifiersTyped => modifiers;
	}
}