using System;

namespace InternalLogic
{
	[Serializable]
	public class BodyModifier : IBodyModifier
	{
		public BodyProperties modifies;
		public ModifierType how;
		public float value;

		public string Modifies => Enum.GetName(typeof(BodyProperties), ModifiesTyped);
		public BodyProperties ModifiesTyped => modifies;
		public ModifierType How => how;
		public float Value => value;
	}
}