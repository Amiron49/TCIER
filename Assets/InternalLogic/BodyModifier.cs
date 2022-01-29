using System;

namespace InternalLogic
{
	[Serializable]
	public class BodyModifier : IBodyModifier
	{
		public BodyProperties modifies;
		public ModifierType how;
		public float value;

		public BodyProperties Modifies => modifies;
		public ModifierType How => how;
		public float Value => value;
	}
}