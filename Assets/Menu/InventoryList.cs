using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using JetBrains.Annotations;
using Menu.ItemTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
	public class InventoryList : MonoBehaviour
	{
		public StackedItemTile tilePrefab;
		[FormerlySerializedAs("statsDisplayUI")] public PropertiesDisplay propertiesDisplay;
		[CanBeNull] 
		public GunIndexProvider gunIndexProvider;
		public ItemTypeFilter filter = ItemTypeFilter.Whatever;
		private ChildrenSync<StackedItemTile, IEnemyAsEquipment, int> _childrenSync;

		// Start is called before the first frame update
		void Start()
		{
			_childrenSync = new ChildrenSync<StackedItemTile, IEnemyAsEquipment, int>(CreateItemTile, UpdateItemTile);
			Game.Instance.State.Inventory.UnusedChange += (_, _) => Refresh();
			Refresh();
		}
		
		void Refresh()
		{
			if (_childrenSync == null)
				return;
			
			var fullList = Game.Instance.State.Inventory.Unused.Where(CreateSearch())
				.GroupBy(x => x.InventoryHusk)
				.Select(x => (x.First(), x.Count())).ToList();

			_childrenSync.Update(fullList);
		}

		private StackedItemTile CreateItemTile(IEnemyAsEquipment enemyAsEquipment, int stackedCount)
		{
			var tileStack = Instantiate(tilePrefab, gameObject.transform);
			var tile = tileStack.GetComponent<ItemTile>();
			tileStack.SetAmount(stackedCount);
			tile.InitialItemHusk = enemyAsEquipment.InventoryHusk;
			var equipTarget = GetEquipper();
			var equipment = GetEquipment(enemyAsEquipment);
			var equipController = new EquipController(equipTarget, equipment);

			equipController.OnHoverStart += (_, _) =>
			{
				var current = equipTarget.Properties;
				var preview = equipTarget.PreviewEquip(equipment);
				propertiesDisplay.DisplayPreview(current, preview, equipTarget.IsPositiveChangeMap);
			};
				
			equipController.OnHoverEnd += (_, _) =>
			{
				var current = equipTarget.Properties;
				propertiesDisplay.DisplayNormal(current);
			};

			tile.itemController = equipController;

			return tileStack;
		}

		private void UpdateItemTile(StackedItemTile stackedItemTile, int updatedCount)
		{
			stackedItemTile.SetAmount(updatedCount);
		}
		
		private ISlotLimitedEquipper GetEquipper()
		{
			return filter switch
			{
				ItemTypeFilter.Whatever => throw new Exception("Cannot use this with whatever"),
				ItemTypeFilter.Body => Game.Instance.State.Inventory.Body,
				ItemTypeFilter.Gun => Game.Instance.State.Inventory.Body.Guns[gunIndexProvider!.gunIndex],
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private IEquipment GetEquipment(IEnemyAsEquipment enemyAsEquipment)
		{
			return filter switch
			{
				ItemTypeFilter.Whatever => throw new Exception("Whatever pls"),
				ItemTypeFilter.Body => enemyAsEquipment.BodyEquipment,
				ItemTypeFilter.Gun => enemyAsEquipment.GunEquipment,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
		
		private Func<IEnemyAsEquipment, bool> CreateSearch()
		{
			return filter switch
			{
				ItemTypeFilter.Whatever => _ => true,
				ItemTypeFilter.Body => x => x.BodyEquipment != null,
				ItemTypeFilter.Gun => x => x.GunEquipment != null,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		// Update is called once per frame
		void Update()
		{
		}
	}

	public enum ItemTypeFilter
	{
		Whatever,
		Body,
		Gun
	}

	public class EquipController : BaseItemController
	{
		private readonly ISlotLimitedEquipper _slotLimitedEquipper;
		private readonly IEquipment _equipment;

		public EquipController(ISlotLimitedEquipper slotLimitedEquipper, IEquipment equipment)
		{
			_slotLimitedEquipper = slotLimitedEquipper;
			_equipment = equipment;
		}

		public override bool UseInternal(ItemTile tile)
		{
			var error = _slotLimitedEquipper.Equip(_equipment);

			if (error != null)
			{
				tile.BubbleTextOnMe(error);
			}

			return error == null;
		}
	}
	
	public abstract class BaseItemController : IItemController
	{
		public bool Use(ItemTile tile)
		{
			var success = UseInternal(tile);

			if (success)
			{
				OnUseSuccess?.Invoke(this, EventArgs.Empty);
			}

			return success;
		}

		public abstract bool UseInternal(ItemTile tile);
		
		public event EventHandler OnHoverStart;
		public void HoverStart()
		{
			OnHoverStart?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler OnHoverEnd;
		public void HoverEnd()
		{
			OnHoverEnd?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler OnUseSuccess;
	}
	
	public class ItemController : BaseItemController
	{
		public override bool UseInternal(ItemTile tile)
		{
			return true;
		}
	}
}