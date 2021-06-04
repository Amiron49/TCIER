using System;

namespace InternalLogic
{
	[Serializable]
	public class BodyModifier : IBodyModifier
	{
		public BodyModifiers modifies;
		public ModifierType how;
		public float value;

		public BodyModifiers Modifies => modifies;
		public ModifierType How => how;
		public float Value => value;
	}
}