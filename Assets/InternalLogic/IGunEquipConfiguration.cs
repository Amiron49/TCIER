using System;
using System.Collections.Generic;

namespace InternalLogic
{
	public interface IGunEquipConfiguration
	{
		public string Description { get; }
		IEnumerable<IGunModifier> Modifiers { get; }
	}

	public interface IGunModifier
	{
		GunModifiers Modifies { get; }
		ModifierType How { get; }
		float Value { get; }
	}

	[Serializable]
	public class GunModifier : IGunModifier
	{
		public GunModifiers modifies;
		public ModifierType how;
		public float value;

		public GunModifiers Modifies => modifies;
		public ModifierType How => how;
		public float Value => value;
	}

	public enum ModifierType
	{
		Add,
		Multiply
	}
}