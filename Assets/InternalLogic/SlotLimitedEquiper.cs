#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternalLogic
{
	public abstract class SlotLimitedEquipper<TEquipment, TPropertyModifer, TProperties> : ISlotLimitedEquipper
		where TEquipment : IEquipment<TPropertyModifer, TProperties>
		where TPropertyModifer : IPropertyModifier<TProperties>
		where TProperties : Enum
	{
		protected readonly Inventory Inventory;
		public int Capacity => CalculateSlots(PropertiesTyped);
		protected abstract TProperties SlotChangingProperty { get; }
		protected List<TEquipment> EquippedTyped = new();
		public IReadOnlyList<IEquipment> Equipped => EquippedTyped.Cast<IEquipment>().ToList();
		public Dictionary<string, float> Properties => PropertiesTyped.ToStringDictionary();
		protected Dictionary<TProperties, float> PropertiesTyped { get; private set; } = new();

		public event EventHandler? OnPropertyChange;

		protected SlotLimitedEquipper(Inventory inventory)
		{
			Inventory = inventory;
		}

		private string? EquipTyped(TEquipment configuration)
		{
			if (!HasSpace())
				return "No free slots available";

			EquippedTyped.Add(configuration);
			Inventory.Take(configuration);
			RecalculateProperties();

			return null;
		}

		protected virtual void RecalculateProperties()
		{
			PropertiesTyped = BaseStatsTyped().WithAddedModifiers(EquippedTyped.SelectMany(x => x.ModifiersTyped));
			OnPropertyChange?.Invoke(this, EventArgs.Empty);
		}

		protected Dictionary<TProperties, float> PreviewEquipTyped(TEquipment gunEquipment)
		{
			var propertyModifiers = EquippedTyped.Concat(new[] { gunEquipment }).SelectMany(x => x.ModifiersTyped);
			return BaseStatsTyped().WithAddedModifiers(propertyModifiers);
		}

		protected abstract Dictionary<TProperties, float> BaseStatsTyped();

		public string? Equip(IEquipment configuration)
		{
			return EquipTyped((TEquipment)configuration);
		}

		public Dictionary<string, float> PreviewEquip(IEquipment gunEquipment)
		{
			return PreviewEquipTyped((TEquipment)gunEquipment).ToStringDictionary();
		}

		public string? UnEquip(int slotIndex)
		{
			if (EquippedTyped.Count < slotIndex + 1)
				return "Nothing is in this slot";

			var configAtSlot = EquippedTyped[slotIndex];
			var overflow = CalculateUnEquipCascade(EquippedTyped, new List<int> { slotIndex });
			var without = EquippedTyped.Without(new List<int> { slotIndex }).ToList();
			var withoutOverflow = without.Take(without.Count - overflow).ToList();
			var removedItems = without.AsEnumerable().Reverse().Take(overflow);

			EquippedTyped = withoutOverflow;
			Inventory.Return<TEquipment, TPropertyModifer, TProperties>(configAtSlot);

			foreach (var removedItem in removedItems)
				Inventory.Return<TEquipment, TPropertyModifer, TProperties>(removedItem);

			RecalculateProperties();

			return null;
		}

		public (Dictionary<string, float> Properties, int UnEquippedItemsCount, int Capacity) PreviewUnEquip(int slotIndex)
		{
			var (properties, unEquippedItemsCount, capacity) = PreviewUnEquipTyped(slotIndex);
			return (properties.ToStringDictionary(), unEquippedItemsCount, capacity);
		}
		
		private (Dictionary<TProperties, float> Properties, int UnEquippedItemsCount, int Capacity) PreviewUnEquipTyped(int slotIndex)
		{
			var overflow = CalculateUnEquipCascade(EquippedTyped, new List<int> { slotIndex });
			var without = EquippedTyped.Without(new List<int> { slotIndex }).ToList();
			var withoutOverflow = without.Take(without.Count - overflow).ToList();

			var properties = BaseStatsTyped().WithAddedModifiers(withoutOverflow.SelectMany(x => x.ModifiersTyped));
			var capacity = CalculateSlots(properties);
			return (properties, withoutOverflow.Count, capacity);
		}

		private int CalculateUnEquipCascade(IList<TEquipment> equipped, List<int> slotIndex)
		{
			var configsToBeRemoved = equipped.Where((x, i) => slotIndex.Contains(i));

			var isSlotInfluencing = configsToBeRemoved.SelectMany(x => x.ModifiersTyped).Any(x => x.ModifiesTyped.Equals(SlotChangingProperty) && x.Value > 0);
			if (!isSlotInfluencing)
				return 0;

			var without = equipped.Without(slotIndex);

			var newMaxCount = CalculateSlots(BaseStatsTyped().WithEquipment<TPropertyModifer, TProperties, TEquipment>(without));
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

		private int CalculateSlots(IReadOnlyDictionary<TProperties, float> modifiers)
		{
			var slots = modifiers[SlotChangingProperty];
			return (int)Math.Pow(slots, 2);
		}
		
		public abstract Dictionary<string, bool> IsPositiveChangeMap { get; }
	}

	public interface ISlotLimitedEquipper
	{
		public IReadOnlyList<IEquipment> Equipped { get; }
		public Dictionary<string, float> Properties { get; }
		public event EventHandler? OnPropertyChange;
		public string? Equip(IEquipment configuration);
		public Dictionary<string, float> PreviewEquip(IEquipment gunEquipment);
		public string? UnEquip(int slotIndex);
		public (Dictionary<string, float> Properties, int UnEquippedItemsCount, int Capacity) PreviewUnEquip(int slotIndex);
		public Dictionary<string, bool> IsPositiveChangeMap { get; }
		int Capacity { get; }
	}

	public static class EnumHelper
	{
		public static Dictionary<string, T> ToStringDictionary<TEnum, T>(this IDictionary<TEnum, T> dictionary) where TEnum: Enum
		{
			return dictionary.ToDictionary(x => Enum.GetName(typeof(TEnum), x.Key!), x => x.Value);
		}
	}
}