using System;

namespace InternalLogic
{
	[Serializable]
	public class GunModifier : IGunModifier
	{
		public GunProperties modifies;
		public ModifierType how;
		public float value;

		public string Modifies => Enum.GetName(typeof(GunProperties), ModifiesTyped);
		public GunProperties ModifiesTyped => modifies;
		public ModifierType How => how;
		public float Value => value;
	}
}