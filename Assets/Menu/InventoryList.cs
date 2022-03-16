using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace Menu
{
	public class InventoryList : MonoBehaviour
	{
		private List<StackedItemTile> currentTiles = new();
		public StackedItemTile tilePrefab;
		public StatsDisplayUI statsDisplayUI;
		[CanBeNull] 
		public GunIndexProvider gunIndexProvider;
		public ItemTypeFilter filter = ItemTypeFilter.Whatever;

		private void Awake()
		{
			Game.Instance.State.Inventory.UnusedChange += (_, _) => Refresh();
		}

		// Start is called before the first frame update
		void Start()
		{
		}

		void Refresh()
		{
			var fullList = Game.Instance.State.Inventory.Unused.Where(CreateSearch()).GroupBy(x => x.InventoryHusk).ToList();

			foreach (var currentTile in currentTiles)
				Destroy(currentTile.gameObject);

			currentTiles.Clear();

			foreach (var group in fullList)
			{
				var tileStack = Instantiate(tilePrefab, gameObject.transform);
				var tile = tileStack.GetComponent<ItemTile>();
				tileStack.SetAmount(group.Count());
				var enemyAsEquipment = @group.First();
				tile.InitialItemHusk = enemyAsEquipment.InventoryHusk;
				var equipTarget = GetEquipper();
				var equipment = GetEquipment(enemyAsEquipment);
				var equipController = new EquipController(equipTarget, equipment);

				equipController.OnHoverStart += (_, _) =>
				{
					var current = equipTarget.Properties;
					var preview = equipTarget.PreviewEquip(equipment);
					statsDisplayUI.DisplayPreview(current, preview, equipTarget.IsPositiveChangeMap);
				};
				
				equipController.OnHoverEnd += (_, _) =>
				{
					var current = equipTarget.Properties;
					statsDisplayUI.DisplayNormal(current);
				};

				tile.itemController = equipController;
				currentTiles.Add(tileStack);
			}
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