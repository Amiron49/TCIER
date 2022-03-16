using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using Menu.ItemTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
	public class SlotLimitedEquipperUI : MonoBehaviour
	{
		private readonly List<ItemTile> _currentTiles = new();
		public ItemTypeFilter typeFilter = ItemTypeFilter.Body;
		public GunIndexProvider gunIndexProvider;
		public ItemTile equipmentPrefab;
		public ItemTile emptyItemTilePrefab;
		public GameObject xMarkPrefab;
		[FormerlySerializedAs("propertiesDisplayUI")] public PropertiesDisplay propertiesDisplay;

		private ISlotLimitedEquipper _slotLimitedEquipper;

		// Start is called before the first frame update
		void Start()
		{
			_slotLimitedEquipper = GetEquipper();
			_slotLimitedEquipper.OnPropertyChange += (_, _) => Refresh();
			Refresh();
		}

		public void Refresh()
		{
			var fullList = _slotLimitedEquipper.Equipped;

			foreach (var currentTile in _currentTiles)
				Destroy(currentTile.gameObject);

			_currentTiles.Clear();

			for (var index = 0; index < fullList.Count; index++)
			{
				var itemIndex = index;
				var equipment = fullList[index];
				var tile = Instantiate(equipmentPrefab, gameObject.transform);
				tile.InitialItemHusk = equipment.InventoryHusk;
				var unEquipItemController = new UnEquipItemController(_slotLimitedEquipper, index);
				tile.itemController = unEquipItemController;

				unEquipItemController.OnHoverStart += (_, _) =>
				{
					var currentProperties = _slotLimitedEquipper.Properties;
					var (properties, _, newMaxCapacity) = _slotLimitedEquipper.PreviewUnEquip(itemIndex);

					var amountOfXMarks = _slotLimitedEquipper.Capacity - newMaxCapacity;
					RenderXMarksOverLastXSlots(amountOfXMarks);
					RenderXMarkOverSlot(tile);
					propertiesDisplay.DisplayPreview(currentProperties, properties, _slotLimitedEquipper.IsPositiveChangeMap);
				};

				unEquipItemController.OnHoverEnd += (_, _) =>
				{
					RemoveXMarks();
					propertiesDisplay.DisplayNormal(_slotLimitedEquipper.Properties);
				};

				_currentTiles.Add(tile);
			}

			var equippedItemsCount = _currentTiles.Count;
			var maxCount = _slotLimitedEquipper.Capacity;

			var emptyCount = maxCount - equippedItemsCount;

			for (var i = 0; i < emptyCount; i++)
			{
				var emptyTile = Instantiate(emptyItemTilePrefab, gameObject.transform);
				_currentTiles.Add(emptyTile);
			}
		}

		private readonly List<GameObject> _xMarks = new();

		private void RenderXMarkOverSlot(Component tile)
		{
			var xMark = Instantiate(xMarkPrefab, tile.transform);
			_xMarks.Add(xMark);
		}


		private void RenderXMarksOverLastXSlots(int amount)
		{
			var slotsToDecorate = _currentTiles.AsEnumerable().Reverse().Take(amount);

			foreach (var itemTile in slotsToDecorate)
			{
				RenderXMarkOverSlot(itemTile);
			}
		}

		private void RemoveXMarks()
		{
			foreach (var xMark in _xMarks)
			{
				Destroy(xMark);
			}

			_xMarks.Clear();
		}

		// Update is called once per frame
		void Update()
		{
		}

		private ISlotLimitedEquipper GetEquipper()
		{
			return typeFilter switch
			{
				ItemTypeFilter.Body => Game.Instance.State.Inventory.Body,
				ItemTypeFilter.Gun => Game.Instance.State.Inventory.Body.Guns[gunIndexProvider.gunIndex],
				ItemTypeFilter.Whatever => throw new NotSupportedException("Whatever is not valid here as an item filter"),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

	}

	public class UnEquipItemController : BaseItemController
	{
		private readonly ISlotLimitedEquipper _limitedEquipper;
		private readonly int _index;

		public UnEquipItemController(ISlotLimitedEquipper limitedEquipper, int index)
		{
			_limitedEquipper = limitedEquipper;
			_index = index;
		}

		public override bool UseInternal(ItemTile tile)
		{
			var error = _limitedEquipper.UnEquip(_index);

			if (error != null)
			{
				tile.BubbleTextOnMe(error);
			}

			return error != null;
		}
	}
}