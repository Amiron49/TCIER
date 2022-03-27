using System;
using System.Collections.Generic;
using System.Linq;

namespace InternalLogic
{
	public sealed class Body : SlotLimitedEquipper<IBodyEquipment, IBodyModifier, BodyProperties>
	{
		public List<Gun> Guns { get; } = new();

		protected override BodyProperties SlotChangingProperty => BodyProperties.Slots;

		public event GunCountChanged OnGunCountChange;
		
		public Body(Inventory inventory) : base(inventory)
		{
			RecalculateProperties();
		}

		protected override void RecalculateProperties()
		{
			base.RecalculateProperties();
			RecalculateEmitters();
		}

		private void RecalculateEmitters()
		{
			var maxGuns = PropertiesTyped.Single(x => x.Key == BodyProperties.Emitters).Value;
			var change = (int)maxGuns- Guns.Count;

			if (change == 0)
				return;

			if (change > 0)
			{
				for (var i = 0; i < change; i++)
					Guns.Add(new Gun(Inventory));
			}
			else
			{
				var changeAbs = Math.Abs(change);
				Guns.RemoveRange(Guns.Count - changeAbs, changeAbs);
			}

			OnGunCountChange?.Invoke(this, change);
		}

		protected override Dictionary<BodyProperties, float> BaseStatsTyped()
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

		public override Dictionary<string, bool> IsPositiveChangeMap => new Dictionary<BodyProperties, bool>
		{
			{BodyProperties.Dashes, true},
			{BodyProperties.Emitters, true},
			{BodyProperties.Health, true},
			{BodyProperties.Slots, true},
			{BodyProperties.Speed, true},
			{BodyProperties.DamagingDashes, true}
		}.ToStringDictionary();
	}

	public delegate void GunCountChanged(object sender, int difference);
}