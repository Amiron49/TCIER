using System;
using InternalLogic;

namespace InternalLogic
{
	public interface IPropertyModifier<out TProperty> : IPropertyModifier
	{
		TProperty ModifiesTyped { get; }
	}
	
	public interface IPropertyModifier
	{
		string Modifies { get; }
		ModifierType How { get; }
		float Value { get; }
	}
}

public static class TextHelpers {
	
	public static string Textify(this IPropertyModifier<GunProperties> property)
	{
		return $"{property.ModifiesTyped.Textify()} {property.How.Textify()} {property.Value}";
	}
	
	public static string Textify(this IPropertyModifier<BodyProperties> property)
	{
		return $"{property.ModifiesTyped.Textify()} {property.How.Textify()} {property.Value}";
	}
    
	public static string Textify(this BodyProperties property)
	{
		return property switch
		{
			BodyProperties.Health => "Health",
			BodyProperties.Slots => "Slots",
			BodyProperties.Speed => "Speed",
			BodyProperties.Dashes => "Dashes",
			BodyProperties.DamagingDashes => "DamagingDashes",
			BodyProperties.Emitters => "Guns",
			_ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
		};
	}
	
	public static string Textify(this GunProperties property)
	{
		return property switch
		{
			GunProperties.Cooldown => "Cooldown",
			GunProperties.WindUp => "WindUp",
			GunProperties.WindDown => "WindDown",
			GunProperties.Homing => "Homing",
			GunProperties.Velocity => "Velocity",
			GunProperties.Arcs => "Arcs",
			GunProperties.Projectiles => "Projectiles",
			GunProperties.Damage => "Damage",
			GunProperties.Pierce => "Pierce",
			GunProperties.ChargeShot => "ChargeShot",
			GunProperties.Accuracy => "Accuracy",
			GunProperties.Slots => "Slots",
			_ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
		};
	}
    
	public static string Textify(this ModifierType modifierType)
	{
		return modifierType switch
		{
			ModifierType.Add => "+",
			ModifierType.Multiply => "*",
			_ => throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null)
		};
	}
}