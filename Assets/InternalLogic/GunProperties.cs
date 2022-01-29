using System;
using UnityEngine;

namespace InternalLogic
{
	public enum GunProperties
	{
		Cooldown,
		WindUp,
		WindDown,
		Homing,
		Velocity,
		Arcs,
		Projectiles,
		Damage,
		Pierce,
		ChargeShot,
		Accuracy,
		Slots
	}

	public interface IGunProperty
	{
		GunProperties Property { get; }
		float Value { get; }
	}

	[Serializable]
	public class GunPropertyFoEditor : IGunProperty
	{
		[SerializeField]
		private GunProperties property;
		[SerializeField]
		private float value;

		public GunProperties Property => property;
		public float Value => value;
	}
}