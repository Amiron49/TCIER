using System;
using System.Collections.Generic;
using System.Linq;

namespace InternalLogic
{
	public class Body : SlotLimitedInventory<IBodyEquipment, IBodyModifier, BodyProperties>
	{
		public List<Gun> Guns { get; } = new List<Gun>();

		protected override BodyProperties SlotChangingProperty => BodyProperties.Slots;

		public event GunCountChanged OnGunCountChange;
		
		public Body(Inventory inventory) : base(inventory)
		{
		}

		protected override void RecalculateProperties()
		{
			base.RecalculateProperties();
			var maxGuns = Properties.Single(x => x.Key == BodyProperties.Slots).Value;
			var gunOverflow = Guns.Count - (int)maxGuns;
			
			if (gunOverflow == 0)
				return;
			
			if (gunOverflow > 0)
			{
				Guns.RemoveRange(Guns.Count - gunOverflow, gunOverflow);
			}
			else
			{
				for (var i = 0; i < Math.Abs(gunOverflow); i++)
					Guns.Add(new Gun(Inventory));
			}

			OnGunCountChange?.Invoke(this, gunOverflow);
		}

		protected override Dictionary<BodyProperties, float> BaseStats()
		{
			return new Dictionary<BodyProperties, float>
			{
				{BodyProperties.Dashes, 2},
				{BodyProperties.Emitters, 1},
				{BodyProperties.Health, 100},
				{BodyProperties.Slots, 3},
				{BodyProperties.Speed, 8},
				{BodyProperties.DamagingDashes, 0}
			};
		}
	}

	public delegate void GunCountChanged(object sender, int difference);
}