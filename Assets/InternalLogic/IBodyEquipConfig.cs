using System.Collections.Generic;

namespace InternalLogic
{
	public interface IBodyEquipConfig
	{
		public string Description { get; }
		public IEnumerable<IBodyModifier> Modifiers { get; }
	}
}