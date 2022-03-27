using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
namespace InternalLogic
{
	public class Inventory
	{
		public Body Body { get; private set; }
		public List<IEnemyAsEquipment> Unused { get; } = new List<IEnemyAsEquipment>();
		private List<IEnemyAsEquipment> Used { get; } = new List<IEnemyAsEquipment>();

		public event EventHandler? UnusedChange; 

		public Inventory()
		{
			Body = new Body(this);
		}

		public void Add(IEnemyAsEquipment enemyAsEquipment)
		{
			Unused.Add(enemyAsEquipment);
			UnusedChange?.Invoke(this, EventArgs.Empty);
		}
		
		public void Take<TEquipment>(TEquipment enemyAsEquipment)
		{
			var matchingEquipment = Unused.First(x => Equals(x.BodyEquipment, enemyAsEquipment) || Equals(x.GunEquipment, enemyAsEquipment));
			Unused.Remove(matchingEquipment);
			Used.Add(matchingEquipment);
			UnusedChange.Invoke(this, EventArgs.Empty);
		}

		public void Return<TEquipment, TModifier, TProperty>(TEquipment equipment)
			where TEquipment : IEquipment<TModifier, TProperty>
			where TModifier : IPropertyModifier<TProperty>
		{
			var matchingEquipment = Used.First(x => Equals(x.BodyEquipment, equipment) || Equals(x.GunEquipment, equipment));
			Used.Remove(matchingEquipment);
			Unused.Add(matchingEquipment);
			UnusedChange.Invoke(this, EventArgs.Empty);
		}
	}

	public interface IEnemyAsEquipment
	{
		IBodyEquipment? BodyEquipment { get; }
		IGunEquipment? GunEquipment { get; }
		GameObject InventoryHusk { get; }
	}

	public class EnemyAsEquipment : IEnemyAsEquipment
	{
		public IBodyEquipment? BodyEquipment { get; set; }
		public IGunEquipment? GunEquipment { get; set; }
		public IBulletEquipConfig? AsBulletEquipment { get; set; }
		public GameObject InventoryHusk => ((IEquipment?)BodyEquipment ?? GunEquipment)!.InventoryHusk;
	}
}