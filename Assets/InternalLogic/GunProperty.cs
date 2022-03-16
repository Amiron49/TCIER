using System;

namespace InternalLogic
{
	[Serializable]
	public class GunProperty : IGunProperty
	{
		public GunProperties property;
		public float value;
		public GunProperties Property => property;
		public float Value => value;
	}
}