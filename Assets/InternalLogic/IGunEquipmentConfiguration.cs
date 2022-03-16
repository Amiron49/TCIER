using System.Collections.Generic;
using UnityEngine;

namespace InternalLogic
{
	public interface IGunEquipment : IEquipment<IGunModifier, GunProperties>
	{
	}

	public interface IEquipment<out TPropertyModifier, TProperties>: IEquipment where TPropertyModifier : IPropertyModifier<TProperties>
	{
		IEnumerable<TPropertyModifier> ModifiersTyped { get; }
	}

	public interface IEquipment
	{
		public string Name { get; }
		public string Description { get; }
		public IEnumerable<IPropertyModifier> Modifiers { get; }
		public GameObject InventoryHusk { get; }
	}
}