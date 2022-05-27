using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
namespace InternalLogic
{
	public sealed class Gun : SlotLimitedEquipper<IGunEquipment, IGunModifier, GunProperties>
	{
		private Vector2 _offset = Vector2.up;
		private Quaternion _rotation = Quaternion.identity;

		public event EventHandler<Vector2>? OnOffsetChange;

		public Vector2 Offset
		{
			get => _offset;
			set
			{
				_offset = value;
				OnOffsetChange?.Invoke(this, value);
			}
		}
		
		public event EventHandler<Quaternion>? OnRotationChange;

		public Quaternion Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				OnRotationChange?.Invoke(this, value);
			}
		}

		protected override GunProperties SlotChangingProperty => GunProperties.Slots;
		public IBulletEquipConfig? Bullet { get; private set; }
		public event EventHandler<IBulletEquipConfig>? OnBulletChange;

		public Gun(Inventory inventory) : base(inventory)
		{
			RecalculateProperties();
		}

		public void Equip(IBulletEquipConfig bulletEquipConfig)
		{
			Bullet = bulletEquipConfig;
			OnBulletChange?.Invoke(this, bulletEquipConfig);
			RecalculateProperties();
		}

		public Dictionary<GunProperties, float> PreviewEquip(IBulletEquipConfig? bulletEquipConfig)
		{
			return bulletEquipConfig?.BaseStats.WithAddedModifiers(EquippedTyped.SelectMany(x => x.ModifiersTyped)) ??
				   CharacterPropertyHelper.DefaultProperties<GunProperties>();
		}

		public void Remove(IBulletEquipConfig bulletEquipConfig)
		{
			Bullet = bulletEquipConfig;
		}

		protected override Dictionary<GunProperties, float> BaseStatsTyped()
		{
			var baseStats = new Dictionary<GunProperties, float>
			{
				{ GunProperties.Slots, 2 }
			};

			var bulletBaseStats = (Bullet?.BaseStats ?? CharacterPropertyHelper.DefaultProperties<GunProperties>()).Where(x => x.Key != GunProperties.Slots);

			return baseStats.Concat(bulletBaseStats).ToDictionary(x => x.Key, x => x.Value);
			;
		}

		public override Dictionary<string, bool> IsPositiveChangeMap => new Dictionary<GunProperties, bool>
		{
			{ GunProperties.Accuracy, true },
			{ GunProperties.Arcs, true },
			{ GunProperties.Cooldown, false },
			{ GunProperties.Damage, true },
			{ GunProperties.Homing, true },
			{ GunProperties.Pierce, true },
			{ GunProperties.Projectiles, true },
			{ GunProperties.Slots, true },
			{ GunProperties.Velocity, true },
			{ GunProperties.ChargeShot, true },
			{ GunProperties.WindDown, false },
			{ GunProperties.WindUp, false },
		}.ToStringDictionary();
	}
}