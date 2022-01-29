#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternalLogic
{
	public abstract class SlotLimitedInventory<TEquipment, TPropertyModifer, TProperties>
		where TEquipment : IEquipment<TPropertyModifer, TProperties>
		where TPropertyModifer : IPropertyModifier<TProperties>
		where TProperties : Enum
	{
		protected readonly Inventory Inventory;
		protected int Capacity => _equipped.Capacity;
		protected abstract TProperties SlotChangingProperty { get; }
		private List<TEquipment> _equipped = new List<TEquipment>();
		public IReadOnlyList<TEquipment> Equipped => _equipped;
		public Dictionary<TProperties, float> Properties { get; private set; } = new Dictionary<TProperties, float>();

		public event PropertiesChange OnPropertyChange;
		
		public SlotLimitedInventory(Inventory inventory)
		{
			Inventory = inventory;
		}

		public string? Equip(TEquipment configuration)
		{
			if (!HasSpace())
				return "No free slots available";

			_equipped.Add(configuration);

			RecalculateProperties();

			return null;
		}

		protected virtual void RecalculateProperties()
		{
			Properties = BaseStats().WithAddedModifiers(_equipped.SelectMany(x => x.Modifiers));
			OnPropertyChange.Invoke(this);
		}

		public Dictionary<TProperties, float> PreviewEquip(TEquipment gunEquipment)
		{
			var propertyModifiers = _equipped.Concat(new[] {gunEquipment}).SelectMany(x => x.Modifiers);
			return BaseStats().WithAddedModifiers(propertyModifiers);
		}

		protected abstract Dictionary<TProperties, float> BaseStats();

		private string? UnEquip(int slotIndex)
		{
			if (_equipped.Count < slotIndex + 1)
				return "Nothing is in this slot";

			var configAtSlot = _equipped[slotIndex];
			var overflow = CalculateUnEquipCascade(_equipped, new List<int> {slotIndex});
			var without = _equipped.Without(new List<int> {slotIndex}).ToList();
			var withoutOverflow = without.Take(without.Count - overflow).ToList();
			var removedItems = without.AsEnumerable().Reverse().Take(overflow);

			_equipped = withoutOverflow;
			Inventory.Return<TEquipment, TPropertyModifer, TProperties>(configAtSlot);

			foreach (var removedItem in removedItems)
				Inventory.Return<TEquipment, TPropertyModifer, TProperties>(removedItem);

			RecalculateProperties();

			return null;
		}

		public (Dictionary<TProperties, float> Properties, int NewLength) PreviewUnEquip(int slotIndex)
		{
			var overflow = CalculateUnEquipCascade(_equipped, new List<int> {slotIndex});
			var without = _equipped.Without(new List<int> {slotIndex}).ToList();
			var withoutOverflow = without.Take(without.Count - overflow).ToList();

			var properties = BaseStats().WithAddedModifiers(withoutOverflow.SelectMany(x => x.Modifiers));

			return (properties, withoutOverflow.Count());
		}

		private int CalculateUnEquipCascade(IList<TEquipment> equipped, List<int> slotIndex)
		{
			var configsToBeRemoved = equipped.Where((x, i) => slotIndex.Contains(i));

			var isSlotInfluencing = configsToBeRemoved.SelectMany(x => x.Modifiers).Any(x => x.Modifies.Equals(SlotChangingProperty) && x.Value > 0);
			if (!isSlotInfluencing)
				return 0;

			var without = equipped.Without(slotIndex);

			var newMaxCount = CalculateSlots(without);
			var overflow = without.Count - newMaxCount;

			if (overflow <= 0)
				return 0;

			return overflow + CalculateUnEquipCascade(without, Enumerable.Range(newMaxCount - 1, overflow).ToList());
		}

		private bool HasSpace()
		{
			return Capacity > Equipped.Count;
		}

		private bool IsOverCapacity()
		{
			return Capacity < Equipped.Count;
		}

		protected int CalculateSlots(IEnumerable<TEquipment> equip)
		{
			return (equip.SelectMany(x => x.Modifiers).Count(x => x.Modifies.Equals(SlotChangingProperty)) + 1) ^ 2;
		}
	}

	public delegate void PropertiesChange(object sender);
}