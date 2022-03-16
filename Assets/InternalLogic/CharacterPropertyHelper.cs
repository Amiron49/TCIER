using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace InternalLogic
{
	public static class CharacterPropertyHelper
	{
		public static Dictionary<TProperty, float> WithEquipment<TPropertyModifier, TProperty, TEquipment>(this Dictionary<TProperty, float> properties,
			IEnumerable<TEquipment> equipment)
			where TPropertyModifier: IPropertyModifier<TProperty>
			where TEquipment : IEquipment<TPropertyModifier, TProperty>
		{
			return properties.WithAddedModifiers(equipment.SelectMany(x => x.ModifiersTyped));
		}
		
		public static Dictionary<TProperty, float> WithAddedModifiers<TPropertyModifier, TProperty>(this Dictionary<TProperty, float> properties,
			IEnumerable<TPropertyModifier> modifiers)
			where TPropertyModifier : IPropertyModifier<TProperty>
		{
			var gunModifiers = modifiers.ToList();
			var additiveModifiers = gunModifiers.Where(x => x.How == ModifierType.Add);
			var multiplicativeModifiers = gunModifiers.Where(x => x.How == ModifierType.Multiply);

			foreach (var additiveModifier in additiveModifiers)
				properties[additiveModifier.ModifiesTyped] += additiveModifier.Value;

			foreach (var multiplicativeModifier in multiplicativeModifiers)
				properties[multiplicativeModifier.ModifiesTyped] *= multiplicativeModifier.Value;

			return properties;
		}

		public static Dictionary<TEnum, float> DefaultProperties<TEnum>() where TEnum : Enum
		{
			var result = new Dictionary<TEnum, float>();

			foreach (var enumValue in Enum.GetValues(typeof(TEnum)))
			{
				result.Add((TEnum) enumValue, 0);
			}

			return result;
		}

		public static IList<T> Without<T>(this IEnumerable<T> list, List<int> index)
		{
			return list.Where((x, i) => !index.Contains(i)).ToList();
		}
	}
}