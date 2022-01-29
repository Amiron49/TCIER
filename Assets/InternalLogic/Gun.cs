using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace InternalLogic
{
	public class Gun : SlotLimitedInventory<IGunEquipment, IGunModifier, GunProperties>
	{
		protected override GunProperties SlotChangingProperty => GunProperties.Slots;
		public IBulletEquipConfig? Bullet { get; private set; }

		public Gun(Inventory inventory) : base(inventory)
		{
		}

		public void Equip(IBulletEquipConfig bulletEquipConfig)
		{
			Bullet = bulletEquipConfig;
		}

		public Dictionary<GunProperties, float> PreviewEquip(IBulletEquipConfig? bulletEquipConfig)
		{
			return bulletEquipConfig?.BaseStats.WithAddedModifiers(Equipped.SelectMany(x => x.Modifiers)) ??
				   CharacterPropertyHelper.DefaultProperties<GunProperties>();
		}

		public void Remove(IBulletEquipConfig bulletEquipConfig)
		{
			Bullet = bulletEquipConfig;
		}

		protected override Dictionary<GunProperties, float> BaseStats()
		{
			return Bullet?.BaseStats ?? CharacterPropertyHelper.DefaultProperties<GunProperties>();
		}
	}
}