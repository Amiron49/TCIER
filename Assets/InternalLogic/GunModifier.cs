using System;

namespace InternalLogic
{
	[Serializable]
	public class GunModifier : IGunModifier
	{
		public GunProperties modifies;
		public ModifierType how;
		public float value;

		public GunProperties Modifies => modifies;
		public ModifierType How => how;
		public float Value => value;
	}
}